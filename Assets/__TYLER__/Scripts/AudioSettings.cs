using UnityEngine;

/// <summary>
/// Used by the audio settings menu--adjusts in-game volume
/// 
/// Author: Tyler Hostager
/// Version: 5/19/17
/// </summary>
public class AudioSettings : MonoBehaviour {
    private const float DefaultVolume = 1.0f;

    private float _MasterVolume;
    private float _MusicVolume;
    private float _SFXVolume;

    public float MasterVolume {
        get {
            return AudioListener.volume;
        }
        set {
            AudioListener.volume = _MasterVolume = value >= 0.0f && value <= 1.0f ? value : AudioSettings.DefaultVolume;
        }
    }

    public float MusicVolume {
        get {
            var volume = AudioSettings.DefaultVolume;

            if (GameObject.FindWithTag("IntroMusic")) {
                volume = GameObject.FindWithTag("IntroMusic").GetComponent<AudioSource>().volume;
            } else if (GameObject.FindWithTag("GameMusic")) {
                volume = GameObject.FindWithTag("GameMusic").GetComponent<AudioSource>().volume;
            } else if (GameObject.FindWithTag("Music")) {
                volume = GameObject.FindWithTag("Music").GetComponent<AudioSource>().volume;
            } else {
                volume = AudioListener.volume;
            }

            return volume;
        }

        set {
            var audioSource = GetAudioSourceForGameObject("IntroMusic");
            if (!audioSource) {
                audioSource = GetAudioSourceForGameObject("GameMusic");
                if (!audioSource) {
                    audioSource = GetAudioSourceForGameObject("Music");
                    if (!audioSource) {
                        audioSource = null;
                    }
                }
            }

            if (audioSource) {
                _MusicVolume = audioSource.volume = value > 1.0f || value < 0.0f ? AudioSettings.DefaultVolume : value;
            } else {
                _MusicVolume = AudioListener.volume;
            }
        }
    }

    public float SFXVolume {
        get {
            var sfxObj = GetAudioSourceForGameObject("SFX");
            if (sfxObj) {
                return sfxObj.volume;
            }

            return _SFXVolume > AudioListener.volume || _SFXVolume < AudioListener.volume ? _SFXVolume : AudioListener.volume;
        }
        set {
            var sfxObj = GetAudioSourceForGameObject("SFX");
            if (sfxObj) {
                _SFXVolume = sfxObj.volume = value < 0.0f || value > 1.0f ? AudioSettings.DefaultVolume : value;
            } else {
                _SFXVolume = value >= 0.0f && value <= 1.0f ? value : AudioSettings.DefaultVolume;
            }
        }
    }

    private void Awake() {
        this.MasterVolume = AudioSettings.DefaultVolume;
    }

    private void Start() {
        if (this._MasterVolume < AudioListener.volume || _MasterVolume > AudioListener.volume) {
            this.MasterVolume = AudioListener.volume;
        }
    }

    private void Update() {

    }

    public void SetMasterVolume(float volumeLevel) {
        if (volumeLevel <= 1.0f && volumeLevel >= 0.0f) {
            AudioListener.volume = volumeLevel;
        }
    }

    public void SetMasterVolumePercent(int volumeLevelPercent) {
        if (volumeLevelPercent >= 0 && volumeLevelPercent <= 100) {
            SetMasterVolume(volumeLevelPercent / 100);
        }
    }

    public void SetMusicVolume(float volumeLevel) {
        if (volumeLevel <= 1.0f && volumeLevel >= 0.0f) {
            ApplyVolumeToAudioSourceObj(GameObject.FindWithTag("IntroMusic"), volumeLevel);
            ApplyVolumeToAudioSourceObj(GameObject.FindWithTag("GameMusic"), volumeLevel);
            ApplyVolumeToAudioSourceObj(GameObject.FindWithTag("Music"), volumeLevel);
        }
    }

    private AudioSource GetAudioSourceForGameObject(GameObject obj) {
        if (obj && obj.GetComponent<AudioSource>()) {
            return obj.GetComponent<AudioSource>();
        }

        return null;
    }

    private AudioSource GetAudioSourceForGameObject(string tagStr) {
        AudioSource tmpSrc = null;
        if (GameObject.FindWithTag(tagStr)) {
            var tmpObj = GameObject.FindWithTag(tagStr);
            if (tmpObj) {
                if (tmpObj.GetComponent<AudioSource>()) {
                    tmpSrc = tmpObj.GetComponent<AudioSource>();
                } else if (tmpObj.GetComponentInParent<AudioSource>()) {
                    tmpSrc = tmpObj.GetComponentInParent<AudioSource>();
                } else if (tmpObj.GetComponentInChildren<AudioSource>()) {
                    tmpSrc = tmpObj.GetComponentInChildren<AudioSource>();
                } else {
                    tmpSrc = null;
                }
            }
        }

        return tmpSrc;
    }

    private void ApplyVolumeToAudioSourceObj(GameObject musicObj, float volumeLevel) {
        if (musicObj) {
            var audioSources = musicObj.GetComponents<AudioSource>();
            if (!ObjUtils.IsNullOrEmpty(audioSources)) {
                foreach (var audioSource in audioSources) {
                    audioSource.volume = volumeLevel;
                }
            }
        }
    }

    public int MasterVolumePercent() {
        return (int)(AudioListener.volume * 100);
    }

}
