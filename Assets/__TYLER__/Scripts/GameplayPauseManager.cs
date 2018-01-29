using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayPauseManager : MonoBehaviour {
    public GameObject Canvas;
    public Camera Cam;
    public bool IsPaused = false;

    private void Awake() {
        if (!Canvas) {
            this.Canvas = FindCanvasInScene();
        }

        if (!Cam) {
            this.Cam = FindCameraInScene();
        }
    }

    // Use this for initialization
    void Start() {

        var numCanvasChecks = 0;
    CanvasConfiguration:
        if (Canvas && Cam) {
            ConfigurePlayerCanvas();
        } else if (Canvas) {
            this.Cam = FindCameraInScene();
            numCanvasChecks++;
            goto CanvasConfiguration;
        } else if (Cam) {
            this.Canvas = FindCanvasInScene();
            numCanvasChecks++;
            goto CanvasConfiguration;
        } else if (numCanvasChecks >= 2) {
            Log.w("Unable to locate the appropriate gameobject(s). Skipping procedure");
            return;
        } else {
            this.Canvas = FindCanvasInScene();
            this.Cam = FindCameraInScene();
            numCanvasChecks++;
            goto CanvasConfiguration;
        }
    }

    // Update is called once per frame
    void Update() {
#if DEBUG
        if (Input.GetKeyDown(KeyCode.P)) {
#else
        if (Input.GetKeyDown(KeyCode.Escape)) {     //      <-- Listen for escape key--do pause and free mouse if selected
#endif
            FireEscapeBtnPressed();
        }
    }

    private void ConfigurePlayerCanvas() {
        var numCanvasChecks = 0;

    CheckForCanvasObj:
        if (Canvas) {
            Canvas.gameObject.SetActive(false);     //      <-- Disable pause menu canvas by default

            //
            // TODO change to keep the canvas, but instead hide the pause menu
            //

        } else if (numCanvasChecks == 2) {
            Log.w("Skipping player canvas rendering procedure");
        } else {
            Log.w("Unable to find player canvas. Checking again...");
            Canvas = FindCanvasInScene();
            numCanvasChecks++;
            goto CheckForCanvasObj;
        }
    }

    private void FireEscapeBtnPressed() {
        Time.timeScale = IsPaused ? 1.0f : 0.0f;
        Canvas.gameObject.SetActive(!IsPaused);
        Cursor.lockState = IsPaused ? CursorLockMode.Confined : CursorLockMode.None; //CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = IsPaused;
        RequestAudioSourceChangeState();
        IsPaused = !IsPaused;
    }

    private void RequestAudioSourceChangeState() {  //      <-- play/pause game music
        var audioSource = Cam.GetComponent<AudioSource>();
        if (audioSource) {
            if (IsPaused) {
                audioSource.Play();
            } else {
                audioSource.Pause();
            }
        } else {
            Log.w("Unable to play/pause music. Skipping procedure");
        }
    }

    private GameObject FindCanvasInScene() {
        var canvas = GameObject.FindWithTag("Canvas");    // TODO use another method for better efficiency
        if (canvas) {
            return canvas;
        }

        return null;
    }

    private Camera FindCameraInScene() {
        var mainCam = GetComponent<Camera>();
        if (mainCam) {
            return mainCam;
        }

        mainCam = GameObject.Find("FPSCamera").GetComponent<Camera>();
        if (mainCam) {
            return mainCam;
        }

        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        if (mainCam) {
            return mainCam;
        }

        Log.w("Unable to locate camera in the current scene. Skipping pause menu procedure.");
        return null;
    }
}


