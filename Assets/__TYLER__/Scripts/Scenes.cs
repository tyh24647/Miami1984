using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Custom scene manager for loading and accessing scene-related items such as
/// loading new scenes, retrieving scene indices, etc.
/// 
/// Author: Tyler Hostager
/// Version: 5/19/17
/// </summary>
public class Scenes : MonoBehaviour {

    private const int DefaultStartingSceneIndex = 1;

    // create scene manager singleton
    private static Scenes ScenesSingleton = null;
    public static Scenes Instance {
        get { return ScenesSingleton; }
    }


    private static AudioSettings _AudioSettings = null;
    public static AudioSettings AudioSettings {
        get { return _AudioSettings; }
    }

    private void Awake() {
        LoadInitialScene();
    }

    private void Start() {

    }

    private void Update() {

    }

    public void LoadInitialScene() {
        if (SceneManager.GetSceneByBuildIndex(1) != null) {
            for (var i = DefaultStartingSceneIndex; i < SceneManager.sceneCount; i++) {
                if (i == 1) {
                    continue;
                }

                if (SceneManager.GetSceneAt(i).isLoaded) {
                    //SceneManager.UnloadSceneAsync(i);
                    StartCoroutine(AsyncUnloadSceneWithIndex(i));
                }
            }

            Log.d("Loading initial scene");
            SceneManager.LoadScene(1);
        }
    }

    public void AsyncLoadInitialScene() {
        for (var i = DefaultStartingSceneIndex; i < SceneManager.sceneCount; i++) {
            if (i == 1) {
                continue;
            }

            if (SceneManager.GetSceneAt(i).isLoaded) {
                //SceneManager.UnloadSceneAsync(i);
                StartCoroutine(AsyncUnloadSceneWithIndex(i));
            }
        }

        Log.d("Loading initial scene");
        StartCoroutine(AsyncLoadSceneWithIndex(1));
    }


    private IEnumerator AsyncLoadSceneWithIndex(int index) {
        Log.d("AsyncOperation -> Loading scene \'" + SceneManager.GetSceneAt(index).name + "\'...");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
        while (!asyncLoad.isDone) {
            yield return null;

            // TODO
        }
    }

    private IEnumerator AsyncLoadSceneWithName(string sceneName) {
        Log.d("AsyncOperation -> Loading scene \'" + SceneManager.GetSceneByName(sceneName).name + "\'...");
        AsyncOperation asyncload = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncload.isDone) {
            yield return null;

            // TODO
        }
    }

    private IEnumerator AsyncUnloadSceneWithIndex(int index) {
        Log.d("AsyncOperation -> Unloading scene \'" + SceneManager.GetSceneAt(index).name + "\'...");
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(index);
        while (!asyncUnload.isDone) {
            yield return null;

            // TODO 
        }
    }

    private IEnumerator AsyncUnloadSceneWithName(string sceneName) {
        Log.d("AsyncOperation -> Unloading scene \'" + SceneManager.GetSceneByName(sceneName).name + "\'...");
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncUnload.isDone) {
            yield return null;

            // TODO
        }
    }
}
