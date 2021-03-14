using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Picker : MonoBehaviour
{
    [SerializeField]
    private Camera curCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Worked in orthographic camera
            Vector2 mousePos = Input.mousePosition;
            Vector3 worldPoint = curCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log($"2D hit: {hit.collider.gameObject.name}");
            }

            // Worked in perspective camera
            // Ray ray = curCamera.ScreenPointToRay(Input.mousePosition);
            // RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            // if (hit.transform != null)
            // {
            //     Debug.Log($"Intersection: {hit.collider.gameObject.name}");
            // }
        }
    }
}