using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomSliderValues : MonoBehaviour {

    private const float DefaultMaximumVolume = 1.0f;
    private const string SliderTag = "UISlider";

    private float StartingMasterVolume;
    private Slider MasterVolumeSlider, MusicVolumeSlider, SFXVolumeSlider;

    public Canvas MainCanvas;
    public List<Slider> Sliders;

    private void Awake() {
        //VerifySliders();
        //AutoDetectMasterVolumeSlider();

        //if (MasterVolumeSlider != null) {
        //    SecondaryVolumeSliders = DetectSecondaryVolumeSliders();
        //}

        //if (VolumeSliders != null) {
        //    if (VolumeSliders.Count > 0) {
        //        MasterVolumeSlider = AutoDetectMasterVolumeSlider();
        //    }
        //} else {


        //}
    }

    // Use this for initialization
    void Start() {
        //VerifySliders();
        //if (MasterVolumeSlider != null) {
        //    SecondaryVolumeSliders = DetectSecondaryVolumeSliders();
        //}

        MasterVolumeSlider = Sliders[0];
        MusicVolumeSlider = Sliders[1];
        SFXVolumeSlider = Sliders[2];

        MasterVolumeSlider.onValueChanged.AddListener(
            delegate {
                FireVolumeChangedListener();
            }
        );
    }

    // Update is called once per frame
    void Update() {
        //FireVolumeChangedListener();
    }

    private void VerifySliders() {
        if (Sliders == null || Sliders.Count == 0) {
            Log.w("No \'Slider\' Objects found");
            Log.d("Initializing auto-detection of \'Slider\' objects in the current canvas");

            var detectedSliders = AutoDetectSlidersInCanvas();
            if (detectedSliders != null && detectedSliders.Length > 0) {
                foreach (var slider in AutoDetectSlidersInCanvas()) {
                    Sliders.Add(slider);
                }
            }

            //Sliders = AutoDetectSlidersInCanvas();
            Log.d("Automatic \'Slider\' object detection "
                  + Sliders != null && Sliders.Count > 0 ? "successful" : "failed");
        }
    }

    private Slider[] AutoDetectSlidersInCanvas() {
        Log.d("Searching for \'Slider\' objects in canvas \'" + MainCanvas.name + "\'");

        // reconfigure, then recurse

        Slider[] sliders = null;
        //var sliderObjs = MainCanvas.gameObject.GetComponentsInChildren<Slider>();
        var sliderObjs = MainCanvas.GetComponentsInChildren<Slider>();
        if (sliderObjs == null || sliderObjs.Length == 0) {
            //    sliderObjs = FindObjectsOfType<Slider>();
            //sliderObjs = MainCanvas.gameObject.GetComponents<Slider>();
            //if (sliderObjs == null || sliderObjs.Length == 0) {
            //    sliderObjs = MainCanvas.gameObject.GetComponentsInParent<Slider>();
            //    if (sliderObjs == null || sliderObjs.Length == 0) {
            //        sliderObjs = FindObjectsOfType<Slider>();
            //    }
            //}
        }

        if (sliderObjs != null && sliderObjs.Length > 0) {
            sliders = sliderObjs;

            foreach (var slider in sliders) {
                if (slider.tag.ToLower().Contains("volume") || slider.name.ToLower().Contains("volume")) {
                    //VolumeSliders.Add(slider);
                }
            }
        }

        return sliders;
    }

    private void VerifyCanvas() {
        int numCanvasChecks = 0;

    VerifyCanvas:
        if (!MainCanvas) {
            if (numCanvasChecks < 5) {
                AutoDetectCanvas();
                numCanvasChecks++;
                goto VerifyCanvas;
            }

            AutoDetectSlidersInCanvas();
            if (!MainCanvas && (Sliders == null || Sliders.Count == 0)) {
                Log.w("Canvas not found");
            }
        }
    }

    //private List<Slider> DetectSecondaryVolumeSliders() {
    //    List<Slider> secondaryVolumeSliders = VolumeSliders;
    //    foreach (var slider in VolumeSliders) {
    //        if (!slider.Equals(MasterVolumeSlider)) {
    //            secondaryVolumeSliders.Add(slider);
    //        }
    //    }

    //    return secondaryVolumeSliders;
    //}

    private void FireVolumeChangedListener() {
        if (Sliders != null && Sliders.Count > 0 && MasterVolumeSlider) {

            // set volume limit of secondary sliders to equal the master volume limit
            foreach (var slider in Sliders) {
                if (!slider.Equals(MasterVolumeSlider)) {
                    slider.maxValue = MasterVolumeSlider.value;
                }
            }
        }
    }

    private Slider AutoDetectMasterVolumeSlider() {
        Slider masterVolSlider = null;

        //if (VolumeSliders.Count > 0) {
        //    for (var i = 0; masterVolSlider == null && i < VolumeSliders.Count; i++) {
        //        var slider = VolumeSliders[i];
        //        if (slider.name.ToLower().Contains("master") || slider.name.ToLower().Contains("main")) {
        //            masterVolSlider = slider;
        //        }
        //    }
        //}

        return masterVolSlider;
    }

    private Canvas AutoDetectCanvas() {
        Log.w("Attempting to auto-detect canvas...");
        try {
            var parentCanvas = FindObjectOfType(typeof(Canvas));
            if (parentCanvas) {
                Log.d("Canvas successfully identified");
                return parentCanvas as Canvas;
            }
        } catch (UnassignedReferenceException e) {
            Log.w("Exception thrown in canvas configuration");
        }

        return gameObject.GetComponentInParent<Canvas>();
    }

}
