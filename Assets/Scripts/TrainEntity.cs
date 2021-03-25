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
        StartCoroutine(nameof(LeaveForest));
    }

    public IEnumerator LeaveForest()
    {
        CameraController.followTarget = gameObject.transform;
        animator.enabled = true;
        animator.CrossFade("FishCardMove", 0.1f);
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
        animator.Play("FishCardMove");
    }
}