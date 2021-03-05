using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CodeContent : MonoBehaviour
{
    Texture2D textureCodeContent;
    GameObject selections;
    int id;
    TinyGame_Coding gameRef;
    Button button;
    void Awake()
    {
        selections = transform.Find("Selections").gameObject;
        button = GetComponent<Button>();
    }
    void Start()
    {
        HideSelections();
    }

    public CodeContent Init(TinyGame_Coding refe, int id)
    {
        this.id = id;
        gameRef = refe;
        button.onClick.AddListener(() => gameRef.OnAnswerSelect(id));
        return this;
    }

    public void ShowSelections()
    {
        selections.SetActive(true);
    }

    public void HideSelections()
    {
        selections.SetActive(false);
    }

    public void OnButtonClicked(int i)
    {
        gameRef.OnAnswerSelected(id, i);
    }
}
