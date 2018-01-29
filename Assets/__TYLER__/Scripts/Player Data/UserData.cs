using UnityEngine;
using System.Collections;

public static class UserData {
    public static void SetPlayerName(string playerName) {
        PLGameData.GameData.PlayerName = playerName;
        PLGameData.GameData.Save();
        PLGameData.GameData.Load();
    }
}