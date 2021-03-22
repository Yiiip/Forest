using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicMaskUI : BaseUI
{
    [SerializeField]
    public MagicMask magicMask;

    protected override bool canAutoHide { get => false; }

    protected override void Start()
    {
        base.Start();

        // First in
        magicMask.RemoveTarget()
            .SetCenter(Vector2.zero)
            .Focus(fromRadius: 0f, toRadius: Screen.width, duration: 1f, onFinish: () =>
        {
            magicMask.Disable();
        });
    }
}
