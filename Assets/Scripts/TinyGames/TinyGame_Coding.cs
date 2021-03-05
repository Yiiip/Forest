using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeContentType = TinyGame_Coding_Helper.CodeContentType;
public class TinyGame_Coding : MonoBehaviour
{
    
    public Texture2D[] codeContentTextures;
    public CodeContentType[] answers;
    public CodeContentType[] requirements;
    public GameObject codeContentPrefab;
    private CodeContent[] contents;
    void Awake()
    {
        var contentTypes = System.Enum.GetNames(typeof(CodeContentType));
        codeContentTextures = new Texture2D[contentTypes.Length];
        for (int i = 0; i < contentTypes.Length; i++)
        {
            codeContentTextures[i] = Resources.Load<Texture2D>(contentTypes[i]);
        }
    }

    void Start()
    {  
        GenerateNewIssue(4, false, false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateNewIssue(int codeLength, bool containsIf, bool taskWillChange)
    {
        answers = new CodeContentType[codeLength];
        for (int i = 0; i < codeLength; i++)
        {
            answers[i] = (CodeContentType)(-1);
        }
        requirements = TinyGame_Coding_Helper.GenerateRequirement(codeLength);
        contents = new CodeContent[codeLength];
        for (int i = 0; i < codeLength; i++)
        {
            contents[i] = Instantiate(codeContentPrefab).GetComponent<CodeContent>().Init(this, i);
            contents[i].transform.SetParent(transform);
            contents[i].transform.position += Vector3.right * i * 100;
        }
    }

    public void OnAnswerSelect(int contentIndex)
    {
        foreach (var item in contents)
        {
            item.HideSelections();
        }
        contents[contentIndex].ShowSelections();
    }

    public void OnAnswerSelected(int contentIndex, int selectionIndex)
    {
        Debug.Log($"contentIndex = {contentIndex}");
        Debug.Log($"selectionIndex = {selectionIndex}");
        contents[contentIndex].HideSelections();
        if (contentIndex < requirements.Length - 1)
        {
            contents[contentIndex + 1].ShowSelections();
        }

    }

    public void OnDoneButtonClick()
    {
        TinyGame_Coding_Helper.CheckResult(requirements, answers);
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
    public static CodeContentType[] GenerateRequirement(int codeLength)
    {
        var ret = new CodeContentType[codeLength];
        
        for (int i = 0; i < codeLength; i++)
        {
            ret[i] = (CodeContentType)Random.Range(0, codeLength - 1);
        }
        return ret;
    }

    public static int CheckResult(CodeContentType[] requirements, CodeContentType[] answers)
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