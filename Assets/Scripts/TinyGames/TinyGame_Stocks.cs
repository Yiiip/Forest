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
            RefreshUI();
        }

    }
    public void SellButtonClicked()
    {
        // return;
        if (currentRound < settings.totalRounds)
        {
            amountToDealNextTime -= 100;
            if (-amountToDealNextTime > stock)
                amountToDealNextTime = -stock;
            RefreshUI();
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
    float f_Yestoday_Earned = 0;
    float f_earned = 0;
    private void DoDeal()
    {
        float total = cash + stock;
        cash -= amountToDealNextTime < 0 ? amountToDealNextTime * 0.985f : amountToDealNextTime;
        stock += amountToDealNextTime;
        float a = currentRound / (float)settings.totalRounds;
        float b = (currentRound + 1) / (float)settings.totalRounds;
        float currentValue = settings.curve.Evaluate(a);
        float nextValue = settings.curve.Evaluate(b) + Random.Range(0, .1f);;
        float currentRate = (nextValue + 1) / (currentValue + 1);
        stock *= currentRate;

        Debug.Log($"Dealed amount {amountToDealNextTime}, current rate = {currentRate}");
        if (currentRound == 0)
        {
            UpdateStockValue(blocks[currentRound], blocks[currentRound], nextValue, true);
        }
        else
        {
            UpdateStockValue(blocks[currentRound], blocks[currentRound - 1], nextValue, true);
        }
        currentRound += 1;
        f_earned = stock + cash - settings.initCash;
        f_Yestoday_Earned = cash + stock - total;
        if (amountToDealNextTime > 0 && cash < amountToDealNextTime) amountToDealNextTime = cash;
        if (amountToDealNextTime < 0 && stock < -amountToDealNextTime) amountToDealNextTime = -stock;
        MoveCanvas();
        RefreshUI();
        if (currentRound == settings.totalRounds)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        cash += stock;
        stock = 0;
        EndCard.ShowEndCardWithContent(GenerearteResultString(), transform);
        SaveData.current.playerProfile.coin += (int)(cash - settings.initCash);
        RefreshUI();
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
    public Color green;
    public Color red;
}