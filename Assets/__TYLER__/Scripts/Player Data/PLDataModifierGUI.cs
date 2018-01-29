using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


#if UNITY_EDITOR
public class PLDataModifierGUI : MonoBehaviour {
    private void OnGUI() {
        if (GUI.Button(new Rect(10, 200, 100, 30), "Health up")) { PLGameData.GameData.Health += 10; }
        if (GUI.Button(new Rect(10, 240, 100, 30), "Health down")) { PLGameData.GameData.Health -= 10; }
        if (GUI.Button(new Rect(10, 280, 100, 30), "Stamina boost")) { PLGameData.GameData.Stamina += 10; }
        if (GUI.Button(new Rect(10, 320, 100, 30), "Stamina drain")) { PLGameData.GameData.Stamina -= 10; }
        if (GUI.Button(new Rect(10, 400, 100, 30), "Save")) { PLGameData.GameData.Save(); }
        if (GUI.Button(new Rect(10, 440, 100, 30), "Load")) { PLGameData.GameData.Load(); }
        if (GUI.Button(new Rect(10, 480, 100, 30), "Reload Scene")) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
        if (GUI.Button(new Rect(10, Screen.height - 80, 100, 30), "Exit Game")) { Application.Quit(); }
        GUI.color = Color.red;
    }
}
#endif
