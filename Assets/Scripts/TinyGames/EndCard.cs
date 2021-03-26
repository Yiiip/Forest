using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndCard : MonoBehaviour
{
    [ObjectReference("Text")] Text text;
    [ObjectReference("Button")] Button button;

    private static EndCard instance;

    void Awake()
    {
        ObjectReferenceAttribute.GetReferences(this);
    }
    public static void ShowEndCardWithContent(string content, Transform parent)
    {
        instance = Instantiate(Resources.Load<EndCard>("Prefabs/UI/EndCard"));
        instance.text.text = content;
        instance.button.onClick.AddListener(() =>
        {
            GameManager.FromCityToForest = true;
            GameManager.LockTimer = false;
            FirstOpenGame.InTutorial = false;
            SceneManager.LoadScene("Forest", LoadSceneMode.Single);
        });
        instance.transform.SetParent(parent);
        instance.transform.localScale = Vector3.one;
        instance.transform.localPosition = Vector3.zero;
    }
}
