using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Component))]
public class PressAnyKeyBlinkingTxt : MonoBehaviour {

    [SerializeField]
    public Component blinkingText;
    private CanvasRenderer CanvasRenderer;

    // Use this for initialization
    void Start() {
        var tmp = null as CanvasRenderer;
        if (!blinkingText) {
            tmp = GetComponentInChildren<Text>().GetComponent<CanvasRenderer>();
        } else {
            tmp = blinkingText.GetComponent<CanvasRenderer>();
        }

        if (CanvasRenderer == null && tmp != null) {
            CanvasRenderer = tmp;
        }
    }

    // Update is called once per frame
    void Update() {
        if (CanvasRenderer) {
            CanvasRenderer.SetAlpha(Time.fixedTime %
                                    0.5f//0.5f//0.5f 
                                    <
                                    0.125f//0.2f 
                                    ? 0.0f : 1.0f);
        }
    }



}




//			StartCoroutine(Blink(2.0));

//function Blink(waitTime : float) {
//	var endTime = Time.time + waitTime;
//	while (Time.time < waitTime) {
//		renderer.enabled = false;
//		yield WaitForSeconds(0.2);
//		renderer.enabled = true;
//		yield WaitForSeconds(0.2);
//	}
//}