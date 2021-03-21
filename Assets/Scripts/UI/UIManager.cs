using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : SingletonMono<UIManager>
{
    [SerializeField]
    public Canvas canvas;

    [SerializeField]
    private GraphicRaycaster graphicRaycaster = null;

    private PointerEventData pointerData = new PointerEventData(null);

    public static event Action CloseAllUI = delegate() {};

    private List<BaseUI> views = new List<BaseUI>();

    private void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        if (canvas != null)
        {
            for (int i = 0; i < canvas.transform.childCount; i++)
            {
                var comp = canvas.transform.GetChild(i).GetComponent<BaseUI>();
                if (comp != null)
                {
                    views.Add(comp);
                }
            }
        }
    }

    public static void InvokeCloseAllUI()
    {
        CloseAllUI();
    }

    public void Show(Type tp, object intent = null)
    {
        foreach (var view in views)
        {
            if (view.GetType() == tp)
            {
                view.Show(intent);
                break;
            }
        }
    }

    public T GetUI<T>() where T : BaseUI
    {
        foreach (var view in views)
        {
            if (view.GetType() == typeof(T))
            {
                return view as T;
            }
        }
        return null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI())
            {
            }
            else
            {
                InvokeCloseAllUI();
            }
        }
    }

    private List<RaycastResult> results;
    public bool IsPointerOverUI()
    {
        pointerData.position = Input.mousePosition;
        if (results == null)
            results = new List<RaycastResult>();
        else
            results.Clear();
        if (graphicRaycaster != null)
            graphicRaycaster.Raycast(pointerData, results);
        return results.Count > 0;
    }
}
