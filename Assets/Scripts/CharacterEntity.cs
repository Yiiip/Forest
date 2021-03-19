using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public enum eDirection
{
    Left,
    Right,
    Up,
    Down,
}

public enum eMoveState
{
    Idle,
    MovingRandom,
}

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

        moveState = (eMoveState) UnityEngine.Random.Range(0, 2);
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
        // Debug.Log("OnMouseDown");
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
        StupidAI();
    }

    private eMoveState moveState = 0;
    private float state0Timer = 0f;
    private float state1Timer = 0f;
    private float state1Duration = 3f;
    private eDirection state1Dir = 0;

    private void StupidAI()
    {
        if (canDrag)
        {
            return;
        }
        //TODO state machine
        switch (moveState)
        {
            case eMoveState.MovingRandom: //走路
                state1Timer += Time.deltaTime;
                if (state1Timer > state1Duration)
                {
                    state1Timer = 0f;
                    moveState = eMoveState.Idle;
                }
                else
                {
                    var vec = new Vector3(-1, 0, 0);
                    if (state1Dir == eDirection.Right) vec = new Vector3(1, 0, 0);
                    if (state1Dir == eDirection.Up) vec = new Vector3(0, 1, 0);
                    if (state1Dir == eDirection.Down) vec = new Vector3(0, -1, 0);
                    transform.Translate(vec * Time.deltaTime * 3);
                }
                break;

            case eMoveState.Idle: //停留
                state0Timer += Time.deltaTime;
                if (state0Timer > 1.5f)
                {
                    state0Timer = 0f;
                    moveState = eMoveState.MovingRandom;
                    state1Dir = (eDirection) (((int) state1Dir + 1) % 4);
                    state1Duration = UnityEngine.Random.Range(1f, 4f);
                }
                break;
        }
    }
}