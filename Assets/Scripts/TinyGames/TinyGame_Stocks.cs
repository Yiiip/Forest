using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public partial class TinyGame_Stocks : MonoBehaviour
{
    float cash;
    float stock;
    float amountToDealNextTime;
    int level;
    int currentRound = 0;
    public static TinyGame_Stocks instance;
    public StockGameSettings settings;
    public static void StartOrRestartGame(StockGameSettings settings = null)
    {
        if (instance != null) Destroy(instance.gameObject);

        instance = Instantiate(Resources.Load<TinyGame_Stocks>("Prefabs/TinyGame_Stocks/TinyGame_Stocks")).InitGame().InitUI();
        instance.transform.SetParent(GameObject.Find("Canvas").transform);
        var rect = instance.GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
        rect.localScale = Vector3.one;
        if (settings != null) instance.settings = settings;

    }

    public TinyGame_Stocks InitGame()
    {
        cash = settings.initCash;
        return this;
    }

    public void BuyButtonClicked()
    {
        if (currentRound < settings.totalRounds)
        {
            amountToDealNextTime += 100;
            if (amountToDealNextTime > cash)
                amountToDealNextTime = cash;
            RefreshUI_AmountToDeal();
        }

    }
    public void SellButtonClicked()
    {
        if (currentRound < settings.totalRounds)
        {
            amountToDealNextTime -= 100;
            if (-amountToDealNextTime > stock)
                amountToDealNextTime = -stock;
            RefreshUI_AmountToDeal();
        }
    }

    public void ConfirmButtonClicked()
    {
        if (currentRound < settings.totalRounds)
        {
            DoDeal();
            DoAnimatations();
        }
        //end game?
        // endCard.ShowEndCard(GenerearteResultString());
    }

    private void DoDeal()
    {
        cash -= amountToDealNextTime < 0 ? amountToDealNextTime * 0.985f : amountToDealNextTime;
        stock += amountToDealNextTime;
        float currentValue = settings.curve.Evaluate((float)currentRound / settings.totalRounds);
        float nextValue = settings.curve.Evaluate((float)(currentRound + 1) / settings.totalRounds);
        float currentRate = nextValue / currentValue;
        stock *= currentRate;

        Debug.Log($"Dealed amount {amountToDealNextTime}, current rate = {currentRate}");
        if (currentRound == 0)
        {
            UpdateStockValue(blocks[currentRound], blocks[currentRound], currentValue - 1);
        }
        else
        {
            UpdateStockValue(blocks[currentRound], blocks[currentRound - 1], currentValue - 1);
        }
        currentRound += 1;
        MoveCanvas();
        if (currentRound == settings.totalRounds)
        {
            // EndGame();
        }
    }

    private void DoAnimatations()
    {

    }

    public void StartNewRound()
    {
        // UIRenderer.ShowUIs();
    }
}

[System.Serializable]
public class StockGameSettings
{
    public int initCash = 1000;
    public int totalRounds = 10;
    public AnimationCurve curve;
}