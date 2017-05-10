using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This class is a music and background audo player to be attached to
/// specific <code>GameObject</code> instances. 
/// 
/// <code>TyMusicPlayer</code> has the following capabilities:
/// - Assign songs to the player
/// - Repeat the desired song
/// - Reveal a GUI on the active camera, providing skip, pause/play, and mute 
/// functionality
/// - Allows for certain songs to be played at specific times
/// - Certain songs may be played at specific locations
/// 
/// Author:     Tyler Hostager
/// Version:    05/10/17
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(AudioClip))]
[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(GameObject))]
[AddComponentMenu("Audio/TyMusicPlayer")]
public class TyMusicPlayer : MonoBehaviour {

    #region Private Vars
    bool hasRoot;
    bool hasAudioSource;
    bool hasAudioListener;
    bool hasPlayerCamera;
    bool hasSongs;
    bool hasBackgroundAudio;

    GameObject rootObject {
        get { return RootObject ?? gameObject; }
        set { RootObject = value as GameObject ?? RootObject ?? gameObject; }
    }

    Camera playerCamera {
        get { return PlayerCamera ?? gameObject == null ? new Camera() : gameObject.GetComponent<Camera>(); }
        set { PlayerCamera = value as Camera ?? PlayerCamera ?? gameObject.GetComponent<Camera>(); }
    }

    #endregion

    #region Public Vars (Editable in Editor GUI)
    [Header("Audio Object Settings")]
    public AudioSource AudioSource;
    public AudioListener AudioListener;

    [Header("Game Settings")]
    public GameObject RootObject;
    public Camera PlayerCamera;

    [Header("Audio Files")]
    public AudioClip[] Songs;
    public AudioClip[] BackgroundAudio;
    #endregion

    void Awake() {
        Log.d("Initializing \'TyMusicPlayer\'...");
        Log.d("Attempting to locate root object...");

        if (rootObject == null) {
            Log.w("Null root instance -- please specify a valid \'GameObject\' as a root for \'TyMusicPlayer\'");
            hasRoot = false;

            Log.d("Auto-detecting root...");
            if (rootObject != null) {
                Log.d("Root object detected\nLoading into script...");

                RootObject = rootObject;
                Log.d("object loaded successfully");
            }
        }

        /*
         * TODO: camera stuffs
         */

        hasRoot = true;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    /*
     * 
     *          TODO!!!!!!!!!!!!!!
     * 
     *          FINISH MEEEEEE
     * 
     */
}
