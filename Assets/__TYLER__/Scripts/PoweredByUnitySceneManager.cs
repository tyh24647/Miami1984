using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

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

    #region CONSTANTS
    private const int DefaultDisplayTime_Seconds = 15;
    #endregion

    #region PUBLIC ATTRIBUTES
    [Space(1)]
    [Header("General Settings")]
    public int DisplayTime = 0;
    public int MaxAutoDetectionAttempts = 3;

    [Space(1)]
    public GameObject BackgroundObject;
    public Camera MainCam;

    [Space(1)]
    [Header("Main Background Options")]
    public GameObject MainBackgroundPanel;
    public float BackgroundFadeTime = 3.0f;
    public float MinBackgroundAlpha = 0.0f;
    public float MaxBackgroundAlpha = 1.0f;

    [Space(1)]
    [Header("Fade out options")]
    public float FadeStartTime = 12.0f;
    //public float FadeOutDuration = 3.0f;
    public float FadeOutDuration = 15f;
    public float SceneStartAlpha = 1.0f;
    public float SceneEndAlpha = 0.0f;

    [Space(1)]
    [Header("Video Settings")]
    public VideoPlayer VidPlayer;
    public VideoClip VidClip;
    public GameObject EightiesBackgroundObj;
    public float VidFadeTime = 15.0f;
    public float VidMinAlpha = 0.0f;
    public float VidMaxAlpha = 1.0f;
    #endregion

    #region PRIVATE ATTRIBUTES
    private SpriteRenderer SpRenderer;
    private SpriteRenderer BackgroundRenderer;
    private float StartTime;
    #endregion

    #region INHERITED METHODS
    private void Awake() {
        VidMinAlpha = 0.0f;
    }

    // Use this for initialization
    void Start() {
        StartTime = Time.time;
        Invoke("LoadMainMenuAfterPause", DisplayTime == 0 ? DefaultDisplayTime_Seconds : DisplayTime);
        ConfigureMainBackground();
        ConfigureVideo();
        PlayBackgroundVideo();
    }

    // Update is called once per frame
    void Update() {
        FadeInMainBackgroundObj();
        FadeInVideoObj();
        //FadeOutSceneObjs();
    }

    private void OnGUI() {
        InitSceneQuickExplorer();
    }
    #endregion

    #region FUNCTIONS
    private void ConfigureMainBackground() {
        var numMainBackgroundChecks = 0;

    FindMainBackgroundObj:
        if (MainBackgroundPanel != null && numMainBackgroundChecks < MaxAutoDetectionAttempts) {
            if (MainBackgroundPanel.GetComponent<SpriteRenderer>() != null) {
                BackgroundRenderer = MainBackgroundPanel.GetComponent<SpriteRenderer>();
            }
        } else if (numMainBackgroundChecks == 1) {
            var tmp = GameObject.Find("Background").gameObject;
            if (tmp) {
                this.MainBackgroundPanel = tmp;
            } else {
                var objArr = this.MainBackgroundPanel.gameObject.GetComponents<GameObject>();
                if (!ObjUtils.IsNullOrEmpty(objArr)) {
                    this.MainBackgroundPanel = objArr.FirstOrDefault(
                        (panel) => panel.name.ToLower().Equals("background")
                    );
                }
            }

            numMainBackgroundChecks++;
            goto FindMainBackgroundObj;
        }
    }

    private void ConfigureVideo() {
        var numBackgroundChecks = 0;

    FindEightiesBackgroundObj:
        if (EightiesBackgroundObj != null && numBackgroundChecks < MaxAutoDetectionAttempts) {
            if (EightiesBackgroundObj.GetComponent<SpriteRenderer>() != null) {
                SpRenderer = EightiesBackgroundObj.GetComponent<SpriteRenderer>();
            }
        } else if (numBackgroundChecks == 1) {
            var tmp = GameObject.Find("EightiesBackgroundObj").gameObject;
            if (tmp) {
                this.EightiesBackgroundObj = tmp;
            } else {
                var objArr = this.BackgroundObject.gameObject.GetComponents<GameObject>();

                if (!ObjUtils.IsNullOrEmpty(objArr)) {
                    this.EightiesBackgroundObj = objArr.FirstOrDefault(
                        (panel) => panel.name.ToLower().Equals("EightiesBackgroundObj".ToLower())
                    );
                }
            }

            numBackgroundChecks++;
            goto FindEightiesBackgroundObj;
        }
    }

    private void PlayBackgroundVideo() {
        if (VidClip != null && EightiesBackgroundObj != null) {
            VidPlayer.clip = VidClip;
            VidPlayer.targetMaterialRenderer = SpRenderer;
            VidPlayer.Play();
        }

        if (VidClip == null) {
            Log.w("Video not found. Please assign one in the inspector and re-run");
        }

        if (EightiesBackgroundObj == null) {
            Log.w("Eighties background object not found. Please assign one in the inspector and re-run");
        }
    }

    // not currently working
    private void FadeOutSceneObjs() {


        // TODO fix fade-out


        //float deltaT = (Time.time - StartTime);
        float time = (Time.time - FadeStartTime) / FadeOutDuration;

        if (BackgroundRenderer && SpRenderer) {
            try {
                //if (deltaT.CompareTo(FadeStartTime) == 0) {
                BackgroundRenderer.color = new Color {
                    r = 1.0f,
                    g = 1.0f,
                    b = 1.0f,
                    a = Mathf.SmoothStep(
                        from: this.SceneStartAlpha,
                        to: this.SceneEndAlpha,
                        t: FadeOutDuration
                    )
                };

                SpRenderer.color = BackgroundRenderer.color;
                //}
            } catch (Exception e) {
                Log.w("Unable to fade out scene objects");
                Log.e(e.Message, e);
            }
        }
    }

    private void FadeInMainBackgroundObj() {
        float time = (Time.time - StartTime) / BackgroundFadeTime;

        if (BackgroundRenderer) {
            try {
                BackgroundRenderer.color = new Color {
                    r = 1.0f,
                    g = 1.0f,
                    b = 1.0f,
                    a = Mathf.SmoothStep(
                        from: MinBackgroundAlpha,
                        to: MaxBackgroundAlpha,
                        t: time
                    )
                };
            } catch (Exception e) {
                Log.w("Unable to fade in main background object");
                Log.e(e.Message, e);
            }
        }
    }

    private void FadeInVideoObj() {
        float time = (Time.time - StartTime) / VidFadeTime;

        if (SpRenderer) {
            try {
                SpRenderer.color = new Color {
                    r = 1.0f,
                    g = 1.0f,
                    b = 1.0f,
                    a = Mathf.SmoothStep(
                        from: VidMinAlpha,
                        to: VidMaxAlpha,
                        t: time
                    )
                };
            } catch (NullReferenceException e) {
                Log.w("Unable to fade in video object");
                Log.e(e.Message, e);
            }
        }
    }

    private void InitSceneQuickExplorer() {
        if (GUI.Button(
            position: new Rect {
                x = ((Screen.width / 2) - 50),
                y = (Screen.height - 50),
                width = 100,
                height = 40
            },

            text: "Jump to main menu")) {
            SceneManager.LoadScene(3);
        }
    }


    private void PauseOnBlackScreen() {
        //this.IsPaused = true;

        Camera dummyCam = new Camera {
            aspect = (16 / 9),
            allowHDR = false,
            allowMSAA = false,
            rect = new Rect {
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

        GUI.Box(
            position: new Rect {
                x = 0,
                y = 0,
                width = Screen.width,
                height = Screen.height
            },

            image: tex
        );
    }

    private void PauseOnLogo() {
        //this.IsPaused = true;

        // TODO: add zoom-in
    }

    private void FadeOutScene() {
        SceneManager.LoadScene(2, LoadSceneMode.Single);

        // TODO Consider changing this to an async task

        //SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
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
        //this.IsPaused = false;
        FadeOutScene();
    }

    #endregion
}
