using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControll : MonoBehaviour
{
    [Header("视野初始值")]
    public float sizeInit = 16f;
    [Header("视野缩放最远")]
    public float sizeMin = 20f;
    [Header("视野缩放最近")]
    public float sizeMax = 10f;
    [Header("视野缩放的速率")]
    public float sizeSpd = 50f;
    [Header("摄像机移动的速率")]
    public float moveSpd = 50f;

    private Camera _camera;
    private Vector2 _downPos;

    private void Awake()
    {
        _camera = gameObject.GetComponent<Camera>();
        _camera.orthographicSize = sizeInit;
    }

    void Start()
    {
    }

    void Update()
    {
        var scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel != 0)
        {
            var newSize = _camera.orthographicSize - Mathf.Sign(scrollWheel) * Time.deltaTime * sizeSpd;
            if (newSize < sizeMin && newSize > sizeMax)
            {
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, newSize, Time.deltaTime * sizeSpd);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            _downPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            var xAxisRaw = Input.GetAxisRaw("Mouse X");
            var yAxisRaw = Input.GetAxisRaw("Mouse Y");
            var move = new Vector3(-xAxisRaw * moveSpd * Time.deltaTime, -yAxisRaw * moveSpd * Time.deltaTime, 0);
            var end = transform.position + move;
            transform.position = Vector3.Lerp(transform.position, end, Time.deltaTime * moveSpd);
        }
    }
}