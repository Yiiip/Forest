using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class SaveData : IPersistentObject
{
    private static SaveData _current;

    public static SaveData current
    {
        get
        {
            if (_current == null)
            {
                _current = new SaveData();
                _current.Init();
            }
            return _current;
        }
    }

    public static void InitSaveData(SaveData saveData)
    {
        _current = saveData;
        _current.Init();
    }

    public static void CleanUp()
    {
        _current = null;
    }

    public string version;
    public int slotIndex;

    public PlayerProfilePO playerProfile;
    public SettingPO setting;
    public TutorialPO tutorial;

    public void Init()
    {
        if (playerProfile == null)
        {
            playerProfile = new PlayerProfilePO();
            playerProfile.Init();
        }

        if (setting == null)
        {
            setting = new SettingPO();
            setting.Init();
        }

        if (tutorial == null)
        {
            tutorial = new TutorialPO();
            tutorial.Init();
        }
    }
}
