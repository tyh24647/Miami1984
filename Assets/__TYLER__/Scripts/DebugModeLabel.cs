using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DebugModeLabel : MonoBehaviour {

    public Text DebugLabel;

    // Awake is called when the script instance is being loaded
    private void Awake() {
        DebugLabel.text = "Debug mode: ";

        if (gameObject) {

            if (DebugLabel) {
                var isDebug = false;
#if DEBUG
                isDebug = true;
#endif
                DebugLabel.text += isDebug ? "ENABLED" : "DISABLED";
            } else {
                Log.w("Unable to instantiate debug label. Skipping procedure.");
                var textLabels = FindObjectsOfType(typeof(Text));
                if (textLabels.Length > 0) {
                    foreach (var label in textLabels) {
                        if (label.name.ToLower().Contains("debug")) {
                            DebugLabel = label as Text;
                        }
                    }
                }
            }
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
