using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Singleton<T> where T : class
{
    private static T _instance;

    static Singleton()
    {
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T) Activator.CreateInstance(typeof(T), true);
            }
            return _instance;
        }
    }

    public static void Destroy()
    {
        _instance = null;
    }
}