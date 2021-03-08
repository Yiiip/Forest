using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public Toggle toggleFPS;
    public Button btnSaveAndQuit;

    private void Start()
    {
        btnSaveAndQuit.onClick.AddListener(SaveAndQuit);
    }

    public void SaveAndQuit()
    {
        
    }
}
