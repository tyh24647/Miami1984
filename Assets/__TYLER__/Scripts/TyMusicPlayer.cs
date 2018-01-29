using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
[RequireComponent(typeof(AudioClip))]
[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(GameObject))]
[AddComponentMenu("Audio/TyMusicPlayer")]
public class TyMusicPlayer : MonoBehaviour {

    #region CONSTANTS
    private const ulong DefaultSongPreBufferTime = 44100;
    private const int DefaultNumNullChecks = 5;
    private const int DefaultAsyncTaskMinutes = 2;
    #endregion

    #region PRIVATE VARS
    private bool hasRoot;
    private bool hasAudioSource;
    private bool hasAudioListener;
    private bool hasPlayerCamera;
    private bool hasSongs;
    private bool hasBackgroundAudio;
    private bool canPlayMusic;
    private bool hasWarnings = false;
    private bool hasErrors = false;
    #endregion

    #region EDITOR-MODIFIABLE PRIMITIVES
    public bool RandomizeMusic = true;
    public bool Play;
    public int CurrentSongIndex;
    #endregion

    int currentSongIndex {
        get { return CurrentSongIndex; }
        set {
            var rand = new System.Random();
            CurrentSongIndex = value < 0 ? RandomizeMusic ? rand.Next(0, Songs.Length) : 0 : value;
        }
    }

    #region PROPERTY ACCESSORS
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


    #region PUBLIC VARS
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
    public AudioClip[] BackgroundAudioEffectsClips;
    #endregion


    #region INHERITED METHODS
    void Awake() {
        Log.d("Initializing \'TyMusicPlayer\'...");
        Log.d("Attempting to locate root object...");
        this.canPlayMusic = false;
        this.hasRoot = VerifyRootObject();
        this.hasAudioSource = VerifyAudioSource();
        this.hasAudioListener = VerifyAudioListener();
        this.hasPlayerCamera = PlayerCamera;
        this.hasSongs = Songs.Length > 0;
        this.hasBackgroundAudio = BackgroundAudioEffectsClips.Length > 0;

        if (this.hasRoot) {
            Log.d("Root object found");
            Debug.ClearDeveloperConsole();
        } else {
            Log.w("Unable to locate root object");
            this.hasWarnings = true;
        }

        canPlayMusic = (
            hasRoot && hasAudioSource && hasAudioListener
            && hasPlayerCamera && hasSongs && hasBackgroundAudio
        );

        audioSource.volume = 1.0f;
    }

    private void OnDestroy() {
        StopCurrentSong();

        if (!this.hasWarnings && !this.hasErrors) {
            Debug.ClearDeveloperConsole();
        }
    }

    // Use this for initialization
    void Start() {
        var numSongs = Songs.Length;
        if (numSongs == 0) {
            Log.w("No songs or audio clips loaded");
        }

        if (GetComponent<GameObject>()) {
            this.rootObject = GetComponent<GameObject>();
            this.hasRoot = true;
            this.VerifyRootObject();
        }

        if (canPlayMusic) {
            StartMusic();
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.anyKeyDown) {
            HandleKeyPressed(Play);
        }

        if (Play) {
            if (Songs.Length > 0) {
                if (!audioSource) {
                    audioSource = AudioSource;
                } else if (!audioSource.isPlaying) {
                    var newSong = Songs[CurrentSongIndex];
                    if (newSong != null && newSong.loadState == AudioDataLoadState.Loaded) {
                        PlaySong(newSong ?? Songs[0]);
                    }
                }
            } else {
                Log.w("No songs found in the songs list. Skipping procedure");
            }
        } else {
            AudioSource.Stop();
        }
    }
    #endregion

    #region FUNCTIONS
    void HandleKeyPressed(bool playMusic) {
        bool playNext = false;
        bool playPrevious = false;
        bool isImportantKey = false;

        if (Input.GetKeyDown(KeyCode.F9)) {
            playNext = true;
            playPrevious = false;
            isImportantKey = true;
        } else if (Input.GetKeyDown(KeyCode.F7)) {
            playNext = false;
            playPrevious = true;
            isImportantKey = true;
        } else if (Input.GetKeyDown(KeyCode.F10)) {
            playMusic = !playMusic;
            playNext = false;
            playPrevious = false;
            isImportantKey = true;

            if (!playMusic) {
                this.audioSource.Stop();
            } else {
                this.audioSource.Play();
            }
        }

        if (isImportantKey) {
            ChangeSong(playNext && !playPrevious);
        }
    }


    /*
    void HandleNoMusicInScene() {

        AudioClip song = this.audioSource.clip;
        bool firstCheck = true;
    NoSongs:
        if (!song) {
            song = Songs[0] ?? null;
            AsyncOperation waitForUserSongAdditions = new AsyncOperation(() => Log.d(""));
            if (!song && firstCheck) {
                firstCheck = false;
                goto NoSongs;
            }

            audioSource.Stop();
        }
    }
    */

    // Initialize audio clip
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

    /// <summary>
    /// Plays the specified audio clip, as long as it is in the valid .wav
    /// format.
    /// </summary>
    /// <param name="song">The song in which to play</param>
    void PlaySong(AudioClip song) {
        if (Play) {
            if (!song) {
                PlaySong(Songs[0]);     // if null, recurse with the default song at index zero
            } else if (song.loadState == AudioDataLoadState.Loaded) {
                if (audioSource.isActiveAndEnabled) {
                    AudioSource.clip = song;
                    if (audioSource.clip != null) {
                        Log.d("Now playing: \'" + audioSource.clip.name + "\'.");
                        audioSource.Play(DefaultSongPreBufferTime);
                        //audioSource.PlayDelayed(DefaultSongPreBufferTime);
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

    void StopCurrentSong() {
        if (this.audioSource) {
            this.audioSource.Stop();
            this.audioSource.clip = null;
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

    /// <summary>
    /// Changes the song by either going forward or backwards in the queue,
    /// depending on the boolean value of the input parameter
    /// </summary>
    /// <param name="isNext">If set to <c>true</c> is next.</param>
    void ChangeSong(bool isNext) {
        if (Play) {
            if (!isNext && CurrentSongIndex <= 0) {
                CurrentSongIndex = Songs.Length - 1;
            } else if (isNext && CurrentSongIndex + 1 >= Songs.Length - 1) {
                CurrentSongIndex = 0;
            }

            if (Songs != null && Songs.Length > 0) {
                var songIndex = isNext ? CurrentSongIndex++ : CurrentSongIndex--;
                PlaySong(Songs[songIndex]);
            } else {
                Log.d("Unable to skip song--no songs loaded into the scene");
            }
        } else {
            audioSource.Stop();
        }
    }

    /// <summary>
    /// Changes the song based on the known index of a specified song
    /// within the queue, as long as the index is valid, and the sound clip
    /// at that index is not null.
    /// </summary>
    /// <param name="songIndex">The index of the desired clip.</param>
    void ChangeSong(int songIndex) {
        if (Play) {
            PlaySong(Songs[songIndex >= 0 && songIndex < Songs.Length ? songIndex : 0]);
        } else {
            audioSource.Stop();
        }
    }

    /// <summary>
    /// Changes the song based on the title of a specified song within
    /// the queue, as long as there is a song that matches this name
    /// </summary>
    /// <param name="songTitle">The title of the desired audio clip.</param>
    void ChangeSong(string songTitle) {
        if (Play) {
            int index = 0;
            foreach (var song in Songs) {
                if (song.name.ToUpper().Equals(songTitle.ToUpper())) {
                    PlaySong(Songs[index] ?? Songs[0]);
                } else {
                    index++;
                }

                if (index == Songs.Length) { break; }
            }

            return;
        } else {
            audioSource.Stop();
        }
    }

    bool VerifyRootObject() {
        var isValid = false;
        if (!rootObject) {
            Log.w("Null root instance -- please specify a valid \'GameObject\' as a root for \'TyMusicPlayer\'");
            this.hasRoot = false;
            Log.d("Auto-detecting root...");
            if (rootObject != null) {
                RootObject = this.rootObject;
                isValid = true;
                Log.d("Root object loaded successfully");
            } else {
                this.hasWarnings = true;
                Log.w("Unable to locate root object");
            }
        } else {
            this.hasRoot = true;
            isValid = true;
            Log.d("Root object loaded successfully");
        }

        return isValid;
    }

    bool VerifyAudioSource() {
        var isValid = false;
        if (!audioSource) {
            Log.w("Null audiosource instance - please specify a valid \'AudioSource\' object");
            this.hasAudioSource = false;
            Log.d("Auto-detecting audio source...");
            if (audioSource) {
                AudioSource = this.audioSource;
                isValid = true;
                Log.d("Audio source loaded successfully");
            } else {
                this.hasWarnings = true;
                Log.w("Unable to locate audio source");
            }
        } else {
            this.hasAudioSource = true;
            isValid = true;
            Log.d("Audio source loaded successfully");
        }

        return isValid;
    }

    bool VerifyAudioListener() {
        var isValid = false;
        if (!audioListener) {
            Log.w("Null audio listener - please specify an \'AudioListener\' object");
            this.hasAudioListener = false;
            Log.d("Searching for an audio listener...");
            if (audioListener != null) {
                AudioListener = this.audioListener;
                isValid = true;
                Log.d("\'AudioListener\' loaded successfully.");
            } else {
                this.hasWarnings = true;
                Log.w("Unable to locate \'AudioListener\'");
            }
        } else {
            this.hasAudioListener = true;
            isValid = true;
            Log.d("\'AudioListener\' loaded successfully");
        }

        return isValid;
    }
    #endregion
}
