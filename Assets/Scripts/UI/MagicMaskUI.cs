using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMaskUI : BaseUI
{
    [SerializeField]
    public MagicMask magicMask;

    protected override bool canAutoHide { get => false; }

    protected override void Start()
    {
        base.Start();
        magicMask.Disable();
    }
}
