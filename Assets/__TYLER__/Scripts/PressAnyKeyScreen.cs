using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 'Press any key to continue' screen customizations
/// 
/// Author: Tyler Hostager
/// Version: 5/19/17
/// </summary>
public class PressAnyKeyScreen : MonoBehaviour {

    // Use this for initialization
    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update() {
        if (Input.anyKeyDown) {
            //SceneManager.LoadScene(3);
            StartCoroutine(AsyncLoadMainMenu());
        }
    }

    IEnumerator AsyncLoadMainMenu() {
        Log.d("Loading main menu...");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(3);
        while (!asyncLoad.isDone) {
            yield return null;

            // TODO
        }

        Log.d("Main menu loaded successfully");
    }
}
