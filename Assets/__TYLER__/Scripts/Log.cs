using System;
using UnityEngine;


/// <summary>
/// Basic logger that provides custom functionality that isn't included
/// in the <code>Debug.Log()</code> function.
/// 
/// Author:     Tyler Hostager
/// Version:    05/10/17
/// </summary>
public static class Log {

#if DEBUG
    #region  Debug Logging
    public static void d(string debugMessage) {
        Debug.Log(debugMessage);
    }

    public static void d(string debugMessage, string title) {
        d(title + ": " + debugMessage);
    }

    public static void d(string debugMessage, UnityEngine.Object sender) {
        Debug.Log(debugMessage, sender);
    }

    public static void d(string debugMessage, string title, UnityEngine.Object sender) {
        Debug.Log(title + ": " + debugMessage, sender);
    }
    #endregion

    #region Warning Logging
    public static void w(string warning) {
        Debug.LogWarning(warning);
    }

    public static void w(string warning, string title) {
        w(title + ": " + warning);
    }

    public static void w(string warning, UnityEngine.Object sender) {
        Debug.LogWarning(warning, sender);
    }

    public static void w(string warning, string title, UnityEngine.Object sender) {
        Debug.LogWarning(title + ": " + warning, sender);
    }
    #endregion

    #region Error Logging
    public static void e(string errorMessage) {
        Debug.LogError(errorMessage);
    }

    public static void e(string errorMessage, string errorTitle) {
        e(errorTitle + ": " + errorMessage);
    }

    public static void e(string errorMessage, Exception cause) {
        Debug.LogError(errorMessage);
        Debug.LogException(cause);
    }

    public static void e(string errorMessage, UnityEngine.Object errorRootObject) {
        Debug.LogError(errorMessage, errorRootObject);
    }

    public static void e(string errorMessage, Exception cause, UnityEngine.Object errorRootObject) {
        Debug.LogError(errorMessage, errorRootObject);
        Debug.LogException(cause);
    }

    public static void e(string errorMessage, string errorTitle, Exception cause) {
        Debug.LogError(errorTitle + ": " + errorMessage);
        Debug.LogException(cause);
    }

    public static void e(string errorMessage, string errorTitle, UnityEngine.Object errorRootObject) {
        Debug.LogError(errorTitle + ": " + errorMessage, errorRootObject);
    }

    public static void e(string errorMessage, string errorTitle, Exception cause, UnityEngine.Object errorRootObject) {
        Debug.LogError(errorTitle + ": " + errorMessage, errorRootObject);
        Debug.LogException(cause);
    }
    #endregion
#endif
}
