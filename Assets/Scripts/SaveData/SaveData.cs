﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : IPersistentObject
{
    private static SaveData _current;

    public static SaveData current
    {
        get
        {
            if (_current == null)
            {
                _current = new SaveData();
            }
            return _current;
        }
    }

    public static void InitSaveData(SaveData saveData)
    {
        _current = saveData;
        _current.Init();
    }

    public string version;
    public int slotIndex;

    public PlayerProfilePO playerProfile;
    public SettingPO setting;

    public void Init()
    {
        if (playerProfile == null)
            playerProfile = new PlayerProfilePO();
        playerProfile.Init();

        if (setting == null)
            setting = new SettingPO();
        setting.Init();
    }
}
