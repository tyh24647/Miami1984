using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom configurations for the player camera
/// 
/// Author:     Tyler Hostager
/// Version:    05/10/17
/// </summary>
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(GameObject))]
public class TyCamConfiguration : MonoBehaviour {
    const CursorLockMode DefaultLockMode = CursorLockMode.Confined;

    CursorLockMode currentLockMode;
    public CursorLockMode CurrentLockMode {
        get { return currentLockMode; }
        set { currentLockMode = value; }
    }

    public Camera PlayerCamera;
    Camera playerCamera {
        get { return PlayerCamera ?? gameObject.GetComponent<Camera>(); }
    }

    public bool LockToCenter;

    bool isStart = true;

    // Use this for initialization
    void Start() {
        PlayerCamera = playerCamera;
        CurrentLockMode = TyCamConfiguration.DefaultLockMode;
        ToggleCameraLockMode();
        isStart = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.M) && playerCamera.isActiveAndEnabled) {
            Debug.Log("Event triggered: \'KeyCode.M\' pressed");
            ToggleCameraLockMode();
        }
    }

    void ToggleCameraLockMode() {
        var original = currentLockMode;
        if (!isStart) { Debug.Log("Changing \'CursorLockMode\'..."); }
        var changed = currentLockMode = LockToCenter ? CursorLockMode.Locked : CursorLockMode.Confined;
        LockToCenter = !LockToCenter;
        if (!isStart) { Debug.Log("Camera cursor lock mode changed: \'" + original + "\' --> \'" + changed + "\'"); }
    }
}
