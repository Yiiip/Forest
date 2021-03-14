using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingEntity : MonoBehaviour
{
    void Start()
    {
        var lis = EventTriggerListener.Get(this.gameObject);
        lis.onClick = delegate(GameObject go)
        {
            Debug.Log("onclick");
        };
        lis.onBeginDrag = delegate(PointerEventData eventData)
        {
            Debug.Log("onBeginDrag");
        };
        lis.onDrag = delegate(PointerEventData eventData)
        {
            Debug.Log("onDrag");
        };
        lis.onEndDrag = delegate(PointerEventData eventData)
        {
            Debug.Log("onEndDrag");
        };
        lis.onEnter = delegate(GameObject go)
        {
            Debug.Log("onEnter");
        };
        lis.onExit = delegate(GameObject go)
        {
            Debug.Log("onExit");
        };
    }

}
