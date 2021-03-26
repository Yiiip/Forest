using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public enum eDirection
{
    Left = 0,
    Right = 1,
    Up = 2,
    Down = 3,
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
    public SpriteRenderer spriteRenderer;
    [SerializeField]
    public Collider2D entityCollider;
    [SerializeField]
    public Animator animator;
    [SerializeField]
    public List<Sprite> animalSprites;
    [SerializeField]
    public List<Sprite> animanSprites;

    private bool isMouseDown;
    private float mouseDownTimer;
    private bool canDrag;

    private eMoveState moveState;
    private float idleTimer = 0f;
    private float moveTimer = 0f;
    private float moveDuration = 3f;
    private eDirection moveDir = 0;
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
                        buildingEntity.spriteRenderer.sprite = Resources.Load<Sprite>("Images/Building/B1_1");
                        gameObject.transform.position = new Vector3(buildingEntity.transform.position.x + UnityEngine.Random.Range(-15f, -5f), buildingEntity.transform.position.y - 23f, transform.position.z);
                        gameObject.GetComponent<DOTweenAnimation>()?.DORestart();
                        StateToIdle();

                        if (SaveData.current.playerProfile.water >= staticData.m_water)
                        {
                            //扣水变身
                            SaveData.current.playerProfile.water -= staticData.m_water;
                            characterPO.avator = eCharacterAvator.Animal;
                        }
                        else
                        {
                            ToastUI.Toast($"需要{staticData.m_water}个水才能变身！");
                        }
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

            Collider2D[] r = new Collider2D[2];
            var contactFilter2D = new ContactFilter2D();
            contactFilter2D.useTriggers = true;
            entityCollider.OverlapCollider(contactFilter2D, r);

            BuildingEntity B1 = null;
            bool collideB1 = false;
            foreach (var building in GameManager.Instance.World.BuildingEntities)
            {
                if (building.staticDataId == "B1")
                {
                    B1 = building;
                    break;
                }
            }
            if (r != null && r.Length > 0)
            {
                for (int i = 0; i < r.Length; i++)
                {
                    if (r[i] != null)
                    {
                        foreach (var building in GameManager.Instance.World.BuildingEntities)
                        {
                            if (building.staticDataId == "B1" && r[i] == building.entityCollider)
                            {
                                collideB1 = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (B1 != null)
                B1.spriteRenderer.sprite = Resources.Load<Sprite>(collideB1 ? "Images/Building/B1_2" : "Images/Building/B1_1");
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
                StateToIdle();
                gameObject.GetComponent<DOTweenAnimation>()?.DORestart();
            }
        }
        StupidAI();
        RefreshAvator();
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
        idleTimer = 0f;
        moveState = eMoveState.Idle;
    }

    private void StateToStop()
    {
        idleTimer = 0f;
        moveState = eMoveState.Stop;
    }

    private void StupidAI()
    {
        if (FirstOpenGame.InTutorial && staticData.m_id == CharacterPO.RabbitId)
        {
            return;
        }
        if (canDrag)
        {
            return;
        }
        switch (moveState)
        {
            case eMoveState.MoveRandom: //走路
                moveTimer += Time.deltaTime;
                if (moveTimer > moveDuration)
                {
                    moveTimer = 0f;
                    StateToIdle();
                }
                else
                {
                    var vec = new Vector3(-1, 0, 0);
                    if (moveDir == eDirection.Right) vec = new Vector3(1, 0, 0);
                    if (moveDir == eDirection.Up) vec = new Vector3(0, 1, 0);
                    if (moveDir == eDirection.Down) vec = new Vector3(0, -1, 0);
                    transform.Translate(vec * Time.deltaTime * 3);
                }
                break;

            case eMoveState.Idle: //停留
                idleTimer += Time.deltaTime;
                if (idleTimer > 1.5f)
                {
                    idleTimer = 0f;
                    moveState = eMoveState.MoveRandom;
                    moveDir = (eDirection) (((int) moveDir + 1) % 4);
                    moveDuration = UnityEngine.Random.Range(1f, 4f);
                }
                break;

            case eMoveState.MoveToTarget:
                if (Vector2.Distance(transform.position, moveTarget.position) <= targetDistance)
                {
                    StateToIdle();
                    moveTarget = null;
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, moveTarget.position, Time.deltaTime * 1.5f);
                    var n = (moveTarget.position - transform.position).normalized;
                    if (Mathf.Abs(n.x) > Mathf.Abs(n.y))
                    {
                        moveDir = n.x > 0 ? eDirection.Right : eDirection.Left;
                    }
                    else
                    {
                        moveDir = n.y > 0 ? eDirection.Up : eDirection.Down;
                    }
                }
                break;

            case eMoveState.Stop:
                moveDir = eDirection.Down;
                break;
        }
    }

    private void RefreshAvator()
    {
        if (FirstOpenGame.InTutorial && staticData.m_id == CharacterPO.RabbitId)
        {
            return;
        }
        List<Sprite> sprites = characterPO.avator == eCharacterAvator.Animal ? animalSprites : animanSprites;
        int index = (int) moveDir;
        if (sprites.IsIndexValid(index))
        {
            spriteRenderer.sprite = sprites[index];
        }
    }
}