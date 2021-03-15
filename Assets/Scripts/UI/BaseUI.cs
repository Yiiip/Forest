using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView
{
    void Show(object intent);
    void Hide();
}

public abstract class BaseUI : MonoBehaviour, IView
{
    protected virtual bool canAutoHide { get => true; }

    protected object intent;

    protected virtual void OnEnable()
    {
        UIManager.CloseAllUI += AutoHide;
        // Debug.Log(name + " OnEnable");
    }

    protected virtual void Start()
    {
        // Debug.Log(name + " Start");
    }

    protected virtual void OnDisable()
    {
        UIManager.CloseAllUI -= AutoHide;
        // Debug.Log(name + " OnDisable");
    }

    protected virtual void Update()
    {
    }

    public virtual void Show(object intent)
    {
        UIManager.InvokeCloseAllUI();
        this.intent = intent;
        // Debug.Log(name + " set intent");
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    private void AutoHide()
    {
        if (canAutoHide)
        {
            Hide();
        }
    }
}
