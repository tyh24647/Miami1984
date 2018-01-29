using UnityEngine;

namespace RetroAesthetics {
    public class UVScroller : MonoBehaviour {
        public Vector2 scrollSpeed = new Vector2(-1f, 0f);
        public string textureName = "_GridTex";
        private Material target;
        private Vector2 offset = Vector2.zero;
        private float YRotation;

        void Start() {
            var aRenderer = GetComponent<Renderer>();
            if (aRenderer == null || aRenderer.sharedMaterial == null) {
                this.enabled = false;
                return;
            }

            target = aRenderer.sharedMaterial;
            if (!target.HasProperty(textureName)) {
                Debug.LogWarning("Texture name '" + textureName + "' not found in material.");
                this.enabled = false;
                return;
            }

            this.YRotation = 0.0f;
        }

        void Update() {
            //YRotation += 0.0015f;  // rotate triangle 1/2 of one degree per frame
            offset += scrollSpeed * Time.deltaTime * Application.targetFrameRate;
            target.SetTextureOffset(textureName, offset);
            var delorean = GameObject.Find("Delorean 1");
            if (delorean != null) {
                Log.d("Attempting to rotate delorean...");
                //delorean.transform.localRotation = Quaternion.AngleAxis(YRotation, Vector3.up);
                //delorean.transform.Rotate(Quaternion.Euler(0, YRotation, 0));

#pragma warning disable CS0618 // Type or member is obsolete
                delorean.transform.RotateAroundLocal(Vector3.up, 0.05f);
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }
    }
}