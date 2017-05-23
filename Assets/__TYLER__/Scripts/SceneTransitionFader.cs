using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionFader : MonoBehaviour {

    private bool SceneStarting = true;

    public float FadeSpeed = 1.5f;

    private void Awake() {
        GetComponent<GUITexture>().pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (SceneStarting) {
            InitScene();
        }
    }

    void FadeToClear() {
        GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.clear, FadeSpeed * Time.deltaTime);
    }

    void FadeToBlack() {
        GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.black, FadeSpeed * Time.deltaTime);
    }

    void InitScene() {
        FadeToClear();

        if (GetComponent<GUITexture>().color.a <= 0.05f) {
            GetComponent<GUITexture>().color = Color.clear;
            GetComponent<GUITexture>().enabled = false;
            SceneStarting = false;
        }
    }

    public void EndScene() {
        GetComponent<GUITexture>().enabled = true;
        FadeToBlack();
        if (GetComponent<GUITexture>().color.a >= 0.95f) {
            SceneManager.LoadScene(1);
        }
    }
}
