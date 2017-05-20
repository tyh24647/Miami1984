using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Main menu renderer that applies specific settings that pertain to the main
/// menu object that are specific to the main menu/differ from other menus
/// 
/// Author: Tyler Hostager
/// Version: 5/19/17
/// </summary>
public class MainMenuRenderer : MonoBehaviour {

    public Canvas MainMenuCanvas;
    private Canvas _MainMenuCanvas {
        get {
            return MainMenuCanvas ?? new Canvas();
        }
        set {
            MainMenuCanvas = value ?? MainMenuCanvas ?? new Canvas();
        }
    }

    [Space(1)]
#if DEBUG
    [Header("Auto-Generated Debug Information")]
#endif
    public Button[] Buttons = null;

    public readonly int SceneToLoad;

    #region INHERITED METHODS

    // OnGui is called for rendering and handling GUI events
    private void OnGUI() {
        InitSceneQuickExplorer();
    }

    private void Awake() {
        PopulateUIButtonArray();

    }

    // Use this for initialization
    void Start() {
        if (!MainMenuCanvas) {
            AutoDetectCanvas();
        }

        PopulateUIButtonArray();

        if (_MainMenuCanvas) {
            //InitSceneQuickExplorer();

            //var textItems = _MainMenuCanvas.GetComponentsInChildren<Text>();
            //if (textItems != null && textItems.Length > 0) {
            //    int index = 0;
            //    foreach (var item in textItems) {
            //        if (item.text.ToLower().Contains("debug")) {
            //            var debugItem = item;
            //
            //            if (SavedApplicationState.Debug) {
            //                //item.text += "Enabled";
            //                gameObject.GetComponentsInChildren<Text>()[index].text = "Enabled";
            //            } else {
            //                //item.text += "Disabled";
            //                gameObject.GetComponentsInChildren<Text>()[index].text = "Disabled";
            //            }
            //
            //            index++;
            //        }
            //    }
            //}

            //var debugText = GameObject.FindWithTag("DebugTxt");
            //if (debugText) {
            //    var txtObjs = _MainMenuCanvas.GetComponents<Text>();
            //    foreach (var txtObj in txtObjs) {
            //        //if (txtObj.text.ToLower().Contains("debug")) {
            //        if (txtObj.tag.ToLower().Contains("debug")) {
            //            txtObj.text += "ENABLED";
            //        }
            //    }
            //}
        }
    }

    // Update is called once per frame
    void Update() {
        //PopulateUIButtonArray();
    }
    #endregion

    #region FUNCTIONS
    private void AutoDetectCanvas() {
        Log.w("No \'Canvas\' attribute found. Attempting to auto-detect canvas...");
        try {
            var parentCanvas = FindObjectOfType(typeof(Canvas));
            if (parentCanvas) {
                Log.d("Canvas successfully identified");
                MainMenuCanvas = parentCanvas as Canvas;
                Log.d("Canvas initialized successfully");
            }
        } catch (UnassignedReferenceException e) {
            Log.w("Exception thrown in canvas configuration");
            Log.d("Please add a canvas object manually in the inspector and re");
            Log.d("Exception details: " + e);
        }
    }

    /// <summary>
    /// Allows for quick switching between scenes
    /// </summary>
    private void InitSceneQuickExplorer() {
        GUI.Label(
            new Rect(
                ((Screen.width / 2) - 50),
                (Screen.height - 80),
                100,
                30
            ),
            //("Current Scene: " + SceneManager. + 1)
            ("Current Scene: " + Application.loadedLevel + 1)
        );

        if (GUI.Button(
            new Rect(
                ((Screen.width / 2) - 50),
                (Screen.height - 50),
                100,
                40), "Load Scene: " + (SceneToLoad + 1))) {
            SceneManager.LoadScene(SceneToLoad + 1);
        }
    }

    private void PopulateUIButtonArray() {
        if (_MainMenuCanvas) {
            var mainMenuUIButtons = _MainMenuCanvas.GetComponentsInChildren<Button>();// ?? new List<Button>();
            Buttons = new Button[mainMenuUIButtons.Length];

            for (var i = 0; i < mainMenuUIButtons.Length; i++) {
                Buttons[i] = mainMenuUIButtons[i];
            }

            // disables options such as 'continue game' when not applicable
            ConfigureContinueButton();
        }

    }

    private void ConfigureContinueButton() {
        if (_MainMenuCanvas && Buttons.Length > 0) {
            var shouldEnable = SavedApplicationState.HasSavedGame;
            foreach (var button in Buttons) {
                var childTextAttr = button.GetComponentInChildren<Text>();
                if (childTextAttr && childTextAttr.text.ToLower().Contains("continue")) {
                    button.interactable = shouldEnable;
                    button.GetComponentInChildren<Text>().color = shouldEnable ? Color.white : Color.grey;
                }
            }
        }
    }

    private void EnableContinueButton(bool shouldEnable) {

    }

    #endregion
}
