using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SaveManager : SingletonMonoEntire<SaveManager>
{
    [SerializeField]
    public SaveDataName saveDataName;

    private int currentSlotIndex = -1;

    public void OnSaveCurrent()
    {
        OnSave(currentSlotIndex);
    }

    public void OnSave(int slotIndex)
    {
        if (slotIndex >= 0
            && saveDataName.saveNames.Count > 0
            && slotIndex <= saveDataName.saveNames.Count - 1)
        {
            SaveData.current.version = Application.version;

            var saveName = saveDataName.saveNames[slotIndex];
            SerializationManager.Save(saveName, SaveData.current);
        }
    }

    public bool IsSaveExist(int slotIndex, out DateTime lastModifyDt)
    {
        lastModifyDt = default;
        if (slotIndex >= 0
            && saveDataName.saveNames.Count > 0
            && slotIndex <= saveDataName.saveNames.Count - 1)
        {
            var savePath = SerializationManager.GetSavePath();
            if (!Directory.Exists(savePath))
            {
                return false;
            }

            var saveName = saveDataName.saveNames[slotIndex];
            var saveFilePath = SerializationManager.GetSaveFilePath(saveName);
            bool exist = File.Exists(saveFilePath);
            if (exist)
            {
                lastModifyDt = File.GetLastWriteTime(saveFilePath);
            }
            return exist;
        }
        return false;
    }

    public void OnLoad(int slotIndex)
    {
        if (slotIndex >= 0
            && saveDataName.saveNames.Count > 0
            && slotIndex <= saveDataName.saveNames.Count - 1)
        {
            this.currentSlotIndex = slotIndex;
            var saveName = saveDataName.saveNames[slotIndex];
            var loaded = (SaveData) SerializationManager.Load(saveName);
            SaveData.InitSaveData(loaded);
        }
    }

    public void Create(int slotIndex)
    {
        if (slotIndex >= 0
            && saveDataName.saveNames.Count > 0
            && slotIndex <= saveDataName.saveNames.Count - 1)
        {
            this.currentSlotIndex = slotIndex;
            var saveName = saveDataName.saveNames[slotIndex];
            SaveData.current.slotIndex = slotIndex;
            SaveData.current.Init();
            OnSaveCurrent();
        }
    }

    public void DeleteCurrent()
    {
        Delete(currentSlotIndex);
        SaveData.CleanUp();
    }

    public bool Delete(int slotIndex)
    {
        if (slotIndex >= 0
            && saveDataName.saveNames.Count > 0
            && slotIndex <= saveDataName.saveNames.Count - 1)
        {
            var savePath = SerializationManager.GetSavePath();
            if (!Directory.Exists(savePath))
            {
                return false;
            }

            var saveName = saveDataName.saveNames[slotIndex];
            var saveFilePath = SerializationManager.GetSaveFilePath(saveName);
            bool exist = File.Exists(saveFilePath);
            if (exist)
            {
                File.Delete(saveFilePath);
                return true;
            }
        }
        return false;
    }

    private void OnApplicationQuit()
    {
        if (currentSlotIndex >= 0)
        {
            if (SaveData.current.setting.autoSaveWhenQuit)
            {
                GameManager.Instance.OnSave();
                OnSaveCurrent();
            }
        }
    }
}
