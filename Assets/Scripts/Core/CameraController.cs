using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Header("移动量")]
    public float moveAmount = 1f;
    [Header("移动速度")]
    public float moveSpd = 10f;

    [Header("缩放量")]
    public float zoomAmount = 5f;
    [Header("缩放速度")]
    public float zoomSpd = 10f;
    [Header("缩放最近范围+")]
    public float zoomFar = 50f;
    [Header("缩放最远范围-")]
    public float zoomNear = -50f;

    [Header("相机")]
    public Transform cameraTransform;

    private Vector3 newPos;
    private Vector3 dragStartPos;
    private Vector3 dragCurrentPos;
    private Vector3 newZoom;

    private void Awake()
    {
    }

    private void Start()
    {
        newPos = transform.position;
        newZoom = cameraTransform.localPosition;
    }

    private void Update()
    {
        // if (EventSystem.current.IsPointerOverGameObject())
        // {
        //     return;
        // }

        // var scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        // if (scrollWheel != 0)
        // {
        //     var newSize = _camera.orthographicSize - Mathf.Sign(scrollWheel) * sizeSpd;
        //     if (newSize < sizeFar && newSize > sizeNear)
        //     {
        //         _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, newSize, Time.deltaTime * 10);
        //     }
        // }

        // if (Input.GetMouseButton(0))
        // {
        //     var xAxisRaw = Input.GetAxisRaw("Mouse X");
        //     var yAxisRaw = Input.GetAxisRaw("Mouse Y");
        //     var move = new Vector3(-xAxisRaw * moveSpd * Time.deltaTime, -yAxisRaw * moveSpd * Time.deltaTime, 0);
        //     var end = transform.position + move;
        //     transform.position = Vector3.Lerp(transform.position, end, Time.deltaTime * moveSpd);
        // }

        UpdateMovement();
        UpdateZoom();
    }

    private void UpdateMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPos += Vector3.up * moveAmount;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPos += Vector3.up * -moveAmount;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPos += Vector3.right * -moveAmount;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPos += Vector3.right * moveAmount;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.forward, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragStartPos = ray.GetPoint(entry);
                dragStartPos.z = newPos.z;
            }
        }
        if (Input.GetMouseButton(0))
        {
            // other method:
            // var xAxisRaw = Input.GetAxisRaw("Mouse X");
            // var yAxisRaw = Input.GetAxisRaw("Mouse Y");
            // newPos += new Vector3(-xAxisRaw * moveSpd, -yAxisRaw * moveSpd, 0);

            Plane plane = new Plane(Vector3.forward, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPos = ray.GetPoint(entry);
                dragCurrentPos.z = newPos.z;
                newPos = transform.position + dragStartPos - dragCurrentPos;
            }
        }

        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * moveSpd);
    }

    private void UpdateZoom()
    {
        var scrollDeltaY = Input.mouseScrollDelta.y;
        if (scrollDeltaY != 0)
        {
            var newZoomTemp = newZoom + (Vector3.forward * zoomAmount) * scrollDeltaY;
            if (newZoomTemp.z > zoomNear && newZoomTemp.z < zoomFar)
            {
                newZoom = newZoomTemp;
            }
        }

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * zoomSpd);
    }
}