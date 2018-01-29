using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __TYLER__.TY_SUIMONO_WATER_SYSTEM {

    /// <summary>
    /// Suimono object.
    /// </summary>
    [ExecuteInEditMode]
    public class SuimonoObject : MonoBehaviour {
        String suimonoVersionNumber = "";

        //preset variables
        Boolean useDarkUI = true;
        //RenderTexture renderTex;
        int presetIndex;
        private int presetUseIndex;
        String[] presetOptions;

        int presetFileIndex;
        int presetFileUseIndex;
        String[] presetFiles;

        float refractShift = 1.0f;
        float refractScale = 1.0f;
        float blurSpread = 0.1f;
        bool hasStarted = false;
        float setShoreWaveScale = 1.0f;
        float setFlowShoreScale = 1.0f;
        float setflowOffX = 0.0f;
        float setflowOffY = 0.0f;
        float usewaveShoreHt = 0.0f;
        float waveBreakAmt = 1.0f;
        float shallowFoamAmt = 1.0f;

        float shoreOffX = 0.0f;
        float shoreOffY = 0.0f;

        int presetTransIndexFrm = 0;
        int presetTransIndexTo = 0;
        bool presetStartTransition = false;
        float presetTransitionTime = 3.0f;
        float presetTransitionCurrent = 1.0f;
        String presetSaveName = "my custom preset";
        bool presetToggleSave = false;
        String[] presetDataArray;
        String presetDataString;
        String baseDir = "SUIMONO - WATER SYSTEM 2";
        GameObject suimonoModuleObject;
        //SuimonoModuleLib suimon

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }

}

