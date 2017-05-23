using UnityEngine;
using System.Collections;

public class PLDataModifierGUI : MonoBehaviour {
    private void OnGUI() {
        if (GUI.Button(new Rect(10, 100, 100, 30), "Health up")) { PLGameData.GameData.Health += 10; }
        if (GUI.Button(new Rect(10, 140, 100, 30), "Health down")) { PLGameData.GameData.Health -= 10; }
        if (GUI.Button(new Rect(10, 180, 100, 30), "Stamina boost")) { PLGameData.GameData.Stamina += 10; }
        if (GUI.Button(new Rect(10, 220, 100, 30), "Stamina drain")) { PLGameData.GameData.Stamina -= 10; }
        if (GUI.Button(new Rect(10, 300, 100, 30), "Save")) { PLGameData.GameData.Save(); }
        if (GUI.Button(new Rect(10, 340, 100, 30), "Load")) { PLGameData.GameData.Load(); }
    }
}
