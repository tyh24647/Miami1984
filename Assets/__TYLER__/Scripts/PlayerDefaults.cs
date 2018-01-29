using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerDefaults {
    bool IsDebug();
    bool IsStandardUser();
    bool CanSaveData();
    bool ShouldSaveGame();
    bool ShouldSaveAppPrefs();
    bool ShouldPlayMusic();
    bool IsFirstRun();
    Dictionary<string, ScreenResolution> SupportedResolutions();
}
