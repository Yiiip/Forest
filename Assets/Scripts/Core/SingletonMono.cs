using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    protected const string TAG = "[Singleton]";

    private static T _instance;

    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning($"{TAG} Instance '{typeof(T)}' already destroyed on application quit. Returning null.");
                return null;
            }

            lock(_lock)
            {
                if (_instance == null)
                {
                    _instance = (T) FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError($"{TAG} Something went wrong. There should never be more than 1 singleton! Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        // Debug.Log("--1. new GameObject()");
                        _instance = singleton.AddComponent<T>();
                        singleton.name = TAG + " " + typeof(T).Name;
                        // Debug.Log("--3. AddComponent");

                        Debug.Log($"{TAG} An instance of '{typeof(T)}' is needed in the scene, so '{singleton}' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log($"{TAG} Using instance already created: {_instance.gameObject.name}");
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        applicationIsQuitting = false;
        // Debug.Log("--2. OnAwake");
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    protected virtual void OnDestroy()
    {
        applicationIsQuitting = true;
        // Debug.Log("--4. OnDestroy");
    }

    public static bool IsDestroying()
    {
        return applicationIsQuitting;
    }
}