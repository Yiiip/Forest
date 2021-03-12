using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CodeContent : MonoBehaviour
{
    Text textCodeContent;
    Texture2D textureCodeContent;
    GameObject selections;
    int id;
    TinyGame_Coding gameRef;
    Button button;

    [SerializeField]
    GameObject selectButtonPrefab;
    int level; // for quering selections
    void Awake()
    {
        selections = transform.Find("Selections").gameObject;
        textCodeContent = transform.Find("Text").GetComponent<Text>();
        button = GetComponent<Button>();

    }
    void Start()
    {
        HideSelections();
    }

    public CodeContent Init(TinyGame_Coding refe, int id, int level)
    {
        gameRef = refe;
        this.id = id;
        this.level = level;
        button.onClick.AddListener(() => gameRef.OnAnswerSelect(id));

        //add selections buttons
        //and bind event OnButtonClicked 
        var contents = gameRef.ContentPools(level);
        for (int i = 0; i < contents.Length; i++)
        {
            int finalI = i;
            var go = Instantiate(selectButtonPrefab);
            go.transform.SetParent(selections.transform);
            go.transform.localPosition = Vector3.down * i * 30;
            go.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(finalI));
            go.transform.Find("Text").GetComponent<Text>().text = contents[i];
        }


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
        gameRef.OnAnswerSelected(id, gameRef.ContentPools(level)[i]);
        textCodeContent.text = gameRef.ContentPools(level)[i];
    }
}
