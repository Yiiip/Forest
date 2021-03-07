using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    public SaveDataName saveDataName;

    public void OnSave(int slotIndex)
    {
        if (slotIndex >= 0
            && saveDataName.saveNames.Count > 0
            && slotIndex <= saveDataName.saveNames.Count - 1)
        {
            var saveName = saveDataName.saveNames[slotIndex];
            SerializationManager.Save(saveName, SaveData.current);
        }
    }

    public bool IsSaveExist(int slotIndex)
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
            Debug.Log(saveFilePath);
            return File.Exists(saveFilePath);
        }
        return false;
    }
}
