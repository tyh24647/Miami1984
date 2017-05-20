using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGraphicsConfiguration : MonoBehaviour {

    private static float DefaultShadowDistanceBeforeChanged;

    public const float DefaultCollapsedScrollViewHeight = 280f;
    public const float DefaultExpandedScrollViewHeight = 1000f;



    public float ShadowDistance {
        get { return QualitySettings.shadowDistance; }
        set { QualitySettings.shadowDistance = value < 0 ? DefaultShadowDistanceBeforeChanged : value; }
    }

    [Space(1)]
    [Header("Input Fields")]
    public InputField ShadowDistanceField;

    [Header("Show/Hide Advanced Settings Objects:")]
    public bool ShowAdvancedSettings = false;
    public List<GameObject> Buttons;
    public List<GameObject> TextObjects;


    private List<GameObject> AdvancedSettingsObjects = null;
    private Text ShowAdvancedButton;



    private void Awake() {
        //ShadowDistanceField.contentType = InputField.ContentType.DecimalNumber;
        DefaultShadowDistanceBeforeChanged = QualitySettings.shadowDistance;
        ShadowDistance = DefaultShadowDistanceBeforeChanged;

        if (ShadowDistanceField != null) {
            ShadowDistanceField.placeholder.enabled = false;
            ShadowDistanceField.textComponent.text = DefaultShadowDistanceBeforeChanged.ToString();
        }
        //this.ShowAdvancedSettings = false;  // disable by default
        //AutoDetectAdvancedSettingsObjects();
    }

    // Use this for initialization
    void Start() {
        AutoDetectAdvancedSettingsObjects();
        AutoDetectInputFields();

        foreach (var settingsObj in AdvancedSettingsObjects) {
            settingsObj.SetActive(ShowAdvancedSettings);
        }
    }

    // Update is called once per frame
    void Update() {

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

    public static void FireEditingFinished(InputField sender) {
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
            var tmp = GetComponentsInParent<Text>();
            foreach (var parentObj in tmp) {
                if (parentObj.text.ToLower().Contains("show")) {
                    parentObj.text = "Hide Advanced Settings";
                } else if (parentObj.text.ToLower().Contains("hide")) {
                    parentObj.text = "Show Advanced Settings";
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

}
