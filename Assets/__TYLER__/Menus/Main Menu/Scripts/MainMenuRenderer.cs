using System.Collections.Generic;
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
    private Canvas MainMenuCanvas = null;
    private GameObject MainMenuPanel = null;
    public Button[] Buttons = null;

    public readonly int SceneToLoad;

    #region INHERITED METHODS
    private void OnGUI() {
        InitSceneQuickExplorer();
    }

    // Use this for initialization
    void Start() {
        AutoDetectAttributes();
        ConfigureContinueButton();
    }

    // Update is called once per frame
    void Update() {
        // do nothing for now
    }
    #endregion

    #region FUNCTIONS
    private void AutoDetectAttributes() {
        this.MainMenuCanvas = AutoDetectCanvas();
        this.MainMenuPanel = AutoDetectMainMenuPanel();
        this.Buttons = AutoDetectButtons();
    }

    private GameObject AutoDetectMainMenuPanel() {
        GameObject menuPanel = null;

        if (MainMenuPanel == null && MainMenuCanvas) {
            Log.w("No Main Menu object found. Attempting to auto-detect menu...");

            try {
                menuPanel = GameObject.Find("MainMenuPanel");
                if (menuPanel == null) {
                    Log.w("Main menu object not assigned. Please assign in the inspector panel and re-run");
                }
            } catch (UnassignedReferenceException e) {
                Log.w("Exception thrown in main menu configuration");
                Log.d("Exception details: " + e);
            }
        }

        return menuPanel;
    }

    private Button[] AutoDetectButtons() {
        Button[] buttons = null;

        if (ObjUtils.IsNullOrEmpty(Buttons) && MainMenuPanel) {
            Log.w("No button objects found. Attempting to auto-detect buttons...");

            try {
                buttons = MainMenuPanel.GetComponents<Button>();
                if (ObjUtils.IsNullOrEmpty(buttons)) {
                    buttons = MainMenuPanel.GetComponentsInChildren<Button>();
                    if (ObjUtils.IsNullOrEmpty(buttons)) {
                        buttons = MainMenuPanel.GetComponentsInParent<Button>();
                        if (ObjUtils.IsNullOrEmpty(buttons)) {
                            throw new UnassignedReferenceException("Button objects not assigned. Please assign them in the inspector panel and re-run");
                        }
                    }
                }

            } catch (UnassignedReferenceException e) {
                Log.w("Exception thrown in buttons configuration");
                Log.d("Exception details: " + e);
            }
        }

        return buttons;
    }

    private Canvas AutoDetectCanvas() {
        Canvas canvas = null;

        if (MainMenuCanvas == null) {
            Log.w("No \'Canvas\' attribute found. Attempting to auto-detect canvas...");

            try {
                canvas = FindObjectOfType(typeof(Canvas)) as Canvas;
                if (canvas) {
                    Log.d("Canvas successfully identified");
                } else {
                    canvas = GetComponent<Canvas>();
                    if (canvas == null) {
                        throw new UnassignedReferenceException("Canvas object value not assigned. Please assign in the inspector panel and re-run");
                    }
                }
            } catch (UnassignedReferenceException e) {
                Log.w("Exception thrown in canvas configuration");
                Log.d("Exception details: " + e);
            }
        }

        return canvas;
    }

    private void InitSceneQuickExplorer() {

        GUI.Label(
            position: new Rect {
                x = ((Screen.width / 2) - 50),
                y = (Screen.height - 80),
                width = 100,
                height = 30
            },

            text: "Current Scene: "
#pragma warning disable CS0618 // Type or member is obsolete
            + Application.loadedLevel
#pragma warning restore CS0618 // Type or member is obsolete
        );

        if (GUI.Button(
            position: new Rect {
                x = ((Screen.width / 2) - 50),
                y = (Screen.height - 50),
                width = 100,
                height = 40
            },

            text: "Load Scene: " + (SceneToLoad + 1))) {
            SceneManager.LoadScene(SceneToLoad + 1);
        }
    }


    private void ConfigureContinueButton() {
        if (MainMenuCanvas != null && Buttons != null && Buttons.Length > 0) {
            var shouldEnable = SavedApplicationState.HasSavedGame;
            foreach (var button in Buttons) {
                var childTextAttr = button.GetComponentInChildren<Text>();
                if (childTextAttr && childTextAttr.text.ToLower().Contains("continue")) {
                    button.interactable = shouldEnable;

                    if (button.GetComponentInChildren<Text>() != null) {
                        button.GetComponentInChildren<Text>().color = shouldEnable ? Color.white : Color.grey;
                    }
                }
            }
        }
    }

    #endregion
}
