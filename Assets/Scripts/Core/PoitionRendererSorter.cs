using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoitionRendererSorter : MonoBehaviour
{
    [SerializeField]
    private int sortingOrderBase = 5000;
    [SerializeField]
    private int offset = 0;
    [SerializeField]
    private bool runOnce = false;
    [SerializeField]
    private SpriteRenderer sp;

    private float timer;
    private float timerMax = .1f;

    private void Awake()
    {
        if (sp == null)
            sp = gameObject.GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = timerMax;
            sp.sortingOrder = (int) (sortingOrderBase - transform.position.y - offset);
            if (runOnce)
            {
                Destroy(this);
            }
        }
    }
}
