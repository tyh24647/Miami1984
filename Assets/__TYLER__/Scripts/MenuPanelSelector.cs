using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows for the selection of panels and enables/disables the other panels
/// when another is selected. This fixes issues when certain panels are enabled
/// in the inspector that shouldn't be, as well as if panels have been added to
/// the UI without applying the changes to the other related objects.
/// 
/// Author: Tyler Hostager
/// Version: 5/19/17
/// </summary>
[RequireComponent(typeof(GameObject))]
public class MenuPanelSelector : MonoBehaviour {

    [Header("Canvas")]
    public Canvas canvas;

    [Header("Panels")]
    public GameObject DefaultPanel;
    public List<GameObject> PanelsToDisable;

    #region INHERITED METHODS
    private void Awake() {
        // do nothing for now
    }

    // Use this for initialization
    void Start() {
        Log.d("Configuring menu...");
        ConfigureCanvas();
        ConfigurePanels();
        Log.d("Menu configuration complete");
    }

    // Update is called once per frame
    void Update() {

    }
    #endregion

    #region FUNCTIONS
    private void ConfigureCanvas() {
        if (!canvas) {
            Log.w("No \'Canvas\' attribute found. Attempting to auto-detect canvas...");
            try {
                var parentCanvas = FindObjectOfType(typeof(Canvas));
                if (parentCanvas) {
                    Log.d("Canvas successfully identified");
                    canvas = parentCanvas as Canvas;
                    Log.d("Canvas initialized successfully");
                }
            } catch (UnassignedReferenceException e) {
                Log.w("Exception thrown in canvas configuration");
                Log.d("Please add a canvas object manually in the inspector and re");
                Log.d("Exception details: " + e);
            }
        }
    }

    private void ConfigurePanels() {
        AddMissingChildPanels();
        EnableDefaultPanel();
    }

    private void AddMissingChildPanels() {
        if (DefaultPanel != null) {
            var childCount = canvas.transform.childCount;
            var children = new List<GameObject>(childCount);
            for (int i = 0; i < childCount; i++) {
                var child = canvas.transform.GetChild(i).gameObject;

                if (child) {
                    children.Insert(i, child);
                }
            }

            if (children != null) {
                var numChildPanels = children.Count;
                for (var i = 0; i < numChildPanels; i++) {
                    if (!children[i].tag.ToLower().Contains("UIPanel".ToLower())
                        || children[i] == null
                        || children[i].name.Equals(DefaultPanel.name)) {
                        numChildPanels--;
                    }
                }

                // fixes accidents when adding more panels without also adding them to this script
                if (PanelsToDisable.Count != numChildPanels) {
                    //PanelsToDisable = new List<GameObject>(numChildPanels);

                    for (var childIndex = 0; childIndex < numChildPanels; childIndex++) {
                        var child = children[childIndex];

                        if (!child || !child.tag.ToLower().Contains("UIPanel".ToLower())) {
                            continue;
                        }

                        if (childIndex < numChildPanels) {
                            if (!child.name.Equals(DefaultPanel.name)) {

                                //PanelsToDisable.Insert(childIndex, child);
                                if (!PanelsToDisable.Contains(child)) {
                                    Log.d("Adding panel to array...");
                                    PanelsToDisable.Add(child);
                                    Log.d("Panel added successfully");
                                }
                            }
                        } else {
                            Log.w("Unable to add array element \'" + child.name + "\' at index \'" + childIndex);

                        }
                    }
                }
            }
        }
    }

    // catch mistakes in default panel assignment
    private void ConfigureDefaultPanel() {
        if (!DefaultPanel) {
            var childCount = canvas.transform.childCount;
            Log.d("Auto-detecting root panel...");
            for (int i = 0; i < childCount; i++) {
                var child = canvas.transform.GetChild(i).gameObject;
                if (child.name.ToLower().Contains("main") || child.name.ToLower().Contains("default")) {
                    Log.d("Primary panel found. Assigning object...");
                    DefaultPanel = child;
                    Log.d("Main panel successfully configured");
                }
            }
        }

        Log.d("Enabling default panel, Disabling other panels");
        EnableDefaultPanel();
    }

    private void EnableDefaultPanel() {
        if (this.DefaultPanel) {

            // enable primary panel
            this.DefaultPanel.SetActive(true);

            // disable other panels
            if (PanelsToDisable.Count > 0) {
                foreach (var panel in PanelsToDisable) {
                    if (panel) {
                        panel.SetActive(false);
                    }
                }
            }
        }
    }

    private void EnableCustomPanelWithName(string panelName) {
        if (panelName == null || panelName.Equals("")) {
            return;
        }

        if (this.DefaultPanel.name.ToLower().Equals(panelName.ToLower())) {
            EnableDefaultPanel();
        } else {
            foreach (var panel in PanelsToDisable) {
                DefaultPanel.SetActive(false);
                panel.SetActive(panel.name.ToLower().Equals(panelName));
            }
        }
    }

    private void EnableCustomPanelWithTag(string tagName) {
        if (tag == null || tag.Equals("")) {
            return;
        }

        if (this.DefaultPanel.tag.ToLower().Equals(tagName.ToLower())) {
            EnableDefaultPanel();
        } else {
            foreach (var panel in PanelsToDisable) {
                DefaultPanel.SetActive(false);
                panel.SetActive(panel.tag.ToLower().Equals(tagName.ToLower()));
            }
        }
    }
    #endregion
}
