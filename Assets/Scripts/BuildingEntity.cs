using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingEntity : MonoBehaviour
{
    void Start()
    {
    }

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
    }
    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
    }

    private void OnMouseDrag()
    {
        Debug.Log("OnMouseDrag");
    }
}
