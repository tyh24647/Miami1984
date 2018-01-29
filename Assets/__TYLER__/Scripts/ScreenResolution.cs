using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution {
    public static int LowestSupportedX = 800;
    public static int LowestSupportedY = 600;
    public static int DefaultXResolution = 1440;
    public static int DefaultYResolution = 900;
    public static int ScreenWidth = Screen.width;
    public static int ScreenHeight = Screen.height;

    private int _CurrentX;
    public int CurrentX {
        get {
            return _CurrentX < ScreenResolution.LowestSupportedX ? ScreenWidth > ScreenResolution.LowestSupportedX ? ScreenWidth : DefaultXResolution : ScreenWidth;
        }
        set {
            _CurrentX = value < ScreenResolution.LowestSupportedX ? ScreenWidth < ScreenResolution.LowestSupportedX ? DefaultXResolution : ScreenWidth : value;
        }
    }

    private int _CurrentY;
    public int CurrentY {
        get {
            return _CurrentY < ScreenResolution.LowestSupportedY ? ScreenHeight > ScreenResolution.LowestSupportedY ? ScreenHeight : DefaultYResolution : ScreenHeight;
        }
        set {
            _CurrentY = value < ScreenResolution.LowestSupportedY ? ScreenHeight < ScreenResolution.LowestSupportedY ? DefaultYResolution : ScreenHeight : value;
        }
    }

    public ScreenResolution() {
        CurrentX = Screen.width;
        CurrentY = Screen.height;
    }

    public ScreenResolution(int x, int y) {
        this.CurrentX = x;
        this.CurrentY = y;
    }

    public int X() {
        return CurrentX;
    }

    public int Y() {
        return CurrentY;
    }

}
