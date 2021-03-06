using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using DG.Tweening;
public partial class TinyGame_Stocks : MonoBehaviour
{
    [ObjectReference("Cach_Text")] Text cach_text;
    [ObjectReference("Stock_Text")] Text holding_text;
    [ObjectReference("Yestoday_Earned")] Text Yestoday_Earned;
    [ObjectReference("Earned_Text")] Text Earned_Text;
    [ObjectReference("Control/AmoutToDeal")] Text amountToDeal_text;
    [ObjectReference("Blocks/MovingParent")] RectTransform movingParent;

    [ObjectReference("Control/BuyButton")] Button buy_button;
    [ObjectReference("Control/SellButton")] Button sell_button;
    [ObjectReference("Control/ConfirmButton")] Button confirm_button;
    List<StockBlock> blocks;
    StockBlock blockPrefab;
    public int totolDealPoints = 10;
    public int initCash = 1000;

    int intervalDistance = 50;
    public int intervalTime = 3;
    [ObjectReference("Title")] Text titleText;

    void Awake()
    {
        ObjectReferenceAttribute.GetReferences(this);
        blockPrefab = Resources.Load<StockBlock>("Prefabs/TinyGame_Stocks/Block");
    }

    public TinyGame_Stocks InitUI()
    {
        buy_button.onClick.AddListener(BuyButtonClicked);
        sell_button.onClick.AddListener(SellButtonClicked);
        confirm_button.onClick.AddListener(ConfirmButtonClicked);

        blocks = new List<StockBlock>();
        int i = 0;
        int left = 10;
        int right = 10;
        for (; i < left; i++)
        {
            var go = Instantiate(blockPrefab, Vector3.zero, Quaternion.identity, movingParent);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500 + i * intervalDistance, 0, 0);
            UpdateStockValue(go, null, 0.25f - 0.25f / left * i, false);
        }
        for (; i < left + settings.totalRounds; i++)
        {
            var go = Instantiate(blockPrefab, Vector3.zero, Quaternion.identity, movingParent);
            blocks.Add(go);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500 + i * intervalDistance, 0, 0);
        }
        for (; i < left + settings.totalRounds + right; i++)
        {
            var go = Instantiate(blockPrefab, Vector3.zero, Quaternion.identity, movingParent);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500 + i * intervalDistance, 0, 0);
            go.SetEndBlock();
        }
        RefreshUI();
        return this;
    }
    void Update()
    {

    }

    void RefreshUI_AmountToDeal()
    {
        if (Mathf.Sign(amountToDealNextTime) > 0)
        {
            amountToDeal_text.text = "+" + amountToDealNextTime;
        }
        else
        {
            amountToDeal_text.text = amountToDealNextTime.ToString();
        }
    }

    public void DrawEstimatedFrame()
    {

    }

    public void UpdateStockValue(StockBlock blockToday, StockBlock lastDay, float value, bool animate = false)
    {
        if (lastDay != null)
        {
            blockToday.UpdateColor(value / 0.1f * 125 > lastDay.rectTransform.anchoredPosition.y, animate);
        }
        else
        {
            blockToday.UpdateColor(false);
        }
        blockToday.rectTransform.anchoredPosition = new Vector2(blockToday.rectTransform.anchoredPosition.x, value / 0.1f * 60);
    }

    private void RefreshUI(bool updateSpentMoney = false)
    {
        holding_text.text = $"持有:{stock}";
        cach_text.text = "现金:" + (cash - amountToDealNextTime).ToString();
        RefreshUI_AmountToDeal();
        Yestoday_Earned.text = f_Yestoday_Earned.ToString();
        if (f_Yestoday_Earned < 0) Yestoday_Earned.color = settings.green;
        else Yestoday_Earned.color = settings.red;
        Earned_Text.text = f_earned.ToString();
        if (f_earned < 0) Earned_Text.color = settings.green;
        else Earned_Text.color = settings.red;

        titleText.text = GetTtile();
        // if (updateSpentMoney)
        // {
        //     cach_text.text = "现金:" + (cash - amountToDealNextTime).ToString();
        // }
        // else
        // {
        //     cach_text.text = "现金:" + cash.ToString();
        // }
    }
    private void MoveCanvas()
    {
        movingParent.DOLocalMoveX(-intervalDistance * currentRound, 1f);
    }

    private int GetBuyOrSell()
    {
        if (amountToDealNextTime > 0)
        {
            return 1;
        }
        else if (amountToDealNextTime < 0)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    private string GenerearteResultString()
    {
        if (cash <= initCash)
        {
            return string.Format("炒股超成这样，还是找檀经理报个班吧。\n收益{0}:", f_earned);
        }
        else if (cash > initCash * 0.2f)
        {
            return string.Format("这是后疫情时代的投资美学！。\n收益{0}:", f_earned);
        }
        else
        {
            return string.Format("大吉大利，今晚大龙燚。\n收益{0}:", f_earned);
        }
    }
}
