using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Data object which contains the user's preferences for the application itself, such
/// as graphics, audio, and other non-gameplay settings. This includes a singleton
/// of the object that will not be destroyed while the game is running once the preferences
/// are loaded, and allows the appropriate classes to modify the save data if necessary.
/// 
/// This class serializes the preferences data into raw binary, then writes it to the
/// appropriate data file hidden on the user's machine.
/// 
/// Author: Tyler Hostager
/// Version: 2/23/17
/// </summary>
public class PLPrefs : MonoBehaviour {

    public static PLPrefs UserPrefs;

    private void Awake() {
        if (UserPrefs == null) {
            DontDestroyOnLoad(gameObject);
            UserPrefs = this;
        } else if (UserPrefs != this) {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnEnable() {
        // TODO Autosave data here
    }

    private void OnDisable() {
        // TODO Autosave data here
    }

    private void OnGUI() {
        // TODO
    }

    public void Save() {

    }

}


/// <summary>
/// Serializable container for the player preferences. This allows for the preferences
/// data to be written to a file after serialization is performed.
/// 
/// Author: Tyler Hostager
/// Version: 5/23/17 
/// </summary>
[Serializable]
class PLPreferences {

}

