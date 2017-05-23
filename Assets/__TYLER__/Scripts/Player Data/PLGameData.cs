using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Game Data object which contains the appropriate static attributes related to the
/// current player. This will be serialized in the container class below, and will
/// be written to and read from a binary data file located in the application data
/// folder of the user.
/// 
/// Author: Tyler Hostager
/// Version: 5/23/17
/// </summary>
public class PLGameData : MonoBehaviour {

    #region SINGLETON INSTANCE
    public static PLGameData GameData;
    #endregion


    #region PRIVATE ACCESSORS
    private const FileMode DefaultFileMode = FileMode.Open;
    private string GameDataPath;
    private bool IsDebugging { get; set; }
    private int _CurrentSceneIndex { get; set; }
    #endregion


    #region EDITOR ATTRIBUTES
    public float Health;
    public float Stamina;
    public bool ShouldSaveOnExit = true;
    public bool HasDebugPermissions = true;
    public bool FirstPersonMode = true;
    #endregion


    #region CALCULATED ATTRIBUTES
    public bool HasNearbyEnemies { get; set; }
    public int NearbyEnemyCount { get; set; }
    public int CurrentLevel { get; set; }
    public int CurrentSceneIndex { get { return SceneManager.GetActiveScene().buildIndex; } private set { _CurrentSceneIndex = value; } }
    public int SelectedWeaponIndex { get; set; }
    public int NumWeaponsInInventory { get; set; }
    public string CurrentPlaylistName { get; set; }
    public string LastSongPlayedName { get; set; }
    public string PlayerName { get; set; }
    public float PosX { get; set; } // TODO: make a serializable subclass of GameData to store the player transform values 
    public float PosY { get; set; }
    public float PosZ { get; set; }
    public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }
    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
    public float ScaleZ { get; set; }
    public float MasterVolume { get; set; }
    public float MusicVolume { get; set; }
    public float SFXVolume { get; set; }
    public float ProgressPercentage { get; private set; }
    #endregion


    #region INHERITED METHODS
    private void Awake() {
        GameDataPath = Application.persistentDataPath + "/GameData.dat";

        if (GameData == null) {
            DontDestroyOnLoad(gameObject);
            GameData = this;
        } else if (GameData != this) {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start() {
        this.CurrentLevel = GetCurrentLevel();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnEnable() {
        // TODO Consider Autosaving data here
    }

    private void OnDisable() {
        // save values on exit if enabled
        if (ShouldSaveOnExit) {
            Save();
        }
    }

    private void OnGUI() {
        GUI.Label(new Rect(10, 10, 150, 30), "Health: " + Health);
        GUI.Label(new Rect(10, 40, 150, 30), "Stamina: " + Stamina);
    }
    #endregion


    #region PUBLIC FUNCTIONS
    public void Save() {

        // instantiate binary formatter for writing serialized binary objects to files
        var bf = new BinaryFormatter();

        // create or open file
        var file = CreateFileAtPath(GameDataPath, FileMode.OpenOrCreate);
        if (file != null) {
            Log.d("Game data file successfully created");
        }

        // serialize the data to the player data class
        var playerData = SaveDataToSerializableObject();

        // take serializable class data and write it to a binary file
        bf.Serialize(file, playerData);

        // close the output file
        file.Close();

    }

    public void Load() {
        if (File.Exists(GameDataPath)) {
            var bf = new BinaryFormatter();
            var file = OpenFileAtPath(GameDataPath);

            // deserialize binary file
            var playerData = (GameData)bf.Deserialize(file);
            if (playerData != null) {
                Log.d("Game data loaded successfully");
            }

            // close the file after loading data into the player data object
            file.Close();

            // load the deserialized data
            LoadDataFromSerializableObject(playerData);
        }
    }

    public static int GetCurrentLevel() {
        return 1;

        // TODO implement this
    }
    #endregion


    #region PRIVATE FUNCTIONS
    private GameData SaveDataToSerializableObject() {
        var playerData = new GameData {
            Health = this.Health,
            Stamina = this.Stamina,
            CurrentLevel = this.CurrentLevel,
            PosX = this.PosX,
            PosY = this.PosY,
            PosZ = this.PosZ,
            RotX = this.RotX,
            RotY = this.RotY,
            RotZ = this.RotZ,
            ScaleX = this.ScaleX,
            ScaleY = this.ScaleY,
            ScaleZ = this.ScaleZ,
            ShouldSaveOnExit = this.ShouldSaveOnExit,
            IsDebugging = this.IsDebugging,
            IsThirdPerson = this.FirstPersonMode,
            CurrentSceneIndex = this.CurrentSceneIndex,
            CurrentPlaylistName = this.CurrentPlaylistName,
            LastPlayedSongName = this.LastSongPlayedName,
            NearbyEnemyCount = this.NearbyEnemyCount,
            //HasNearbyEnemies = this.HasNearbyEnemies,




        };

        return playerData;
    }

    private GameData LoadDataFromSerializableObject(GameData playerData) {
        if (playerData != null) {
            this.Health = playerData.Health;
            this.Stamina = playerData.Stamina;
            this.CurrentLevel = playerData.CurrentLevel;
            this.PosX = playerData.PosX;
            this.PosY = playerData.PosY;
            this.PosZ = playerData.PosZ;
            this.RotX = playerData.RotX;
            this.RotY = playerData.RotY;
            this.RotZ = playerData.RotZ;
            this.ScaleX = playerData.ScaleX;
            this.ScaleY = playerData.ScaleY;
            this.ScaleZ = playerData.ScaleZ;
            this.ShouldSaveOnExit = playerData.ShouldSaveOnExit;
            this.IsDebugging = playerData.IsDebugging;
            this.FirstPersonMode = playerData.IsThirdPerson;
            this.CurrentSceneIndex = playerData.CurrentSceneIndex;
            this.CurrentPlaylistName = playerData.CurrentPlaylistName;
            this.LastSongPlayedName = playerData.LastPlayedSongName;
            this.NearbyEnemyCount = playerData.NearbyEnemyCount;
            this.HasNearbyEnemies = playerData.HasNearbyEnemies;


        }

        return playerData;
    }

    private FileStream CreateFileAtPath(string filePath) {
        return CreateFileAtPath(filePath, DefaultFileMode);
    }

    private FileStream CreateFileAtPath(string filePath, FileMode fileMode) {
        FileStream file = null;

        if (!ObjUtils.IsNullOrEmpty(filePath)) {
            Log.d("Creating binary file at path \"" + filePath + "\"...");

            var tmp = File.OpenWrite(filePath);
            if (tmp != null) {
                file = tmp;
            } else {
                Log.w("Unable to create file at the specified location --> \'" + filePath + "\'");
            }
        }

        return file;
    }

    private FileStream OpenFileAtPath(string filePath) {
        return OpenFileAtPath(filePath, DefaultFileMode);
    }

    private FileStream OpenFileAtPath(string filePath, FileMode fileMode) {
        FileStream file = null;

        if (!ObjUtils.IsNullOrEmpty(filePath)) {
            Log.d("Opening deserialized binary file at path \"" + filePath + "\"...");
            var tmp = File.Open(
                path: filePath,
                mode: fileMode
            );

            if (tmp != null) {
                file = tmp;
            } else {
                Log.w("Unable to load file at the specified location --> \'" + filePath + "\'");
            }
        }

        return file;
    }
    #endregion
}


/// <summary>
/// Player Data container class allows the game data to be written to a file after
/// it is serialized and converted into raw binary. It is able to be deserialized
/// when a document is loaded using thie class as long as it has been serialized
/// by this class and not another object.
/// 
/// Author: Tyler Hostager
/// Version: 5/23/17
/// </summary>
[Serializable]
class GameData {

    #region PRIVATE VARS
    private float _Health;
    private float _Stamina;
    private float _PosX;
    private float _PosY;
    private float _PosZ;
    private float _RotX;
    private float _RotY;
    private float _RotZ;
    private float _ScaleX;
    private float _ScaleY;
    private float _ScaleZ;
    private float _MasterVolume;
    private float _MusicVolume;
    private float _SFXVolume;
    private float _ProgressPercentage;

    private bool _ShouldSaveOnExit;
    private bool _IsDebugging;
    private bool _FirstPersonMode;
    private bool _HasNearbyEnemies;

    private int _CurrentLevel;
    private int _CurrentSceneIndex;
    private int _NearbyEnemyCount;
    private int _NumObjectsInInventory;
    private int _NumWeaponsInInventory;
    private int _SelectedWeaponIndex;

    private string _PlayerName;
    private string _CurrentPlaylistName;
    private string _LastPlayedSongName;
    #endregion


    #region BOOLEAN VALUES
    public bool ShouldSaveOnExit { get; set; }
    public bool IsDebugging { get; set; }
    public bool IsThirdPerson { get; set; }
    public bool HasNearbyEnemies { get { return NearbyEnemyCount > 0; } }
    #endregion


    #region FLOAT VALUES
    public float Health {
        get { return _Health; }
        set { _Health = value >= 0 ? value : 0; }
    }

    public float Stamina {
        get { return _Stamina; }
        set { _Stamina = value >= 0 ? value : 0; }
    }

    public float PosX {
        get { return _PosX; }
        set { _PosX = value; }
    }

    public float PosY {
        get { return _PosY; }
        set { _PosY = value; }
    }

    public float PosZ {
        get { return _PosZ; }
        set { _PosZ = value; }
    }

    public float RotX {
        get { return _RotX; }
        set { _RotX = value; }
    }

    public float RotY {
        get { return _RotY; }
        set { _RotY = value; }
    }

    public float RotZ {
        get { return _RotZ; }
        set { _RotZ = value; }
    }

    public float ScaleX {
        get { return _ScaleX; }
        set { _ScaleX = value; }
    }

    public float ScaleY {
        get { return _ScaleY; }
        set { _ScaleY = value; }
    }

    public float ScaleZ {
        get { return _ScaleZ; }
        set { _ScaleZ = value; }
    }

    public float MasterVolume {
        get { return _MasterVolume < 0.0f || _MasterVolume > 1.0f ? 0.85f : _MasterVolume; }
        set { _MasterVolume = value > 0.0f && value < 1.0f ? value : _MasterVolume; }
    }

    public float MusicVolume {  // always stay below master volume level
        get { return _MusicVolume <= _MasterVolume && _MusicVolume > 0 && _MusicVolume < 1.0f ? _MusicVolume : _MasterVolume; }
        set { _MusicVolume = value <= _MasterVolume && value > 0 ? value : MusicVolume; }
    }

    public float SFXVolume {    // always stay below master volume level
        get { return _SFXVolume <= MasterVolume && _SFXVolume > 0 && _SFXVolume < 1.0f ? _SFXVolume : _MasterVolume; }
        set { _SFXVolume = value <= MasterVolume && value > 0 ? value : SFXVolume; }
    }

    public float ProgressPercentage {
        get { return _ProgressPercentage >= 0.0f && _ProgressPercentage <= 1.0f ? _ProgressPercentage : 0.0f; }
        set { _ProgressPercentage = value < 0.0f || value > 1.0f ? ProgressPercentage : value; }
    }
    #endregion


    #region INTEGER VALUES
    public int CurrentLevel {
        get { return _CurrentLevel; }
        set { _CurrentLevel = value > 0 ? value : 1; }
    }

    public int CurrentSceneIndex {
        get { return _CurrentSceneIndex; }
        set { _CurrentSceneIndex = value; }
    }

    public int NearbyEnemyCount {
        get { return _NearbyEnemyCount >= 0 && _NearbyEnemyCount <= 30 ? 5 : _NearbyEnemyCount; }
        set { _NearbyEnemyCount = value < 0 || value > 30 ? NearbyEnemyCount : value; }
    }

    public int NumWeaponsInInventory {
        get { return _NumWeaponsInInventory >= 0 ? _NumWeaponsInInventory : 0; }
    }

    public int SelectedWeaponInded {
        get { return _SelectedWeaponIndex; }
        set { _SelectedWeaponIndex = NumWeaponsInInventory > 0 && value > 0 ? 0 : value; }
    }
    #endregion


    #region STRING VALUES
    public string PlayerName {
        get {
            return ObjUtils.IsNullOrEmpty(_PlayerName) ? "DefaultPlayer" : @_PlayerName;
        }
        set {
            _PlayerName = ObjUtils.IsNullOrEmpty(@value) ? (
                ObjUtils.IsNullOrEmpty(PlayerName) ? "DefaultPlayer" : PlayerName
            ) : @value;
        }
    }

    public string CurrentPlaylistName {
        get {
            return ObjUtils.IsNullOrEmpty(_CurrentPlaylistName) ? "DefaultPlaylist" : @_CurrentPlaylistName;
        }
        set {
            _CurrentPlaylistName = ObjUtils.IsNullOrEmpty(@value) ? (
                ObjUtils.IsNullOrEmpty(CurrentPlaylistName) ? "DefaultPlaylist" : CurrentPlaylistName
            ) : @value;
        }
    }

    public string LastPlayedSongName {
        get {
            return ObjUtils.IsNullOrEmpty(_LastPlayedSongName) ? "Unknown" : @_LastPlayedSongName;
        }
        set {
            _LastPlayedSongName = ObjUtils.IsNullOrEmpty(@value) ? (
                ObjUtils.IsNullOrEmpty(LastPlayedSongName) ? "Unknown" : @_LastPlayedSongName
            ) : @value;
        }
    }
    #endregion
}

