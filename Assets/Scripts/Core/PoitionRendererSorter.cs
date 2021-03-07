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

    private Renderer _renderer;
    private float timer;
    private float timerMax = .1f;

    private void Awake()
    {
        _renderer = gameObject.GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = timerMax;
            _renderer.sortingOrder = (int) (sortingOrderBase - transform.position.y - offset);
            if (runOnce)
            {
                Destroy(this);
            }
        }
    }
}
