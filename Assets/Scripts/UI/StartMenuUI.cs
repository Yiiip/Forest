using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    public SaveManager saveManager;
    public VerticalLayoutGroup saveSlotsLayout;
    public GameObject[] saveSlots;

    private void Start()
    {
        for (int i = 0; i < saveManager.saveDataName.saveNames.Count; i++)
        {
            GameObject slot = saveSlots[i];
            Text txtName = slot.transform.Find("txtName").GetComponent<Text>();
            Text txtDate = slot.transform.Find("txtDate").GetComponent<Text>();
            Button btnLoad = slot.transform.Find("btnLoad").GetComponent<Button>();
            Text txtBtnLoad = slot.transform.Find("btnLoad/Text").GetComponent<Text>();

            DateTime lastModifyDt;
            bool exist = saveManager.IsSaveExist(i, out lastModifyDt);
            if (exist)
            {
                txtDate.text = lastModifyDt.ToString("yyyy/MM/dd HH:mm:ss");
                txtBtnLoad.text = "载入";
                btnLoad.onClick.AddListener(delegate()
                {
                    LoadOrCreate(true);
                });
            }
            else
            {
                txtDate.text = "空存档";
                txtBtnLoad.text = "创建";
                btnLoad.onClick.AddListener(delegate()
                {
                    LoadOrCreate(false);
                });
            }
        }
    }

    private void LoadOrCreate(bool createNew)
    {
        
    }
}
