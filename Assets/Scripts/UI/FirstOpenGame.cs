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
        UIManager.Instance.Show(typeof(DialogUI), new DialogUIIntent
        {
            dialogSheet = StaticDataManager.Instance.GetDialogSheet("New01"),
            onFinish = ()=>
            {
                SaveData.current.tutorialPO.Finish(TutorialConst.Key_FirstOpen);
                if (cameraController != null)
                {
                    cameraController.SetNewSize(cameraController.CurCamera.orthographicSize + 5);
                }
            }
        });
        gameObject.SetActive(false);
    }
}