using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CharacterEntity : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Collider2D entityCollider;
    private bool isMouseDown;
    private float mouseDownTimer;
    private bool canDrag;

    private CharacterPO characterPO;
    private sCharacterVO staticData;

    public void Init(CharacterPO characterPo)
    {
        this.characterPO = characterPo;
        this.staticData = StaticDataManager.Instance.GetCharacterVO(characterPO.staticDataId);
    }

    void Start()
    {
        var lis = EventTriggerListener.Get(this.gameObject);
        lis.onClick = OnClick;
        lis.onBeginDrag = OnBeginDrag;
        lis.onDrag = OnDrag;
        lis.onEndDrag = OnEndDrag;
        lis.onDown = OnMouseDown;
        lis.onUp = OnMouseUp;
        lis.onExit = OnMouseExit;
    }

    private void OnClick(GameObject go)
    {
        if (staticData != null)
        {
            Debug.Log(staticData.m_name);
        }
    }

    private void OnMouseDown(GameObject go)
    {
        Debug.Log("OnMouseDown");
        isMouseDown = true;
        CameraController.LockMovement = true;
        mouseDownTimer = 0.5f;
    }

    private void OnMouseUp(GameObject go)
    {
        // Debug.Log("OnMouseUp");
        isMouseDown = false;
    }

    private void OnMouseExit(GameObject go)
    {
        isMouseDown = false;
    }

    private void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("onBeginDrag");
    }

    private void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
        if (canDrag)
        {
            var newPos = Camera.main.ScreenToWorldPoint(eventData.position);
            gameObject.transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        }
    }

    private void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("OnEndDrag");
        canDrag = false;
        CameraController.LockMovement = false;
    }

    private void Update()
    {
        if (isMouseDown)
        {
            mouseDownTimer = Mathf.Max(0, mouseDownTimer - Time.deltaTime);
            if (mouseDownTimer <= 0)
            {
                isMouseDown = false;
                canDrag = true;
                gameObject.GetComponent<DOTweenAnimation>()?.DORestart();
            }
        }
    }
}