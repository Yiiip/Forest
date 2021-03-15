using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstOpenGame : MonoBehaviour
{
    [SerializeField]
    private Image imgBg;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Text textIntro;

    private void Start()
    {
        animator.enabled = false;
        imgBg.SetAlpha(1);
        StartCoroutine(nameof(OpenEye));
    }

    private IEnumerator OpenEye()
    {
        yield return new WaitForSeconds(0.5f);
        textIntro.TypeText("兔子：山灵大人！快醒醒！", 0.1f, null);
        yield return new WaitForSeconds(1f);
        animator.enabled = true;
        yield return new WaitForSeconds(0.5f);
        textIntro.text = string.Empty;
        yield return new WaitForSeconds(5.3f);
        UIManager.Instance.Show(typeof(DialogUI), GameManager.Instance.GetDialogSheet("New01"));
        gameObject.SetActive(false);
    }
}