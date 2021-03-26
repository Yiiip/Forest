using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstOpenGame : MonoBehaviour
{
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private Image imgBg;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Text textIntro;

    private void Start()
    {
        animator.enabled = false;

        if (SaveData.current.tutorialPO.IsFinished(TutorialConst.Key_FirstOpen))
        {
            gameObject.SetActive(false);
            return;
        }

        imgBg.raycastTarget = true;
        imgBg.SetAlpha(1);
        StartCoroutine(nameof(OpenEye));
    }

    private IEnumerator OpenEye()
    {
        yield return new WaitForSeconds(0.5f);
        textIntro.TypeText("兔子：山灵大人！快醒醒！", 0.1f, null);
        yield return new WaitForSeconds(1f);

        var rabbit = GameManager.Instance.World.CharacterEntities.Find(i => i.staticData.m_id == CharacterPO.RabbitId);
        rabbit.transform.position = new Vector3(0, -8, rabbit.transform.position.z);

        animator.enabled = true;
        yield return new WaitForSeconds(0.5f);
        textIntro.text = string.Empty;
        yield return new WaitForSeconds(5.3f);
        imgBg.raycastTarget = false;
        UIManager.Instance.Show(typeof(DialogUI), new DialogUIIntent
        {
            dialogSheet = StaticDataManager.Instance.GetDialogSheet("New01"),
            onFinish = ()=>
            {
                StartCoroutine(nameof(ClickRabbit));
            }
        });
    }

    private IEnumerator ClickRabbit()
    {
        var tutorialUI = UIManager.Instance.GetUI<TutorialUI>();
        var rabbit = GameManager.Instance.World.CharacterEntities.Find(i => i.staticData.m_id == CharacterPO.RabbitId);
        tutorialUI.ShowHand(rabbit.transform.position, default, "点击兔子");
        yield return new WaitForSeconds(1f);
        tutorialUI.HideHand();
        yield return new WaitForSeconds(1f);

        UIManager.Instance.Show(typeof(DialogUI), new DialogUIIntent
        {
            dialogSheet = StaticDataManager.Instance.GetDialogSheet("New02"),
            onFinish = ()=>
            {
                StartCoroutine(nameof(End));
            }
        });
    }

    private IEnumerator End()
    {
        yield return null;
        SaveData.current.tutorialPO.Finish(TutorialConst.Key_FirstOpen);
        if (cameraController != null)
        {
            cameraController.SetNewSize(cameraController.zoomMaxSize);
        }
        gameObject.SetActive(false);
    }
}