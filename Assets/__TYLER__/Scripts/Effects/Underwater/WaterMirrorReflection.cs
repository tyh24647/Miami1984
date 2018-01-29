using UnityEngine;
using System.Collections;

/// <summary>
/// Water mirror reflection scripting object which applies the appropriate
/// skybox and environment reflections in the oceans/lakes.
/// 
/// Author: Tyler Hostager
/// 5/8/17
/// </summary>
[ExecuteInEditMode]
public class WaterMirrorReflection : MonoBehaviour {

    #region public vars

    public bool m_DisablePixelLights = true;
    public bool m_BackSide = false;
    public float m_ClipPaneOffset = 0.07f;
    public int m_TextureSize = 256;
    public LayerMask m_ReflectLayers = -1;

    #endregion

    #region private vars

    private Hashtable m_ReflectionCameras = new Hashtable();
    private RenderTexture m_ReflectionTexture = null;
    private int m_OldReflectionTextureSize = 0;
    private static bool s_InsideRendering = false;

    #endregion

    public void OnWillRenderObject() {

        // ensure all the properties are available for rendering
        if (!enabled || !GetComponent<Renderer>() || !GetComponent<Renderer>().sharedMaterial
            || !GetComponent<Renderer>().enabled) {
            return;
        }

        if (!Camera.current) {
            return;
        }

        Camera cam = Camera.current;

        // protect from recursive reflections
        if (s_InsideRendering) {
            return;
        }

        s_InsideRendering = true;

        Camera reflectionCam;
        CreateMirrorObjs(cam, out reflectionCam);
        reflectionCam.renderingPath = RenderingPath.Forward;

        // find the reflection plane
        Vector3 pos = transform.position;
        Vector3 normal = transform.up;
        if (m_BackSide) {
            normal = -normal;
        }

        UpdateCameraModes(cam, reflectionCam);

        // render the reflection--reflect camera around reflection plane
        float d = -Vector3.Dot(normal, pos) - m_ClipPaneOffset;
        Vector4 reflectionPlane = new Vector4(normal.x, normal.y, normal.z, d);
        Matrix4x4 reflection = Matrix4x4.zero;
        CalculateReflectionMatrix(ref reflection, reflectionPlane);
        Vector3 oldPos = cam.transform.position;
        Vector3 newPos = reflection.MultiplyPoint(oldPos);
        reflectionCam.worldToCameraMatrix = cam.worldToCameraMatrix * reflection;

        // setup oblique projection matrix
        Vector4 clippingPlane = CamSpcPlane(reflectionCam, pos, normal, 1.0f);
        Matrix4x4 projMat = cam.projectionMatrix;

        // perform calculations
        CalculateReflectionMatrix(ref projMat, clippingPlane);
        reflectionCam.projectionMatrix = projMat;
        reflectionCam.cullingMask = ~(1 << 4) & m_ReflectLayers.value;  // NEVER RENDER WATER LAYER
        reflectionCam.targetTexture = m_ReflectionTexture;

        // render changes with OpenGL using Euler angles
        GL.invertCulling = true;
        reflectionCam.transform.position = newPos;
        Vector3 euler = cam.transform.eulerAngles;
        reflectionCam.transform.eulerAngles = new Vector3(0, euler.y, euler.z);
        reflectionCam.Render();

        // revert back to previous position after rendering
        reflectionCam.transform.position = oldPos;
        GL.invertCulling = false;

        // set material texture rendering properties
        Material[] materials = GetComponent<Renderer>().sharedMaterials;

        foreach (Material mat in materials) {
            if (mat.HasProperty("_ReflectionTex")) {
                mat.SetTexture("_ReflectionTex", m_ReflectionTexture);
            }
        }

        // set matrix on the shader that transforms the UVs from object-space to the screen space
        // so that we just project the texture onto the screen.
        Matrix4x4 scaleOffset = Matrix4x4.TRS(
                                    new Vector3(0.5f, 0.5f, 0.5f),
                                    Quaternion.identity,
                                    new Vector3(0.5f, 0.5f, 0.5f)
                                );

        Vector3 scale = transform.lossyScale;
        Matrix4x4 mtx = transform.localToWorldMatrix * Matrix4x4.Scale(
                            new Vector3(
                                (1.0f / scale.x),
                                (1.0f / scale.y),
                                (1.0f / scale.z)
                            )
                        );

        mtx = scaleOffset * cam.projectionMatrix * cam.worldToCameraMatrix * mtx;

        foreach (Material mat in materials) {
            mat.SetMatrix("_ProjMatrix", mtx);
        }

        s_InsideRendering = false;
    }

    private void OnDisable() {
        if (m_ReflectionTexture) {
            DestroyImmediate(m_ReflectionTexture);
            m_ReflectionTexture = null;
        }

        foreach (DictionaryEntry entry in m_ReflectionCameras) {
            DestroyImmediate(((Camera)entry.Value).gameObject);
        }

        m_ReflectionCameras.Clear();
    }

    private static float sgn(float a) {
        return a > 0.0f ? 1.0f : a < 0.0f ? -1.0f : 0.0f;
    }

    private Vector4 CamSpcPlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign) {
        Vector3 offsetPos = pos + normal * m_ClipPaneOffset;
        Matrix4x4 matrix = cam.worldToCameraMatrix;
        Vector3 cPos = matrix.MultiplyPoint(offsetPos);
        Vector3 cNormal = matrix.MultiplyVector(normal).normalized * sideSign;
        return new Vector4(cNormal.x, cNormal.y, cNormal.z, -Vector3.Dot(cPos, cNormal));
    }

    private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPane) {
        Vector4 q = projection.inverse * new Vector4(
                        sgn(clipPane.x),
                        sgn(clipPane.y),
                        1.0f,
                        1.0f
                    );
        Vector4 c = clipPane * (2.0F / (Vector4.Dot(clipPane, q)));

        // third row --> clip pane - fourth row
        projection[2] = c.x - projection[3];
        projection[6] = c.y - projection[7];
        projection[10] = c.z - projection[11];
        projection[14] = c.w - projection[15];
    }

    private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane) {
        reflectionMat.m00 = (1F - 2F * plane[0] * plane[0]);
        reflectionMat.m01 = (-2F * plane[0] * plane[1]);
        reflectionMat.m02 = (-2F * plane[0] * plane[2]);
        reflectionMat.m03 = (-2F * plane[3] * plane[0]);

        reflectionMat.m10 = (-2F * plane[1] * plane[0]);
        reflectionMat.m11 = (1F - 2F * plane[1] * plane[1]);
        reflectionMat.m12 = (-2F * plane[1] * plane[2]);
        reflectionMat.m13 = (-2F * plane[3] * plane[1]);

        reflectionMat.m20 = (-2F * plane[2] * plane[0]);
        reflectionMat.m21 = (-2F * plane[2] * plane[1]);
        reflectionMat.m22 = (1F - 2F * plane[2] * plane[2]);
        reflectionMat.m23 = (-2F * plane[3] * plane[2]);

        reflectionMat.m30 = 0F;
        reflectionMat.m31 = 0F;
        reflectionMat.m32 = 0F;
        reflectionMat.m33 = 1F;
    }

    /// <summary>
    /// Creates any object as a mirror for any object that we might need
    /// </summary>
    /// <param name="currentCam">Current cam.</param>
    /// <param name="reflectionCam">Reflection cam.</param>
    private void CreateMirrorObjs(Camera currentCam, out Camera reflectionCam) {
        reflectionCam = null;

        // reflection render texture
        if (!m_ReflectionTexture || m_OldReflectionTextureSize != m_TextureSize) {
            if (m_ReflectionTexture) {
                DestroyImmediate(m_ReflectionTexture);
            }

            m_ReflectionTexture = new RenderTexture(m_TextureSize, m_TextureSize, 16);
            m_ReflectionTexture.name = "__MirrorReflection" + GetInstanceID();
            m_ReflectionTexture.isPowerOfTwo = true;
            m_ReflectionTexture.hideFlags = HideFlags.DontSave;
            m_OldReflectionTextureSize = m_TextureSize;
        }

        // cam for reflection
        reflectionCam = m_ReflectionCameras[currentCam] as Camera;
        if (!reflectionCam) {
            GameObject gObj = new GameObject("Mirror Refl Camera id" + GetInstanceID()
                              + " for " + currentCam.GetInstanceID(), typeof(Camera), typeof(Skybox));
            reflectionCam = gObj.GetComponent<Camera>();
            reflectionCam.enabled = false;
            reflectionCam.transform.position = transform.position;
            reflectionCam.transform.rotation = transform.rotation;
            reflectionCam.gameObject.AddComponent<FlareLayer>();
            gObj.hideFlags = HideFlags.HideAndDontSave;
            m_ReflectionCameras[currentCam] = reflectionCam;
        }
    }

    /// <summary>
    /// Updates the camera modes.
    /// </summary>
    /// <param name="source">Source.</param>
    /// <param name="dest">Destination.</param>
    private void UpdateCameraModes(Camera source, Camera dest) {
        if (!dest)
            return;
        dest.clearFlags = source.clearFlags;
        dest.backgroundColor = source.backgroundColor;

        if (source.clearFlags == CameraClearFlags.Skybox) {
            Skybox sky = source.GetComponent(typeof(Skybox)) as Skybox;
            Skybox mySky = dest.GetComponent(typeof(Skybox)) as Skybox;

            if (!sky || !sky.material) {
                mySky.enabled = false;
            } else {
                mySky.enabled = true;
                mySky.material = sky.material;
            }
        }

        // sync settings/update other values to match the current camera
        dest.farClipPlane = source.farClipPlane;
        dest.nearClipPlane = source.nearClipPlane;
        dest.orthographic = source.orthographic;
        dest.fieldOfView = source.fieldOfView;
        dest.aspect = source.aspect;
        dest.orthographicSize = source.orthographicSize;
    }
}

