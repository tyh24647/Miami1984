﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows for the mouse to stay within the desired bounds, as well as process
/// any additional input/key bindings during debug mode.
/// </summary>
public class TyCustomInputManager : MonoBehaviour {

    private static readonly Vector3 DefaultPosition = new Vector3(0, 0, -10);
    private Vector3 position;

    private const int DefaultBoundary = 50;
    private const int DefaultSpeed = 4;

    private int screenBoundsWidth;
    private int screenBoundsHeight;
    private int boundary;
    private int speed;

    public int ScreenBoundsWidth {
        get { return screenBoundsWidth; }
        set { screenBoundsWidth = value < 0 ? screenBoundsWidth < 0 ? 800 : screenBoundsWidth : value; }
    }

    public int ScreenBoundsHeight {
        get { return screenBoundsHeight; }
        set { screenBoundsHeight = value < 0 ? screenBoundsHeight < 0 ? 600 : screenBoundsHeight : value; }
    }

    public int Boundary {
        get { return boundary > 0 ? boundary : DefaultBoundary; }
        set { boundary = value < 0 ? DefaultBoundary : boundary; }
    }

    public int Speed {
        get { return speed; }
        set { speed = value; }
    }

    // Use this for initialization
    void Start() {
        Debug.Log("Custom input manager initialized");
        speed = TyCustomInputManager.DefaultSpeed;
        boundary = TyCustomInputManager.DefaultBoundary;
        position = TyCustomInputManager.DefaultPosition;
        screenBoundsWidth = Screen.width;
        screenBoundsHeight = Screen.height;
    }

    // Update is called once per frame
    void Update() {
        if (Input.mousePosition.x < screenBoundsWidth && Input.mousePosition.y < screenBoundsHeight) {
            if (Input.mousePosition.x > (screenBoundsWidth - boundary)) {
                position.x += speed * Time.deltaTime;
            }
        }

        if (Input.mousePosition.x > 0 && Input.mousePosition.y > 0) {
            if (Input.mousePosition.x < 0 + boundary) {
                position.x -= speed * Time.deltaTime;
            }
        }

        if (Input.mousePosition.y < screenBoundsHeight && Input.mousePosition.x < screenBoundsWidth) {
            if (Input.mousePosition.y > screenBoundsHeight - 10) {
                position.y += speed * Time.deltaTime;
            }
        }

        if (Input.mousePosition.y > 0 && Input.mousePosition.x > 0) {
            if (Input.mousePosition.y < 0 + boundary) {
                position.y -= speed * Time.deltaTime;
            }
        }

        //Camera.main.transform.position = this.position;
        GetComponent<Camera>().transform.position = position;
    }
}
