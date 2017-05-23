using System;
using UnityEditor;
using UnityEngine;



namespace Zios {
    public static class EditorWindowExtensions {
        public static void SetTitle(this EditorWindow current, string title, Texture2D icon = null) {
#if UNITY_5 && !UNITY_5_0
			current.titleContent = new GUIContent(title,icon);
#else
#pragma warning disable CS0618 // Type or member is obsolete
            current.title = title;
#pragma warning restore CS0618 // Type or member is obsolete
#endif
        }
    }
}
