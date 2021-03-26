using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
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
    Tween tween;
    int level; // for quering selections
    void Awake()
    {
        selections = transform.Find("Selections").gameObject;
        textCodeContent = transform.Find("Text").GetComponent<Text>();
        button = GetComponent<Button>();
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
        textCodeContent.enabled = true;
        if (textCodeContent.text == "<color=#ECECEC>|</color>")
            tween = DOVirtual.DelayedCall(1f, () => textCodeContent.enabled = !textCodeContent.enabled).SetLoops(-1);
    }

    public void HideSelections()
    {
        selections.SetActive(false);
        if (tween != null) tween.Kill();
        if (textCodeContent.text == "<color=#ECECEC>|</color>")
            textCodeContent.enabled = false;
    }

    public void OnButtonClicked(int i)
    {
        gameRef.OnAnswerSelected(id, gameRef.ContentPools(level)[i]);
        textCodeContent.text = gameRef.ContentPools(level)[i] + "();";
        textCodeContent.enabled = true;
    }

    void OnDestroy()
    {
        if (tween != null) tween.Kill();
        tween = null;
    }
}
