using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackingScreen : MonoBehaviour {

    // Use this for initialization
    void Start() {
        Invoke("StartUELogoScene", 3);
    }

    void StartUELogoScene() {
        SceneManager.LoadScene(0);
    }
}
