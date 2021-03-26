using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TrainEntity : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Collider2D entityCollider;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    public Vector2 OriginPos; //new Vector2(2.3f, -55.6f);
    [SerializeField]
    public Vector2 OutsidePos; //new Vector2(-256f, -55.6f);

    void Start()
    {
        var lis = EventTriggerListener.Get(this.gameObject);
        lis.onClick = OnClick;
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
        GameManager.LockTimer = true;

        CameraController.followTarget = gameObject.transform;
        animator.enabled = true;
        animator.CrossFade("Idle", 0.1f);
        var duraion = 5f;
        var twPos = TweenPosition.Begin(gameObject, duraion, OutsidePos).From(OriginPos);
        twPos.ResetToBeginning();
        twPos.PlayForward();
        yield return new WaitForSeconds(duraion * 0.8f);
        var magicMask = UIManager.Instance.GetUI<MagicMaskUI>().magicMask;
        magicMask.SetTarget(gameObject.transform).Focus(fromRadius: Screen.width, toRadius: 5, duration: 1f, onFinish: () =>
        {
            SceneManager.LoadScene("CityScene");
            // CameraController.followTarget = null;
            magicMask.RemoveTarget();
            magicMask.Disable();
        });
    }

    public void EnterForest()
    {
        animator.enabled = true;
        animator.Play("Move");
    }
}