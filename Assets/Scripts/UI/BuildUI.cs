using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : BaseUI
{
    public Button BtnClose;

    protected override void Start()
    {
        base.Start();
        BtnClose.onClick.RemoveAllListeners();
        BtnClose.onClick.AddListener(delegate()
        {
            AudioManager.Instance.PlaySound(AudioConst.button);
            base.Hide();
        });
    }
}
