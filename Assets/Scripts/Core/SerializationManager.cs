using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SerializationManager
{
    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        return formatter;
    }

    public static string GetSavePath()
    {
        return $"{Application.persistentDataPath}/saves";
    }

    public static string GetSaveFilePath(string saveName)
    {
        return $"{Application.persistentDataPath}/saves/{saveName}.save";
    }

    public static bool Save(string saveName, object saveData)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        var savesPath = GetSavePath();
        if (!Directory.Exists(savesPath))
        {
            Directory.CreateDirectory(savesPath);
        }

        var saveFilePath = GetSaveFilePath(saveName);
        FileStream file = File.Create(saveFilePath);
        try
        {
            formatter.Serialize(file, saveData);
            file.Close();
            Debug.Log($"Save -> {saveFilePath}");
            return true;
        }
        catch (Exception e)
        {
            file.Close();
            Debug.LogError(e);
            return false;
        }
    }

    public static object LoadByPath(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        BinaryFormatter formatter = GetBinaryFormatter();

        FileStream file = File.Open(path, FileMode.Open);
        try
        {
            object data = formatter.Deserialize(file);
            file.Close();
            Debug.Log($"Load -> {path}");
            return data;
        }
        catch (Exception e)
        {
            file.Close();
            Debug.LogError(e);
            return null;
        }
    }

    public static object Load(string saveName)
    {
        var saveFilePath = GetSaveFilePath(saveName);
        return LoadByPath(saveFilePath);
    }
}
