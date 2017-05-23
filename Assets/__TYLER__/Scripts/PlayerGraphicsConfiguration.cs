using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerGraphicsConfiguration : MonoBehaviour {

    #region STATIC VARS
    private static float DefaultShadowDistanceBeforeChanged;
    #endregion

    #region CONSTANTS
    public const float DefaultCollapsedScrollViewHeight = 280f;
    public const float DefaultExpandedScrollViewHeight = 1000f;
    #endregion

    #region PUBLIC VARS
    public float ShadowDistance {
        get { return QualitySettings.shadowDistance; }
        set { QualitySettings.shadowDistance = value < 0 ? DefaultShadowDistanceBeforeChanged : value; }
    }

    [Space(1)]
    [Header("Inputs")]
    public InputField ShadowDistanceField;
    public Button ApplyButton;

    [Space(1)]
    [Header("Show/Hide Advanced Settings Objects:")]
    public bool ShowAdvancedSettings = false;
    public List<GameObject> Buttons;
    public List<GameObject> TextObjects;
    #endregion

    #region PRIVATE VARS
    private List<GameObject> AdvancedSettingsObjects = null;
    private GameObject ShowAdvancedButton;
    private Button AdvancedButton;
    private List<string> SupportedResolutions;
    private Dropdown ResolutionChooser;
    private int SelectedResolutionIndex;
    private Resolution InitialResolution;
    private Resolution SelectedResolution;
    private bool ResolutionChanged;
    private bool IsFirstChange = true;
    #endregion


    #region INHERITED METHODS
    private void Awake() {
        DefaultShadowDistanceBeforeChanged = QualitySettings.shadowDistance;
        ShadowDistance = DefaultShadowDistanceBeforeChanged;

        if (ShadowDistanceField != null) {
            ShadowDistanceField.enabled = false;
            ShadowDistanceField.placeholder.enabled = false;
            ShadowDistanceField.textComponent.text = DefaultShadowDistanceBeforeChanged.ToString();
        }

        this.InitialResolution = Screen.currentResolution;
    }

    // Use this for initialization
    void Start() {
        AutoDetectAdvancedSettingsObjects();
        AutoDetectInputFields();

        foreach (var settingsObj in AdvancedSettingsObjects) {
            settingsObj.SetActive(ShowAdvancedSettings);
        }

        if (ApplyButton != null) {
            ApplyButton.enabled = false;
            if (ApplyButton.GetComponent<Text>()) {
                ApplyButton.GetComponent<Text>().color = Color.gray;
            }

            var evSystem = EventSystem.current;
            evSystem.SetSelectedGameObject(null);
            ApplyButton.OnSelect(new BaseEventData(evSystem));
        } else {
            Log.w("Unable to locate \'ApplyButton\'");
        }

        FindSpecialButtons();
        ConfigureResolutionDropdown();
    }

    // Update is called once per frame
    void Update() {
        // do nothing for now
    }
    #endregion

    #region PRIVATE FUNCTIONS
    private void ConfigureResolutionDropdown() {
        this.ResolutionChanged = false;

        var rList = Screen.resolutions;
        if (rList.Length > 0) {
            this.SupportedResolutions = new List<string>(rList.Length);
            foreach (var res in rList) {
                SupportedResolutions.Add(res.ToString());
            }
        }

        var resChanger = GameObject.Find("ResolutionPanel").GetComponentInChildren<Dropdown>();
        if (resChanger != null) {
            this.ResolutionChooser = resChanger;

            // add supported resolutions to dropdown
            if (!ObjUtils.IsNullOrEmpty(SupportedResolutions)) {
                ResolutionChooser.AddOptions(SupportedResolutions);
                var myRes = Screen.currentResolution;

                // selects current resolution on the dropdown by default
                for (var i = 0; i < ResolutionChooser.options.Count; i++) {
                    if (ResolutionChooser.options[i].text.Contains(myRes.ToString())) {
                        ResolutionChooser.value = i;
                        break;
                    }
                }
            }
        }

        this.SelectedResolutionIndex = ResolutionChooser.value;
    }

    private void FindSpecialButtons() {
        if (!ObjUtils.IsNullOrEmpty(Buttons)) {
            var tmp = GameObject.Find("ShowAdvancedSettingsBtn");
            if (tmp) {
                AdvancedButton = tmp.GetComponent<Button>();
                if (!AdvancedButton) {
                    Log.w("Unable to find \'ShowAdvancedSettingsBtn\' object");
                }
            }
        }
    }

    private void AutoDetectAdvancedSettingsObjects() {
        if (ObjUtils.List.IsNullOrEmpty(Buttons)) {
            var parentButtons = GetComponentsInParent<Button>();
            if (parentButtons != null && parentButtons.Length > 0) {
                var buttonObjs = new List<GameObject>();
                foreach (var button in parentButtons) {
                    if (button.tag.ToLower().Contains("advanced")) {
                        buttonObjs.Add(button.gameObject);  // add the gameobject instead of the button
                    }
                }

                if (buttonObjs != null && buttonObjs.Count > 0) {
                    Buttons = ObjUtils.List.AddAll(Buttons, buttonObjs);
                }
            }

            var parentTextObjs = GetComponentsInParent<Text>();
            if (parentTextObjs != null && parentTextObjs.Length > 0) {
                var textObjs = new List<GameObject>();
                foreach (var obj in parentTextObjs) {
                    if (obj.tag.ToLower().Contains("advanced")) {
                        textObjs.Add(obj.gameObject);
                    }
                }

                if (textObjs != null && textObjs.Count > 0) {
                    TextObjects = ObjUtils.List.AddAll(TextObjects, textObjs);
                }
            }
        }

        if (Buttons != null && TextObjects != null) {
            AdvancedSettingsObjects = new List<GameObject>(
                Buttons.Count + TextObjects.Count
            );

            foreach (var button in Buttons) {
                if (button != null) {
                    AdvancedSettingsObjects.Add(button);
                }
            }

            foreach (var txtObj in TextObjects) {
                if (txtObj != null) {
                    AdvancedSettingsObjects.Add(txtObj);
                }
            }
        }
    }

    private void AutoDetectInputFields() {
        var inputFieldGameObjs = GameObject.FindGameObjectsWithTag("InputField");
        List<InputField> inputFields = null;

        if (!ObjUtils.List.IsNullOrEmpty(inputFieldGameObjs)) {
            inputFields = new List<InputField>(inputFieldGameObjs.Length);

            if (inputFields != null && inputFields.Count > 0) {
                foreach (var inputField in inputFields) {
                    if (!ShadowDistanceField) {
                        if (inputField.name.ToLower().Equals("ShadowDistanceInput".ToLower())) {
                            ShadowDistanceField = inputField;
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region PUBLIC FUNCTIONS
    public void FireResolutionDropdownItemChanged() {
        if (ResolutionChooser != null) {
            this.SelectedResolutionIndex = ResolutionChooser.value;
            this.SelectedResolution = Screen.resolutions[SelectedResolutionIndex];
            this.ResolutionChanged = true;

            if (IsFirstChange) {    //          <-- Fixes errors with not being able to re-select initial resolution
                if (SelectedResolution.height != InitialResolution.height
                    && SelectedResolution.width != InitialResolution.width
                    && SelectedResolution.refreshRate != InitialResolution.refreshRate) {
                    ToggleSettingsChanged();
                }
            } else {
                ToggleSettingsChanged();
            }
        }
    }

    public void ApplyGraphicsChanges() {
        if (ResolutionChanged) {
            Log.d("Changing resolution to \"" + Screen.resolutions[SelectedResolutionIndex] + "\"...");

            // sets the resolution to the changed value
            Screen.SetResolution(
                SelectedResolution.width,
                SelectedResolution.height,
                true,
                SelectedResolution.refreshRate
            );

            // resets 'initial resolution' in order for the new value 
            // to be the default in which the others are compared
            this.InitialResolution = Screen.currentResolution;
            Log.d("Resolution changed successfully");
        }

        IsFirstChange = false;
        ToggleSettingsChanged();
    }

    public void ToggleSettingsChanged() {
        ApplyButton.enabled = !ApplyButton.isActiveAndEnabled;
        var txt = ApplyButton.GetComponentInChildren<Text>();
        if (txt) {
            txt.color = txt.color == Color.white ? Color.gray : Color.white;
        }
    }

    public void FireEditingFinished(InputField sender) {
        if (sender.text != null && sender.text.ToCharArray().Length > 0) {
            var result = DefaultShadowDistanceBeforeChanged;
            var parsed = float.TryParse(sender.text, out result);
            if (parsed) {
                if (result.CompareTo(DefaultShadowDistanceBeforeChanged) != 0) {
                    if (sender.name.ToLower().Contains("shadowdistance")) {
                        Log.d("Attempting to change settings for \'"
                              + QualitySettings.shadowDistance.GetType().Name
                              + "To value " + result.ToString());
                        QualitySettings.shadowDistance = result;
                        if (QualitySettings.shadowDistance.CompareTo(DefaultShadowDistanceBeforeChanged) != 0) {
                            Log.d("Graphics settings changed successfully");
                        }
                    }
                }
            }
        }

        ToggleSettingsChanged();
    }

    public void FireShowAdvancedSettingsButtonPressed() {
        var scrollView = GameObject.FindWithTag("UIScrollView").GetComponent<RectTransform>();
        if (scrollView) {
            scrollView.sizeDelta = new Vector2(
                x: scrollView.sizeDelta.x,
                y: (ShowAdvancedSettings ?
                    PlayerGraphicsConfiguration.DefaultCollapsedScrollViewHeight :
                    PlayerGraphicsConfiguration.DefaultExpandedScrollViewHeight
                )
            );

            var verticalLayout = GameObject.FindWithTag("UIScrollView").GetComponent<VerticalLayoutGroup>();
            verticalLayout.CalculateLayoutInputVertical();
        } else {
            Log.d("Unable to modify scroll view height. Skipping procedure.");
        }

        this.ShowAdvancedSettings = !ShowAdvancedSettings;  // toggle view mode
        foreach (var obj in AdvancedSettingsObjects) {
            obj.SetActive(ShowAdvancedSettings);
        }

        if (!ShowAdvancedButton) {
            var tmp = GameObject.Find("ShowAdvancedSettingsBtn");
            if (tmp) {
                ShowAdvancedButton = tmp;
            }
        }

        var btnTxt = ShowAdvancedButton.GetComponentInChildren<Text>();
        if (btnTxt != null) {
            btnTxt.text = (btnTxt.text.ToLower().Contains("show") ? "Hide" : "Show") + " Advanced Settings";
        }
    }
    #endregion
}
