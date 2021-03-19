using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
public class TinyGame_Stocks : MonoBehaviour
{
    [SerializeField] Text cach_text;
    [SerializeField] Text holding_text;
    [SerializeField] Button buy_button;
    [SerializeField] Button sell_button;

    [SerializeField] AnimationCurve curveA;
    [SerializeField] AnimationCurve curveB;
    [SerializeField] AnimationCurve curveC;
    [SerializeField] AnimationCurve[] curves;


    [SerializeField] UILineRenderer lineRenderer;

    public int totolDealPoints = 10;
    public int initCash = 1000;

    public int intervalDistance = 50;
    public int intervalTime = 3;
    int currentDealPoints = 0;
    float cash;
    float stock;
    float amountToDealNextTime;
    int level;
    private WaitForSeconds waitFornextStockChange;
    void Start()
    {
        cash = initCash;
        RefreshUI(false);
        waitFornextStockChange = new WaitForSeconds(intervalTime);
        curves = new AnimationCurve[] { curveA, curveB, curveC };
        StartCoroutine(eba1ag3asdg38());
    }

    // Update is called once per frame
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

    IEnumerator eba1ag3asdg38()
    {
        while (currentDealPoints < totolDealPoints)
        {
            yield return waitFornextStockChange;
            DoDeal();
            DrawLines();
        }

        cash += stock;
        Debug.Log($"cash = {cash}");
    }

    private void DoDeal()
    {
        float amountToDeal = cash >= amountToDealNextTime ? amountToDealNextTime : cash;
        cash -= amountToDeal;
        stock += amountToDeal;
        float currentValue = curves[level].Evaluate((float)currentDealPoints / totolDealPoints);
        float nextValue = curves[level].Evaluate((float)(currentDealPoints + 1) / totolDealPoints);
        float currentRate = nextValue / currentValue;
        stock *= currentRate;

        Debug.Log($"Dealed amount {amountToDeal}, current rate = {currentRate}");
        amountToDealNextTime = 0;
        currentDealPoints += 1;
    }

    private void DrawLines()
    {
        var newPoints = new Vector2[lineRenderer.Points.Length + 1];
        lineRenderer.Points.CopyTo(newPoints, 0);
        newPoints[lineRenderer.Points.Length] = new Vector2(intervalDistance * currentDealPoints, curves[level].Evaluate((float)currentDealPoints / totolDealPoints) * 100 - 100);
        lineRenderer.Points = newPoints;
        RefreshUI();
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
}
