using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView
{
}

public class BaseUI : MonoBehaviour, IView
{
    protected virtual void Start()
    {
    }

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
    }

    protected virtual void Update()
    {
    }
}
