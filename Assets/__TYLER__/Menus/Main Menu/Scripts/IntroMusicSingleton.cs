using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Singleton audio class instance to retain the song throughout 
/// the different opening scenes
/// 
/// Author: Tyler Hostager
/// Version: 5/16/17
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioClip))]
public class IntroMusicSingleton : MonoBehaviour {

    private static IntroMusicSingleton InMusic = null;
    public static IntroMusicSingleton Instance {
        get { return InMusic; }
    }

    public AudioClip IntroSong;

    static bool isFirstRun = true;

    public AudioSource AudioSource;

    AudioSource _AudioSource {
        get { return AudioSource ?? gameObject.GetComponent<AudioSource>() ?? new AudioSource(); }
        set { AudioSource = value as AudioSource ?? AudioSource ?? gameObject.GetComponent<AudioSource>() ?? new AudioSource(); }
    }



    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            if (isFirstRun) {
                if (Instance != null && IntroSong) {
                    isFirstRun = false;
                    _AudioSource = Instance.AudioSource;
                } else {
                    if (Instance || !IntroSong) {
                        isFirstRun = false;
                    }

                    if (isFirstRun) {
                        _AudioSource.clip = IntroSong;
                    }

                    if (IntroSong) {
                        DontDestroyOnLoad(this.gameObject);
                    }
                }
            }
        }
    }

    private void Start() {
        if (IntroSong) {
            _AudioSource.clip = IntroSong;
            StartAudio();
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Minus)) {   // not used by other things
            StopAudio();
        }

        if (!Application.isPlaying) {
            _AudioSource.Stop();
        }
    }

    public void StopAudio() {
        if (_AudioSource.isPlaying) {
            _AudioSource.Stop();
        }
    }

    public void StartAudio() {
        if (!_AudioSource.isPlaying && _AudioSource.clip) {
            _AudioSource.Play();
        }
    }

    public void StartAudio(string pathName) {
        // TODO
    }
}
