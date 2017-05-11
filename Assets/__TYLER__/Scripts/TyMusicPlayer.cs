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

    #region Constants
    const ulong DefaultSongPreBufferTime = 44100;
    #endregion

    #region Boolean options (not visible to users)
    bool hasRoot;
    bool hasAudioSource;
    bool hasAudioListener;
    bool hasPlayerCamera;
    bool hasSongs;
    bool hasBackgroundAudio;
    bool canPlayMusic;
    #endregion

    public bool RandomizeMusic = true;
    public bool Play;

    public int CurrentSongIndex;
    int currentSongIndex {
        get { return CurrentSongIndex; }
        set {
            var rand = new System.Random();
            CurrentSongIndex = value < 0 ? RandomizeMusic ? rand.Next(0, Songs.Length) : 0 : value;
        }
    }

    #region Properties
    bool CanPlayMusic {
        get { return canPlayMusic; }
        set { canPlayMusic = value; }
    }

    GameObject rootObject {
        get { return RootObject ?? gameObject; }
        set { RootObject = value as GameObject ?? RootObject ?? gameObject; }
    }

    AudioSource audioSource {
        get { return AudioSource ?? gameObject.GetComponent<AudioSource>() ?? new AudioSource(); }
        set { AudioSource = value as AudioSource ?? AudioSource ?? gameObject.GetComponent<AudioSource>() ?? new AudioSource(); }
    }

    AudioListener audioListener {
        get { return AudioListener ?? gameObject.GetComponent<AudioListener>() ?? new AudioListener(); }
        set { AudioListener = value as AudioListener ?? AudioListener ?? gameObject.GetComponent<AudioListener>() ?? new AudioListener(); }
    }

    Camera playerCamera {
        get { return PlayerCamera ?? gameObject == null ? new Camera() : gameObject.GetComponent<Camera>(); }
        set { PlayerCamera = value as Camera ?? PlayerCamera ?? gameObject.GetComponent<Camera>(); }
    }
    #endregion


    #region Public Vars (Editable in Editor GUI)
    [Space(2)]
    [Header("Audio Object Settings")]
    public AudioSource AudioSource;
    public AudioListener AudioListener;

    [Space(2)]
    [Header("Game Settings")]
    public GameObject RootObject;
    public Camera PlayerCamera;

    [Space(2)]
    [Header("Audio Files")]
    public AudioClip[] Songs;
    public AudioClip[] BackgroundAudio;
    #endregion


    #region Methods
    void Awake() {
        Log.d("Initializing \'TyMusicPlayer\'...");
        Log.d("Attempting to locate root object...");
        this.canPlayMusic = false;
        this.hasRoot = VerifyRootObject();
        this.hasAudioSource = VerifyAudioSource();
        this.hasAudioListener = VerifyAudioListener();
        this.hasPlayerCamera = PlayerCamera;
        this.hasSongs = Songs.Length > 0;
        this.hasBackgroundAudio = BackgroundAudio.Length > 0;

        if (this.hasRoot) {
            Log.d("Root object found");
            Debug.ClearDeveloperConsole();
        } else {
            Log.w("Unable to locate root object");
        }

        canPlayMusic = (
            hasRoot && hasAudioSource && hasAudioListener
            && hasPlayerCamera && hasSongs && hasBackgroundAudio
        );

        audioSource.volume = 1.0f;
    }

    // Use this for initialization
    void Start() {
        var numSongs = Songs.Length;
        if (canPlayMusic) {
            StartMusic();
        }
    }

    // Update is called once per frame
    void Update() {
        if (Play) {
            if (Songs.Length > 0) {
                if (!audioSource) {
                    audioSource = AudioSource;
                }

                if (!audioSource.isPlaying) {
                    var newSong = Songs[this.currentSongIndex];
                    if (newSong != null && newSong.loadState == AudioDataLoadState.Loaded) {
                        PlaySong(newSong ?? Songs[0]);
                    }
                }
            }
        } else {
            AudioSource.Stop();
        }
    }

    void StartMusic() {
        if (Play) {
            var song = Songs[this.currentSongIndex];
            if (!audioSource.isPlaying && song != null) {
                PlaySong(song);
            } else {
                // if no songs are playing and the song 
                // isn't found, load default song at index 0
                PlaySong(Songs[0]);
            }
        } else {
            AudioSource.Stop();
        }
    }

    void PlaySong(AudioClip song) {
        if (Play) {
            if (!song) {
                PlaySong(Songs[0]);     // if null, recurse with the default song at index zero
            }

            if (song.loadState == AudioDataLoadState.Loaded) {
                if (audioSource.isActiveAndEnabled) {
                    AudioSource.clip = song;
                    if (audioSource.clip != null) {
                        audioSource.Play(DefaultSongPreBufferTime);
                    }
                } else {
                    audioSource.enabled = true;
                    PlaySong(song);     // recurse after enabling the audiosource so the song can play
                }
            }
        } else {
            audioSource.Stop();
        }
    }

    void ChangeSong() {
        if (Play) {
            var song = Songs[RandomizeMusic ? this.currentSongIndex : CurrentSongIndex++];
            PlaySong(song ?? Songs[0]);
        } else {
            audioSource.Stop();
        }
    }

    void ChangeSong(bool isNext) {
        if (Play) {
            if (!isNext && CurrentSongIndex <= 0) {
                CurrentSongIndex = Songs.Length - 1;
            } else if (isNext && CurrentSongIndex + 1 >= Songs.Length - 1) {
                CurrentSongIndex = 0;
            }

            PlaySong(
                Songs[isNext ? CurrentSongIndex + 1 : CurrentSongIndex - 1]
            );
        } else {
            audioSource.Stop();
        }
    }

    void ChangeSong(int songIndex) {
        if (Play) {
            PlaySong(Songs[songIndex >= 0 && songIndex < Songs.Length ? songIndex : 0]);
        } else {
            audioSource.Stop();
        }
    }

    void ChangeSong(string songTitle) {
        if (Play) {
            int index = 0;
            foreach (var song in Songs) {
                if (song.name.Equals(songTitle)) {
                    PlaySong(Songs[index] ?? Songs[0]);
                }

                index++;
            }
        } else {
            audioSource.Stop();
        }
    }

    bool VerifyRootObject() {
        var isValid = false;
        if (rootObject == null) {
            Log.w("Null root instance -- please specify a valid \'GameObject\' as a root for \'TyMusicPlayer\'");
            this.hasRoot = false;
            Log.d("Auto-detecting root...");
            if (rootObject != null) {
                RootObject = this.rootObject;
                isValid = true;
                Log.d("Root object loaded successfully");
            }
        }

        return isValid;
    }

    bool VerifyAudioSource() {
        var isValid = false;
        if (audioSource == null) {
            Log.w("Null audiosource instance - please specify a valid \'AudioSource\' object");
            this.hasAudioSource = false;
            Log.d("Auto-detecting audio source...");
            if (audioSource != null) {
                AudioSource = this.audioSource;
                isValid = true;
                Log.d("Audio source loaded successfully");
            }
        }

        return isValid;
    }

    bool VerifyAudioListener() {
        var isValid = false;
        if (audioListener == null) {
            Log.w("Null audio listener - please specify an \'AudioListener\' object");
            this.hasAudioListener = false;
            Log.d("Searching for an audio listener...");
            if (audioListener != null) {
                AudioListener = this.audioListener;
                isValid = true;
                Log.d("\'AudioListener\' loaded successfully.");
            }
        }

        return isValid;
    }

    #endregion
}
