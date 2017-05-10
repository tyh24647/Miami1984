using UnityEngine;
using UnityEditor;

/// <summary>
/// Disables the in-game mouse from moving the camera objects while the
/// game is being debugged, allowing mouse clicks outside of the game preview
/// without affecting the game itself.
/// 
/// Author:     Tyler Hostager
/// Version:    05/09/17
/// </summary>
[ExecuteInEditMode]
public class OffscreenMouseDisable : Editor {

    #region Private constants
    private const bool DEFAULT_MOUSE_LOCKED_STATUS = false;
    private const CursorLockMode CURSOR_CONFINED = CursorLockMode.Confined;
    private const CursorLockMode CURSOR_CENTER_LOCKED = CursorLockMode.Locked;
    private const CursorLockMode CURSOR_UNLOCKED = CursorLockMode.None;
    #endregion

    #region Private vars
    private bool _MouseIsFree = DEFAULT_MOUSE_LOCKED_STATUS;
    #endregion

    #region Public vars
    public bool MouseIsFree {
        get {
#if MOBILE_INPUT
            return false;
#endif
            return _MouseIsFree;
        }

        set { _MouseIsFree = value; }
    }
    #endregion


    // Use this for initialization
    void Start() {
        MouseIsFree = false;    // default value
    }

    // Update is called once per frame
    void Update() {
#if UNITY_EDITOR
        var canChangeMouseState = false;

        if (EditorApplication.isPlaying || EditorApplication.isPaused) {
            canChangeMouseState = true;
        }
#endif
#if !MOBILE_INPUT
        if (Input.GetKeyDown(KeyCode.M) && canChangeMouseState) {
            Debug.Log("KeyEvent detected - \"M\"", this);
            Debug.Log("Current mouse state for play mode: \'" + MouseIsFree + "\'", this);
            UnlockMouseFromPlayMode();
        }
    }
#endif

    void UnlockMouseFromPlayMode() {
        Debug.Log("Toggling cursor state...", this);
        Cursor.lockState = MouseIsFree ? CURSOR_CONFINED : CURSOR_UNLOCKED;

        Debug.Log("Cursor mode changed to \'" + (Cursor.lockState.Equals(CURSOR_CONFINED) ?
                                           "Confined" : "Unlocked") + "\'", this);
        MouseIsFree = !MouseIsFree;
        Debug.Log("Changed mouse state to \'" + MouseIsFree + "\'", this);
    }
}
