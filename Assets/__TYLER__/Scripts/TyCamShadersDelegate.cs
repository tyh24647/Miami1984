using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Camera))]
public class TyCamShadersDelegate : MonoBehaviour {

    public List<Shader> Shaders;

    [Space(1)]
    [Header("Render Target(s): ")]
    public Terrain CurrentTerrain;
    public GameObject RenderObject;

    private Camera playerCam;

    private void Awake() {
        if (this.gameObject.GetComponent<Camera>() != null) {
            playerCam = gameObject.GetComponent<Camera>();

            if (AssetDatabase.FindAssets("shaders", AssetDatabase.GetSubFolders("__TYLER__")).Length > 0) {
                //for (var shader in Ass)

            }
        }
    }

    // Use this for initialization
    void Start() {
        if (this.Shaders.Count > 0) {
            foreach (var shader in this.Shaders) {
                if (shader != null && playerCam != null && playerCam.isActiveAndEnabled) {
                    Color materialColor;

                    switch (shader.name) {
                        case "Ty80sGridPattern.shader":
                            materialColor = Color.black;
                            break;
                        default:
                            materialColor = Color.grey;
                            break;
                    }

                    Material mat = new Material(Shader.Find(shader.name));
                    if (mat) {
                        mat.color = Color.black;

                        if (CurrentTerrain) {
                            Renderer tRenderer = CurrentTerrain.GetComponent<Renderer>();
                            if (tRenderer) {
                                CurrentTerrain.GetComponent<Renderer>().material = mat;
                            }
                        }

                        if (RenderObject) {
                            Renderer objRenderer = RenderObject.GetComponent<Renderer>();
                            if (objRenderer) {
                                RenderObject.GetComponent<Renderer>().material = mat;
                            }
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
