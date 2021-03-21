using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class StartMenuUI : BaseUI
{
    public VerticalLayoutGroup saveSlotsLayout;
    public GameObject[] saveSlots;
    public SaveDataName saveDataName;

    protected override void Start()
    {
        for (int i = 0; i < SaveManager.Instance.saveDataName.saveNames.Count; i++)
        {
            int slotIndex = i;
            GameObject slot = saveSlots[slotIndex];
            Text txtName = slot.transform.Find("txtName").GetComponent<Text>();
            Text txtDate = slot.transform.Find("txtDate").GetComponent<Text>();
            Button btnLoad = slot.transform.Find("btnLoad").GetComponent<Button>();
            Text txtBtnLoad = slot.transform.Find("btnLoad/Text").GetComponent<Text>();
            Button btnDelete = slot.transform.Find("btnDelete").GetComponent<Button>();

            DateTime lastModifyDt;
            var exist = SaveManager.Instance.IsSaveExist(slotIndex, out lastModifyDt);
            if (exist)
            {
                txtDate.text = lastModifyDt.ToString("yyyy/MM/dd HH:mm:ss");
                txtBtnLoad.text = "载入";
                btnLoad.onClick.RemoveAllListeners();
                btnLoad.onClick.AddListener(delegate()
                {
                    LoadOrCreate(false, slotIndex);
                });
                btnDelete.onClick.RemoveAllListeners();
                btnDelete.onClick.AddListener(delegate()
                {
                    SaveManager.Instance.Delete(slotIndex);
                    SetAsEmptyStyle();
                });
                btnDelete.interactable = true;
                btnDelete.gameObject.SetActiveOptimize(true);
            }
            else
            {
                SetAsEmptyStyle();
            }

            void SetAsEmptyStyle()
            {
                txtDate.text = "空存档";
                txtBtnLoad.text = "创建";
                btnLoad.onClick.RemoveAllListeners();
                btnLoad.onClick.AddListener(delegate()
                {
                    LoadOrCreate(true, slotIndex);
                });
                btnDelete.interactable = false;
                btnDelete.gameObject.SetActiveOptimize(false);
            }

            if (i < saveDataName.saveNicknames.Count)
            {
                txtName.text = saveDataName.saveNicknames[slotIndex];
            }
        }
    }

    private void LoadOrCreate(bool createNew, int slotIndex)
    {
        if (createNew)
        {
            SaveManager.Instance.Create(slotIndex);
        }
        else
        {
            SaveManager.Instance.OnLoad(slotIndex);
        }
        SceneManager.LoadScene("Forest");
    }
}
