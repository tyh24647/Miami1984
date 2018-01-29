using UnityEngine;
using UnityEngine.UI;

public class PlayerNameDelegate : MonoBehaviour {

    private InputField InputField;

    private void Awake() {
        this.InputField = gameObject.GetComponent<InputField>();
        if (InputField) {
            InputField.onEndEdit.AddListener(
                delegate {
                    FireNameChanged();
                }
            );
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // assign new name and reload debug data
    public void FireNameChanged() {
        PLGameData.GameData.PlayerName = InputField.text;
        PLGameData.GameData.Save();
        PLGameData.GameData.Load();
    }
}
