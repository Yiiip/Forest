using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log("2D " + hit.collider.gameObject.name);
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var all = Physics.RaycastAll(ray);
            foreach (RaycastHit item in all)
            {
                Debug.Log("all " + item.collider.gameObject.name);
            }

            Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log("col: " + col.Length);
            if (col.Length > 0)
            {
                foreach (Collider2D c in col)
                {
                    //do what you want
                }
            }
        }
    }
}