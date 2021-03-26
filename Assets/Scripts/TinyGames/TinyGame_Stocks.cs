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
    AnimationCurve[] curves;
    public static void StartOrRestartGame(StockGameSettings settings = null)
    {
        if (instance != null) Destroy(instance.gameObject);

        instance = Instantiate(Resources.Load<TinyGame_Stocks>("Prefabs/TinyGame_Stocks/TinyGame_Stocks")).InitGame().InitUI();
        instance.transform.SetParent(GameObject.Find("Canvas").transform);
        var rect = instance.GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
        rect.localScale = Vector3.one;
        if (settings != null) instance.settings = settings;
        instance.curves = new AnimationCurve[3];
        instance.curves[0] = instance.settings.curve;
        instance.curves[1] = instance.settings.curve1;
        instance.curves[2] = instance.settings.curve2;
    
    }
    
    public TinyGame_Stocks InitGame()
    {
        cash = settings.initCash;
        randCurve = Random.Range(0, 3);
        return this;
    }

    public string GetTtile()
    {
        switch (randCurve)
        {
            case 0:
                return "波动";
            case 1:
                return "见好就收";
            case 2:
                return "坚持不懈";
        }
        return null;
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
    int randCurve = 0;
    private void DoDeal()
    {
        float total = cash + stock;
        cash -= amountToDealNextTime < 0 ? amountToDealNextTime * 0.985f : amountToDealNextTime;
        stock += amountToDealNextTime;
        float a = currentRound / (float)settings.totalRounds;
        float b = (currentRound + 1) / (float)settings.totalRounds;
        float currentValue = curves[randCurve].Evaluate(a);
        float nextValue = curves[randCurve].Evaluate(b);
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
    public AnimationCurve curve1;
    public AnimationCurve curve2;
    public Color green;
    public Color red;
}