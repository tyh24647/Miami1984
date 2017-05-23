using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads scene on UI Button click.
/// 
/// Author: Tyler Hostager
/// Version: 5/19/17
/// </summary>
public class LoadSceneOnClick : MonoBehaviour {

    public Canvas canvas;

    private void Start() {
        ConfigureCanvas();
    }

    private void Update() {

    }

    public void LoadByIndex(int sceneIndex) {
        ConfigureCanvas();
        var sceneToLoad = SceneManager.GetSceneByBuildIndex(sceneIndex);
        if (sceneToLoad != null) {
            CheckMusicInstance();
            SceneManager.LoadScene(sceneIndex);
        }
    }

    public void LoadByIndex(int sceneIndex, bool isAsync) {
        ConfigureCanvas();
        if (isAsync) {
            //StartCoroutine(AsyncLoadByIndex(sceneIndex));
            AsyncLoadByIndex(sceneIndex);
        }

        LoadByIndex(sceneIndex);
    }

    public void AsyncLoadByIndex(int sceneIndex) {
        ConfigureCanvas();
        var sceneToLoad = SceneManager.GetSceneByBuildIndex(sceneIndex);
        if (sceneToLoad != null) {
            CheckMusicInstance();
            StartCoroutine(AsyncExecuteLoadWithIndex(sceneIndex));
        }
    }

    private IEnumerator AsyncExecuteLoadWithIndex(int sceneIndex) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        Log.d("Loading scene...");

        while (!asyncLoad.isDone) {
            // TODO Add loading screen here

            yield return null;
        }

        Log.d("Scene loaded");
    }

    public void LoadByName(string sceneName) {
        ConfigureCanvas();
        var sceneToLoad = SceneManager.GetSceneByName(sceneName);
        if (sceneToLoad != null) {
            CheckMusicInstance();
            SceneManager.LoadScene(sceneName);
        }
    }

    public void LoadByName(string sceneName, bool isAsync) {
        ConfigureCanvas();
        if (isAsync) {
            AsyncLoadByName(sceneName);
        }

        LoadByName(sceneName);
    }

    public void AsyncLoadByName(string sceneName) {
        ConfigureCanvas();
        var sceneToLoad = SceneManager.GetSceneByName(sceneName);
        if (sceneToLoad != null) {
            CheckMusicInstance();
            StartCoroutine(AsyncExecuteLoadWithName(sceneName));
        }
    }

    private IEnumerator AsyncExecuteLoadWithName(string sceneName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone) {
            // TODO add loading screen here

            yield return null;
        }
    }

    private void ConfigureCanvas() {
        if (!canvas) {
            Log.w("No \'Canvas\' attribute found. Attempting to auto-detect canvas...");
            try {
                //var parentCanvas = FindObjectOfType(typeof(Canvas));
                var parentCanvas = GetComponentInParent<Canvas>();
                if (parentCanvas) {
                    Log.d("Canvas successfully identified");
                    canvas = parentCanvas;
                    Log.d("Canvas initialized successfully");
                }
            } catch (UnassignedReferenceException e) {
                Log.w("Exception thrown in canvas configuration");
                Log.d("Please add a canvas object manually in the inspector and re");
                Log.d("Exception details: " + e);
            }
        }
    }

    private void CheckMusicInstance() {
        //var c = gameObject;
        if (name.Equals("CreateGameButton")) {
            //if (canvas.GetComponent<Button>().name.Equals("NewGameButton")) {
            if (GameObject.FindWithTag("IntroMusic")) {
                Log.d("Destroying music instance...");
                Destroy(GameObject.FindWithTag("IntroMusic"));
                Log.d("Instance Destroyed");

                // just making sure there aren't two instances...
                if (GameObject.FindWithTag("IntroMusic")) {
                    //Log.d("Recursing until all \'IntroMusic\' instances have been destroyed");
                    //LoadByIndex(sceneIndex);
                    Log.d("Destroying music instance...");
                    Destroy(GameObject.FindWithTag("IntroMusic"));
                    Log.d("Instance Destroyed");
                }
            }
        }
    }

    public void ExitGame() {
        // TODO save application settings/defaults before exit

        Log.d("Exiting game");
        Application.Quit();
#if DEBUG
        if (Application.isPlaying) {
            // TODO
        }
#endif
    }
}
