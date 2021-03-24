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
        // SceneManager.LoadScene("CityScene"); //TODO
    }

    public void LeaveForest()
    {
        animator.enabled = true;
        animator.Play("FishCardMove");
    }

    public void EnterForest()
    {
        animator.enabled = true;
        animator.Play("FishCardMove");
    }
}