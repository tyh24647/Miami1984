using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObject))]
[RequireComponent(typeof(Rigidbody))]
public class Delorean : MonoBehaviour {

    // user-editable values from within the editor
    public Rigidbody rBody;
    public float MinimumHeight;

    // default values
    private const float DEFAULT_MIN_HEIGHT = 32.0f;

    // Use this for initialization
    void Start() {
        MinimumHeight = DEFAULT_MIN_HEIGHT;

        if (rBody == null) {
            gameObject.AddComponent<Rigidbody>();
        }

        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Rigidbody>().detectCollisions = true;
        gameObject.GetComponent<Rigidbody>().drag = 0.0f;   // do this for now

    }

    // Update is called once per frame
    void Update() {
        if (MinimumHeight <= 0.00f) {
            MinimumHeight = DEFAULT_MIN_HEIGHT;
        }

        if (gameObject && gameObject.GetComponent<Rigidbody>()) {
            var originalPos = gameObject.GetComponent<Rigidbody>().position;

            if (originalPos.y <= 31) {
                gameObject.GetComponent<Rigidbody>().MovePosition(
                    new Vector3(
                        originalPos.x,
                        MinimumHeight,
                        originalPos.z
                    )
                );
            }
        }
    }

    /*
     * ++++++++++++++++++++++++++++++++++++++++++++
     * 
     *          TODO: GET THIS WORKING!!!!!!!
     * 
     * 
     * */
}
