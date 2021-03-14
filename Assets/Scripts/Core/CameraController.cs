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
    public float zoomAmount = 15f;
    [Header("缩放速度")]
    public float zoomSpd = 12f;
    [Header("缩放最近范围 (ZoomByPos)")]
    public float zoomNear = 50f;
    [Header("缩放最远范围 (ZoomByPos)")]
    public float zoomFar = -150f;
    [Header("缩放最大尺寸 (ZoomBySize)")]
    public float zoomMaxSize = 30f;
    [Header("缩放最小尺寸 (ZoomBySize)")]
    public float zoomMinSize = 15f;

    [Header("允许视口外操作")]
    [SerializeField]
    private bool enableOutOfViewport = true;

    [Header("移动范围")]
    [SerializeField]
    private SpriteRenderer rangeSp = null;

    [Header("相机")]
    [SerializeField]
    private Camera curCamera = null;

    private Vector3 newPos;
    private Vector3 dragStartPos;
    private Vector3 dragCurrentPos;
    private Vector3 newZoom;
    private float newSize;

    private float rangeMinX, rangeMaxX, rangeMinY, rangeMaxY;

    private bool mouseDownFlag;

    private void Awake()
    {
    }

    private void Start()
    {
        resetData();
    }

    private void resetData()
    {
        newPos = transform.position;
        newZoom = curCamera.transform.localPosition;
        newSize = curCamera.orthographicSize;
    }

    private void Update()
    {
        // Blocked by UI or Sprite
        // if (EventSystem.current.IsPointerOverGameObject())
        // {
        //     return;
        // }

        if (!enableOutOfViewport)
        {
            // Out of viewport
            var mouseViewportPos = curCamera.ScreenToViewportPoint(Input.mousePosition);
            if (mouseViewportPos.x > 1f || mouseViewportPos.x < 0f
                || mouseViewportPos.y > 1f || mouseViewportPos.y < 0f)
            {
                resetData();
                return;
            }
        }

        // Movement
        UpdateMovement();

        // Zoom
        if (curCamera.orthographic)
        {
            UpdateZoomBySize();
        }
        else
        {
            UpdateZoomByPos();
        }
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
            if (UIManager.Instance.IsPointerOverUI())
            {
                return;
            }
            mouseDownFlag = true;

            // method 2:
            // Plane plane = new Plane(Vector3.forward, Vector3.zero);
            // Ray ray = curCamera.ScreenPointToRay(Input.mousePosition);
            // float entry;
            // if (plane.Raycast(ray, out entry))
            // {
            //     dragStartPos = ray.GetPoint(entry);
            //     dragStartPos.z = newPos.z;
            // }

            // method 3:
            dragStartPos = curCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && mouseDownFlag)
        {
            // method 1:
            // var xAxisRaw = Input.GetAxisRaw("Mouse X");
            // var yAxisRaw = Input.GetAxisRaw("Mouse Y");
            // newPos += new Vector3(-xAxisRaw * moveSpd, -yAxisRaw * moveSpd, 0);

            // method 2:
            // Plane plane = new Plane(Vector3.forward, Vector3.zero);
            // Ray ray = curCamera.ScreenPointToRay(Input.mousePosition);
            // float entry;
            // if (plane.Raycast(ray, out entry))
            // {
            //     dragCurrentPos = ray.GetPoint(entry);
            //     dragCurrentPos.z = newPos.z;
            //     newPos = transform.position + dragStartPos - dragCurrentPos;
            // }

            // method 3:
            Vector3 diff = dragStartPos - curCamera.ScreenToWorldPoint(Input.mousePosition);
            newPos = transform.position + diff;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseDownFlag = false;
        }

        newPos = ClampPosRange(newPos);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * moveSpd);
    }

    private void UpdateZoomByPos()
    {
        if (UIManager.Instance.IsPointerOverUI())
        {
            return;
        }

        var scrollDeltaY = Input.mouseScrollDelta.y;
        if (scrollDeltaY != 0)
        {
            var newZoomTemp = newZoom + (Vector3.forward * zoomAmount) * scrollDeltaY;
            if (newZoomTemp.z >= zoomFar && newZoomTemp.z <= zoomNear)
            {
                newZoom = newZoomTemp;
            }
        }

        curCamera.transform.localPosition = Vector3.Lerp(curCamera.transform.localPosition, newZoom, Time.deltaTime * zoomSpd);
    }

    private void UpdateZoomBySize()
    {
        if (UIManager.Instance.IsPointerOverUI())
        {
            return;
        }

        var scrollDeltaY = Input.mouseScrollDelta.y;
        if (scrollDeltaY != 0)
        {
            var newSizeTemp = newSize + zoomAmount * -scrollDeltaY;
            newSize = Mathf.Clamp(newSizeTemp, zoomMinSize, zoomMaxSize);
        }

        curCamera.orthographicSize = Mathf.Lerp(curCamera.orthographicSize, newSize, Time.deltaTime * zoomSpd);
    }

    private Vector3 ClampPosRange(Vector3 input)
    {
        if (rangeSp == null) return input;

        float camHeight = curCamera.orthographicSize;
        float camWidth = curCamera.orthographicSize * curCamera.aspect;

        float rangeMinX = rangeSp.transform.position.x - rangeSp.bounds.size.x * 0.5f;
        float rangeMaxX = rangeSp.transform.position.x + rangeSp.bounds.size.x * 0.5f;
        float rangeMinY = rangeSp.transform.position.y - rangeSp.bounds.size.y * 0.5f;
        float rangeMaxY = rangeSp.transform.position.y + rangeSp.bounds.size.y * 0.5f;

#if UNITY_EDITOR
        Debug.DrawLine(new Vector3(rangeMinX, rangeMinY, transform.position.z), new Vector3(rangeMinX, rangeMaxY, transform.position.z), Color.magenta);
        Debug.DrawLine(new Vector3(rangeMinX, rangeMaxY, transform.position.z), new Vector3(rangeMaxX, rangeMaxY, transform.position.z), Color.magenta);
        Debug.DrawLine(new Vector3(rangeMaxX, rangeMaxY, transform.position.z), new Vector3(rangeMaxX, rangeMinY, transform.position.z), Color.magenta);
        Debug.DrawLine(new Vector3(rangeMaxX, rangeMinY, transform.position.z), new Vector3(rangeMinX, rangeMinY, transform.position.z), Color.magenta);
#endif

        float minX = rangeMinX + camWidth;
        float maxX = rangeMaxX - camWidth;
        float minY = rangeMinY + camHeight;
        float maxY = rangeMaxY - camHeight;

        float newX = Mathf.Clamp(input.x, minX, maxX);
        float newY = Mathf.Clamp(input.y, minY, maxY);

        return new Vector3(newX, newY, transform.position.z);
    }
}