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
    Stop = 0,
    Idle,
    MoveRandom,
    MoveToTarget,
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

    private eMoveState moveState;
    private float state0Timer = 0f;
    private float state1Timer = 0f;
    private float state1Duration = 3f;
    private eDirection state1Dir = 0;
    private Transform moveTarget;
    private float targetDistance = 10f;

    public CharacterPO characterPO { get; private set; }
    public sCharacterVO staticData { get; private set; }

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

        moveState = UnityEngine.Random.value > 0.5 ? eMoveState.Idle : eMoveState.MoveRandom;
    }

    private void OnClick(GameObject go)
    {
        if (staticData != null)
        {
            Debug.Log(staticData.m_name);

            // CameraController.followTarget = gameObject.transform;

            // var magicMaskUI = UIManager.Instance.GetUI<MagicMaskUI>();
            // var magicMask = magicMaskUI.magicMask;
            // magicMask.SetTarget(transform).Focus(fromRadius: Screen.width, toRadius: 0f, duration: 1f, onFinish: () =>
            // {
            // });
        }
    }

    private void OnMouseDown(GameObject go)
    {
        // Debug.Log("OnMouseDown");
        isMouseDown = true;
        canDrag = false;
        mouseDownTimer = 0.5f;
        CameraController.LockMovement = true;
    }

    private void OnMouseUp(GameObject go)
    {
        // Debug.Log("OnMouseUp");
        isMouseDown = false;
        CameraController.LockMovement = false;

        Collider2D[] r = new Collider2D[10];
        var contactFilter2D = new ContactFilter2D();
        contactFilter2D.useTriggers = true;
        entityCollider.OverlapCollider(contactFilter2D, r);
        if (r != null)
        {
            for (int i = 0; i < r.Length; i++)
            {
                if (r[i] != null)
                {
                    var buildingEntity = r[i].transform.parent.GetComponent<BuildingEntity>();
                    if (buildingEntity != null && buildingEntity.staticDataId == "B1")
                    {
                        Debug.Log($"up at {buildingEntity.gameObject.name}");
                        gameObject.transform.position = new Vector3(buildingEntity.transform.position.x + UnityEngine.Random.Range(-10f, 10f), buildingEntity.transform.position.y - 11f, transform.position.z);
                        gameObject.GetComponent<DOTweenAnimation>()?.DORestart();
                        StateToIdle();
                    }
                }
            }
        }
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

    public void MoveToTarget(Transform target, float targetDistance = -1f)
    {
        moveTarget = target;
        moveState = eMoveState.MoveToTarget;
        if (targetDistance >= 0f)
        {
            this.targetDistance = targetDistance;
        }
    }

    private void StateToIdle()
    {
        state0Timer = 0f;
        moveState = eMoveState.Idle;
    }

    private void StateToStop()
    {
        state0Timer = 0f;
        moveState = eMoveState.Stop;
    }

    private void StupidAI()
    {
        if (canDrag)
        {
            return;
        }
        //TODO state machine
        switch (moveState)
        {
            case eMoveState.MoveRandom: //走路
                state1Timer += Time.deltaTime;
                if (state1Timer > state1Duration)
                {
                    state1Timer = 0f;
                    StateToIdle();
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
                    moveState = eMoveState.MoveRandom;
                    state1Dir = (eDirection) (((int) state1Dir + 1) % 4);
                    state1Duration = UnityEngine.Random.Range(1f, 4f);
                }
                break;

            case eMoveState.MoveToTarget:
                if (Vector2.Distance(transform.position, moveTarget.position) <= targetDistance)
                {
                    moveState = eMoveState.Idle;
                    moveTarget = null;
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, moveTarget.position, Time.deltaTime * 1.5f);
                }
                break;

            case eMoveState.Stop:
                break;
        }
    }
}