using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeContentType = TinyGame_Coding_Helper.CodeContentType;
public class TinyGame_Coding : MonoBehaviour
{

    public Texture2D[] codeContentTextures;
    public string[] contentPools1;
    public string[] contentPools2;
    public string[] contentPools3;
    public string[] answers;
    public string[] requirements;
    public int[] paymentPerRequire;
    public GameObject codeContentPrefab;
    private CodeContent[] contents;
    // private CodeContent[] requires;
    private RectTransform codePivot;
    private RectTransform requirementsPivot;

    //current Setting
    private bool taskWillChange;
    private bool containsIf;
    public int taskChangeRate;
    public int level;
    [SerializeField] EndCard endCard;
    void Awake()
    {
        codePivot = transform.Find("CodePivot").GetComponent<RectTransform>();
        //load textures
        var contentTypes = System.Enum.GetNames(typeof(CodeContentType));
        codeContentTextures = new Texture2D[contentTypes.Length];
        for (int i = 0; i < contentTypes.Length; i++)
        {
            codeContentTextures[i] = Resources.Load<Texture2D>(contentTypes[i]);
        }
    }

    void Start()
    {
        AudioManager.Instance.PlayMusic(1002, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GenerateNewIssue(3, false, false, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GenerateNewIssue(4, false, true, 2, 30);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GenerateNewIssue(5, false, true, 3, 50);
        }
    }

    public string[] ContentPools(int index)
    {
        switch (index)
        {
            case 1: return contentPools1;
            case 2: return contentPools2;
            case 3: return contentPools3;
        }
        return null;
    }

    public void GenerateNewIssue(int codeLength, bool containsIf, bool taskWillChange, int level, int taskChangeRate = 0)
    {
        answers = new string[codeLength];
        this.taskWillChange = taskWillChange;
        this.containsIf = containsIf;
        this.taskChangeRate = taskChangeRate;
        this.level = level;

        //code
        if (contents != null)
            foreach (var go in contents)
            {
                Destroy(go.gameObject);
            }
        contents = new CodeContent[codeLength];
        for (int i = 0; i < codeLength; i++)
        {
            contents[i] = Instantiate(codeContentPrefab).GetComponent<CodeContent>().Init(this, i, level);
            contents[i].transform.SetParent(codePivot);
            contents[i].transform.localPosition = Vector3.down * i * 35;
            contents[i].transform.localScale = Vector3.one;
        }

        //require
        requirements = TinyGame_Coding_Helper.GenerateRequirement(codeLength, ContentPools(level));
        // foreach (var go in contents)
        // {
        //     Destroy(go);
        // }
        // contents = new CodeContent[codeLength];
        // for (int i = 0; i < codeLength; i++)
        // {
        //     contents[i] = Instantiate(codeContentPrefab).GetComponent<CodeContent>().Init(this, i, level);
        //     contents[i].transform.SetParent(codePivot);
        //     contents[i].transform.localPosition = Vector3.down * i * 35;
        // }
    }

    public void RequirementChange()
    {
        var t = ContentPools(level);
        requirements[Random.Range(0, requirements.Length - 1)] = t[Random.Range(0, t.Length - 1)];
    }

    public void OnAnswerSelect(int contentIndex)
    {
        foreach (var item in contents)
        {
            item.HideSelections();
        }
        contents[contentIndex].ShowSelections();
    }

    public void OnAnswerSelected(int contentIndex, string selectedContent)
    {
        // Debug.Log($"contentIndex = {contentIndex}");
        // Debug.Log($"selectionContent = {selectedContent}");
        contents[contentIndex].HideSelections();
        answers[contentIndex] = selectedContent;
        if (contentIndex < requirements.Length - 1)
        {
            contents[contentIndex + 1].ShowSelections();
        }

        if (taskWillChange)
        {
            if (taskChangeRate >= Random.Range(0, 100))
            {
                RequirementChange();
            }
        }
    }

    public void OnDoneButtonClick()
    {
        var a = TinyGame_Coding_Helper.CheckResult(requirements, answers);

        SaveData.current.playerProfile.coin += a * paymentPerRequire[level];

        //level up
        if (a == 5 && SaveData.current.smallGameLevelPO.Level_Coding < 3)
            SaveData.current.smallGameLevelPO.Level_Coding++;
        endCard.ShowEndCard(GenerearteResultString(a));
    }

    public string GenerearteResultString(int a)
    {
        switch (a)
        {
            case 5:
                return string.Format("今天的代码没什么问题，拿好上你可以走了");
            case 4:
                return string.Format("今天的代码里有{0}个bug，{1}块已经从你工资里扣掉了", a, (5 - a) * paymentPerRequire[level]);
            default:
                return string.Format("今天的代码里有{0}个bug，这水平还上啥班呢", a);
        }
    }

}

public static class TinyGame_Coding_Helper
{
    public enum CodeContentType
    {
        Circle = 0,
        Square = 1,
        Triangle = 2,
    }
    public static string[] GenerateRequirement(int codeLength, string[] contentPool)
    {
        var ret = new string[codeLength];

        for (int i = 0; i < codeLength; i++)
        {
            ret[i] = contentPool[Random.Range(0, contentPool.Length - 1)];
        }
        return ret;
    }

    public static int CheckResult(string[] requirements, string[] answers)
    {
        int correctAnswer = 0;
        for (int i = 0; i < requirements.Length; i++)
        {
            if (requirements[i] == answers[i])
            {
                correctAnswer++;
            }
        }
        return correctAnswer;
    }
}