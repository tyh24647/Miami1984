using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedApplicationState : MonoBehaviour {

    #region Static preferences loaded from saved application state
    public static bool HasSavedGame = CheckUserForSavedGames();

#if DEBUG
    public static bool Debug = true;
#else
    public static bool Debug = false;
#endif


    #endregion



    #region Private temporary variables to access user states
    private bool _HasSavedGame = false;
    #endregion


    // Awake is called when the instance is being loaded
    private void Awake() {

    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private static bool CheckUserForSavedGames() {

        // TODO implement this

        return false;
    }
}
