using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TrainEntity : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer body1;
    [SerializeField]
    private SpriteRenderer body2;
    [SerializeField]
    private SpriteRenderer leg1;
    [SerializeField]
    private SpriteRenderer leg2;
    [SerializeField]
    private Collider2D entityCollider;
    [SerializeField]
    public Animator animator;

    [SerializeField]
    public Vector2 OriginPos; //new Vector2(2.3f, -55.6f);
    [SerializeField]
    public Vector2 OutsidePos; //new Vector2(-256f, -55.6f);

    private bool isLeave = false;
    private bool isIn = false;

    void Start()
    {
        var lis = EventTriggerListener.Get(this.gameObject);
        lis.onClick = OnClick;
        isLeave = false;
        isIn = false;
        FishStyle();
    }

    public void TrainStyle()
    {
        body1.gameObject.SetActive(false);
        leg1.gameObject.SetActive(false);
        leg2.gameObject.SetActive(false);
        body2.gameObject.SetActive(true);
    }

    public void FishStyle()
    {
        body1.gameObject.SetActive(true);
        leg1.gameObject.SetActive(true);
        leg2.gameObject.SetActive(true);
        body2.gameObject.SetActive(false);
    }

    private void OnClick(GameObject go)
    {
        if (GameManager.Instance.World.GetTodayPercent() >= World.NightPercent)
        {
            ToastUI.Toast("白天才可以乘坐鱼车哦");
            return;
        }
        LeaveForest();
    }

    public void LeaveForest()
    {
        StopCoroutine(nameof(IELeaveForest));
        StartCoroutine(nameof(IELeaveForest));
    }

    private IEnumerator IELeaveForest()
    {
        isLeave = true;
        GameManager.LockTimer = true;

        CameraController.followTarget = gameObject.transform;
        animator.enabled = true;
        animator.SetTrigger("Start");
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        var duraion = 5f;
        var twPos = TweenPosition.Begin(gameObject, duraion, OutsidePos).From(OriginPos);
        twPos.ResetToBeginning();
        twPos.PlayForward();
        yield return new WaitForSeconds(duraion * 0.7f);
        var magicMask = UIManager.Instance.GetUI<MagicMaskUI>().magicMask;
        magicMask.SetTarget(gameObject.transform).Focus(fromRadius: Screen.width, toRadius: 5, duration: 1f, onFinish: () =>
        {
            SceneManager.LoadScene("CityScene");
            // CameraController.followTarget = null;
            magicMask.RemoveTarget();
            // magicMask.Disable();
            isLeave = false;
        });
    }

    public void EnterForest()
    {
        StopCoroutine(nameof(IEEnterForest));
        StartCoroutine(nameof(IEEnterForest));
    }

    public IEnumerator IEEnterForest()
    {
        isIn = true;

        CameraController.followTarget = gameObject.transform;
        gameObject.transform.position = OutsidePos;
        animator.enabled = true;
        animator.SetTrigger("Start");
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        yield return null;
        var duraion = 5f;
        var twPos = TweenPosition.Begin(gameObject, duraion, OriginPos).From(OutsidePos);
        twPos.ResetToBeginning();
        twPos.PlayForward();

        var magicMask = UIManager.Instance.GetUI<MagicMaskUI>().magicMask;
        magicMask.SetTarget(gameObject.transform).Focus(fromRadius: 5, toRadius: Screen.width, duration: 1f, onFinish: () =>
        {
            magicMask.RemoveTarget();
            magicMask.Disable();
        });

        yield return new WaitForSeconds(duraion);
        CameraController.followTarget = null;
    }

    private void Update()
    {
        if (isLeave)
        {
            if (transform.position.x <= -200f)
            {
                TrainStyle();
            }
            else
            {
                FishStyle();
            }
        }
        else if (isIn)
        {
            if (transform.position.x <= -200f)
            {
                TrainStyle();
            }
            else
            {
                FishStyle();
            }
        }
    }
}