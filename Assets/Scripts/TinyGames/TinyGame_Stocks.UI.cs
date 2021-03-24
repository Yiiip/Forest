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
    [ObjectReference("Blocks/MovingParent")] RectTransform movingParent;

    [ObjectReference("Control/BuyButton")] Button buy_button;
    [ObjectReference("Control/SellButton")] Button sell_button;
    [ObjectReference("Control/ConfirmButton")] Button confirm_button;
    List<StockBlock> blocks;
    StockBlock blockPrefab;
    public int totolDealPoints = 10;
    public int initCash = 1000;

    int intervalDistance = 100;
    public int intervalTime = 3;
    int currentDealPoints = 0;

    void Awake()
    {
        ObjectReferenceAttribute.GetReferences(this);
        blockPrefab = Resources.Load<StockBlock>("Prefabs/TinyGame_Stocks/Block");
    }

    public TinyGame_Stocks Init()
    {
        buy_button.onClick.AddListener(BuyButtonClicked);
        sell_button.onClick.AddListener(SellButtonClicked);
        confirm_button.onClick.AddListener(ConfirmButtonClicked);

        blocks = new List<StockBlock>();
        int i = 0;
        for (; i < 5; i++)
        {
            var go = Instantiate(blockPrefab, Vector3.zero, Quaternion.identity, movingParent);
            blocks.Add(go);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500 + i * intervalDistance, 0, 0);
            UpdateStockValue(i, 0.2f - 0.2f / 4 * i);
        }
        for (; i < 5 + settings.totalRounds; i++)
        {
            var go = Instantiate(blockPrefab, Vector3.zero, Quaternion.identity, movingParent);
            blocks.Add(go);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500 + i * intervalDistance, 0, 0);
        }
        for (; i < 5 + settings.totalRounds + 5; i++)
        {
            var go = Instantiate(blockPrefab, Vector3.zero, Quaternion.identity, movingParent);
            blocks.Add(go);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500 + i * intervalDistance, 0, 0);
        }
        RefreshUI(false);
        return this;
    }
    void Update()
    {

    }

    public void Buy()
    {
        if (currentDealPoints < totolDealPoints)
        {
            amountToDealNextTime += 100;
            if (amountToDealNextTime > cash)
                amountToDealNextTime = cash;
            RefreshUI(true);
        }
    }

    public void Sell()
    {
        if (currentDealPoints < totolDealPoints)
        {
            amountToDealNextTime -= 100;
            if (-amountToDealNextTime > stock)
                amountToDealNextTime = -stock;
            RefreshUI(false);
        }
    }

    // private void DrawLines()
    // {
    //     var newPoints = new Vector2[lineRenderer.Points.Length + 1];
    //     lineRenderer.Points.CopyTo(newPoints, 0);
    //     newPoints[lineRenderer.Points.Length] = new Vector2(intervalDistance * currentDealPoints, curves[level].Evaluate((float)currentDealPoints / totolDealPoints) * 100 - 100);
    //     lineRenderer.Points = newPoints;
    //     RefreshUI();
    // }

    public void DrawEstimatedFrame()
    {

    }

    public void UpdateStockValue(int day, float value)
    {
        if (day != 0)
        {
            blocks[day].UpdateColor(blocks[day].rectTransform.anchoredPosition.y > blocks[day - 1].rectTransform.anchoredPosition.y);
        }
        else
        {
            blocks[day].UpdateColor(false);
        }
        blocks[day].rectTransform.anchoredPosition = new Vector2(blocks[day].rectTransform.anchoredPosition.x, value / 0.1f * 125);
    }

    private void RefreshUI(bool updateSpentMoney = false)
    {
        switch (GetBuyOrSell())
        {
            case 1:
                holding_text.text = $"持有:{stock}(购买待确认{amountToDealNextTime})";
                break;
            case -1:
                holding_text.text = $"持有:{stock}(卖出待确认{-amountToDealNextTime})";
                break;
            case 0:
                holding_text.text = $"持有:{stock}";
                break;
        }
        if (updateSpentMoney)
        {
            cach_text.text = "现金:" + (cash - amountToDealNextTime).ToString();
        }
        else
        {
            cach_text.text = "现金:" + cash.ToString();
        }
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
            return "炒股超成这样，还是找檀经理报个班吧。";
        }
        else if (cash > initCash * 0.2f)
        {
            return "大吉大利，今晚大龙燚";
        }
        else
        {
            return "这是后疫情时代的投资美学！";
        }
    }
}
