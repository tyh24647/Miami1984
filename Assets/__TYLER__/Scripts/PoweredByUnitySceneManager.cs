using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class provides the loading functionality of the main menu
/// after showing the 'Powered by Unity' scene.
/// 
/// Author: Tyler Hostager
/// Version: 5/16/17
/// </summary>
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(GameObject))]
public class PoweredByUnitySceneManager : MonoBehaviour {
    private const int DefaultDisplayTime_Seconds = 15;

    private Camera DuplicateMainCam;
    private bool IsPaused = true;
    private float Alpha = 1.0f;
    private int FadeDir = -1;
    private float alphaFadeValue = 1;

    [Space(1)]
    public int DisplayTime = 0;

    [Space(1)]
    public GameObject BackgroundObject;

    public Camera MainCam;

    [Space(1)]
    [Header("Fade Options")]
    public Texture2D FadeTexture;
    public float FadeSpeed = 0.2f;
    public float DrawDepth = -1000f;

    private void Awake() {

        //if (BackgroundObject == null) {
        //    BackgroundObject = gameObject;
        //}

        //if (MainCam) {
        //    DuplicateMainCam = MainCam;
        //    DuplicateMainCam.enabled = false;
        //}

        //if (DisplayTime == 0 && Application.isPlaying) {
        //    DisplayTime = DefaultDisplayTime_Seconds;
        //}

        //if (FadeTexture == null) {
        //    FadeTexture = new Texture2D(Screen.width, Screen.height);
        //    FadeTexture.SetPixels(0, 0, Screen.width, Screen.height, new Color[] {
        //        Color.black
        //    });
        //}

        //DuplicateMainCam.enabled = true;
        //this.MainCam = DuplicateMainCam;
        //this.MainCam = BackgroundObject.GetComponent<Camera>();


    }

    // Use this for initialization
    void Start() {
        // start with black screen as you load gradually
        //MainCam.enabled = true;
        //Camera originalCam = MainCam;
        //Invoke("PauseOnBlackScreen", 4);
        //IsPaused = false;
        //MainCam.enabled = true;
        //FadeInScene(originalCam);
        //Invoke("PauseOnLogo", DefaultDisplayTime_Seconds);
        //FadeOutScene();
        //Invoke("LoadMainMenuAfterPause", DisplayTime == 0 ? DefaultDisplayTime_Seconds : DisplayTime);

        Invoke("LoadMainMenuAfterPause", DisplayTime == 0 ? DefaultDisplayTime_Seconds : DisplayTime);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnGUI() {
        InitSceneQuickExplorer();
    }

    private void InitSceneQuickExplorer() {
        if (GUI.Button(
            new Rect(
                ((Screen.width / 2) - 50),
                (Screen.height - 50),
                100,
                40), "Jump to main menu")) {
            SceneManager.LoadScene(3);
        }
    }


    private void PauseOnBlackScreen() {
        this.IsPaused = true;


        Camera dummyCam = new Camera() {
            aspect = (16 / 9),
            allowHDR = false,
            allowMSAA = false,
            rect = new Rect() {
                height = 0,
                width = 0
            }
        };

        Texture2D tex = new Texture2D(Screen.width, Screen.height);
        for (int i = 0; i < tex.width; i++) {
            for (int j = 0; j < tex.height; j++) {
                tex.SetPixel(i, j, Color.black);
            }
        }

        tex.Apply();

        GUIStyle tmp_genericStyle = new GUIStyle();
        GUI.skin.box = tmp_genericStyle;
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), tex);
    }

    private void PauseOnLogo() {
        this.IsPaused = true;

        // TODO: add zoom-in
    }

    private void FadeOutScene() {
        //SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        SceneManager.LoadScene(2, LoadSceneMode.Single);
        //StartCoroutine(AsyncLoadSceneWithIndex(2, LoadSceneMode.Single));
    }

    private IEnumerator AsyncLoadSceneWithIndex(int index, LoadSceneMode loadSceneMode) {
        Log.d("AsyncOperation -> Loading scene \'" + SceneManager.GetSceneAt(index).name + "\'...");
        AsyncOperation loadTask = SceneManager.LoadSceneAsync(index, loadSceneMode);
        while (!loadTask.isDone) {
            yield return null;

            // TODO
        }
    }

    private void FadeInScene() {
        SceneManager.LoadScene(2);


        // TODO

        //this.MainCam = cam;
        //this.MainCam.enabled = true;
    }

    //private void FadeOutScene() {
    //SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
    //StartCoroutine(SceneManager.LoadSceneAsync(2, LoadSceneMode.Single));
    //StartCoroutine(
    //bool finishedCallback = false;

    //Invoke("FadeOutExec", PoweredByUnitySceneManager.DefaultDisplayTime_Seconds);


    //while (!finishedCallback) {
    //    var result = FadeoutExec(
    //        this.transform,
    //        this.Alpha,
    //        true,
    //        PoweredByUnitySceneManager.DefaultDisplayTime_Seconds
    //    );
    //}


    //StartCoroutine(FadeOutExec(this.transform, this.Alpha, true, PoweredByUnitySceneManager.DefaultDisplayTime_Seconds));


    //}


    //public static bool FadeOutExec(this Transform targetTransform, float targetAlpha, bool isVanish, float duration) {
    //public static IEnumerator bool FadeOutExec(
    //    this Transform targetTransform, float targetAlpha, bool isVanish, float duration) {


    //Renderer mainRenderer = targetTransform.GetComponent<Renderer>();
    //float diffAlpha = (targetAlpha - mainRenderer.material.color.a);

    //float counter = 0;
    //while (counter < duration) {
    //    float alphaAmount = mainRenderer.material.color.a + (Time.deltaTime * diffAlpha) / duration;
    //    mainRenderer.material.color = new Color(mainRenderer.material.color.r, mainRenderer.material.color.g, mainRenderer.material.color.b, alphaAmount);

    //    counter += Time.deltaTime;
    //    yield return null;
    //}

    //mainRenderer.material.color = new Color(mainRenderer.material.color.r, mainRenderer.material.color.g, mainRenderer.material.color.b, targetAlpha);
    //if (isVanish) {
    //    mainRenderer.transform.gameObject.SetActive(false);
    //}


    //    Texture2D tex = new Texture2D(Screen.width, Screen.height);
    //    for (int i = 0; i < tex.width; i++) {
    //        for (int j = 0; j < tex.height; j++) {
    //            tex.SetPixel(i, j, Color.black);
    //        }
    //    }

    //    tex.Apply();

    //    alphaFadeValue -= Mathf.Clamp01(Time.deltaTime / 5);
    //    GUI.color = new Color(alphaFadeValue, alphaFadeValue, alphaFadeValue, alphaFadeValue);
    //    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);
    //    return true;
    //}


    void LoadMainMenuAfterPause() {
        this.IsPaused = false;
        FadeOutScene();
    }


}
