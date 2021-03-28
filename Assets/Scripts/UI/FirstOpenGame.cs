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

    public static bool InTutorial = false;

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
        InTutorial = true;

        var B1 = GameManager.Instance.World.BuildingEntities.Find(i => i.staticDataId == "B1");
        B1.spriteRenderer.color = new Color(1,1,1,0);

        var rabbit = GameManager.Instance.World.CharacterEntities.Find(i => i.staticData.m_id == CharacterPO.RabbitId);
        rabbit.transform.position = new Vector3(0, -8, rabbit.transform.position.z);
        rabbit.moveDir = eDirection.Down;

        yield return new WaitForSeconds(0.5f);
        textIntro.TypeText("兔子：山灵大人！快醒醒！", 0.1f, null);
        yield return new WaitForSeconds(1f);

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
        tutorialUI.ShowHand(rabbit.transform, default, "点击兔子");
        yield return new WaitForSeconds(1f);
        tutorialUI.HideHand();
        yield return new WaitForSeconds(1f);

        UIManager.Instance.Show(typeof(DialogUI), new DialogUIIntent
        {
            dialogSheet = StaticDataManager.Instance.GetDialogSheet("New02"),
            onFinish = ()=>
            {
                StartCoroutine(nameof(DisplayB1));
            }
        });
    }

    private IEnumerator DisplayB1()
    {
        var B1 = GameManager.Instance.World.BuildingEntities.Find(i => i.staticDataId == "B1");

        float p = 0;
        float duration = 1f;
        float spd = 1f / duration;
        while (p < 1)
        {
            p += Time.deltaTime * spd;
            B1.spriteRenderer.color = new Color(1,1,1,p);
            yield return null;
        }
        B1.spriteRenderer.color = new Color(1,1,1,1);


        UIManager.Instance.Show(typeof(DialogUI), new DialogUIIntent
        {
            dialogSheet = StaticDataManager.Instance.GetDialogSheet("New03"),
            onFinish = ()=>
            {
                StartCoroutine(nameof(OpenB1));
            }
        });
    }

    private IEnumerator OpenB1()
    {
        var B1 = GameManager.Instance.World.BuildingEntities.Find(i => i.staticDataId == "B1");

        yield return new WaitForSeconds(0.2f);
        B1.spriteRenderer.sprite = Resources.Load<Sprite>("Images/Building/B1_2"); //open
        yield return new WaitForSeconds(0.2f);
        B1.spriteRenderer.sprite = Resources.Load<Sprite>("Images/Building/B1_1"); //close
        yield return new WaitForSeconds(0.2f);
        B1.spriteRenderer.sprite = Resources.Load<Sprite>("Images/Building/B1_2"); //open
        yield return new WaitForSeconds(0.2f);
        B1.spriteRenderer.sprite = Resources.Load<Sprite>("Images/Building/B1_1"); //close
        yield return new WaitForSeconds(0.2f);

        UIManager.Instance.Show(typeof(DialogUI), new DialogUIIntent
        {
            dialogSheet = StaticDataManager.Instance.GetDialogSheet("New04"),
            onFinish = ()=>
            {
                StartCoroutine(nameof(RabbitChange));
            }
        });
    }

    private IEnumerator RabbitChange()
    {
        var rabbit = GameManager.Instance.World.CharacterEntities.Find(i => i.staticData.m_id == CharacterPO.RabbitId);

        yield return new WaitForSeconds(0.4f);
        rabbit.spriteRenderer.sprite = rabbit.animanSprites[(int) eDirection.Down];
        yield return new WaitForSeconds(0.4f);
        rabbit.spriteRenderer.sprite = rabbit.animalSprites[(int) eDirection.Down];
        yield return new WaitForSeconds(0.4f);
        rabbit.spriteRenderer.sprite = rabbit.animanSprites[(int) eDirection.Down];
        yield return new WaitForSeconds(0.4f);
        rabbit.spriteRenderer.sprite = rabbit.animalSprites[(int) eDirection.Down];
        yield return new WaitForSeconds(0.2f);

        UIManager.Instance.Show(typeof(DialogUI), new DialogUIIntent
        {
            dialogSheet = StaticDataManager.Instance.GetDialogSheet("New05"),
            onFinish = ()=>
            {
                StartCoroutine(nameof(TrainEnter));
            }
        });
    }

    private IEnumerator TrainEnter()
    {
        var train = GameManager.Instance.World.TrainEntity;
        train.TrainStyle();
        train.EnterForest();

        var rabbit = GameManager.Instance.World.CharacterEntities.Find(i => i.staticData.m_id == CharacterPO.RabbitId);
        rabbit.transform.position = new Vector3(0f, -46f, rabbit.transform.position.z);

        yield return new WaitForSeconds(5f);
        train.animator.enabled = false;

        UIManager.Instance.Show(typeof(DialogUI), new DialogUIIntent
        {
            dialogSheet = StaticDataManager.Instance.GetDialogSheet("New06"),
            onFinish = ()=>
            {
                StartCoroutine(nameof(TrainGoOut));
            }
        });
    }

    private IEnumerator TrainGoOut()
    {
        var rabbit = GameManager.Instance.World.CharacterEntities.Find(i => i.staticData.m_id == CharacterPO.RabbitId);
        rabbit.transform.position = new Vector3(0f, 0f, rabbit.transform.position.z);

        var train = GameManager.Instance.World.TrainEntity;
        train.LeaveForest();
        yield return new WaitForSeconds(4f);
        StartCoroutine(nameof(End));
    }

    private IEnumerator End()
    {
        InTutorial = false;
        SaveData.current.tutorialPO.Finish(TutorialConst.Key_FirstOpen);
        yield return null;
        // if (cameraController != null)
        // {
        //     cameraController.SetNewSize(cameraController.zoomMaxSize);
        // }
        gameObject.SetActive(false);
    }
}