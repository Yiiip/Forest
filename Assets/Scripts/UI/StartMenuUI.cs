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

            var exist = saveManager.IsSaveExist(i);
            if (exist)
            {
                txtDate.text = "";
                txtBtnLoad.text = "选择";
            }
            else
            {
                txtDate.text = "空存档";
                txtBtnLoad.text = "创建";
            }
        }
    }
}
