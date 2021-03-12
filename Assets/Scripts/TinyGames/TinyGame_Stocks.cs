using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
public class TinyGame_Stocks : MonoBehaviour
{
    [SerializeField] Text cach_text;
    [SerializeField] Button buy_button;
    [SerializeField] Button sell_button;
    
    [SerializeField] AnimationCurve curve;
    
    UILineRenderer lineRenderer;

    public int totolDealPoints = 10;

    int currentDealPoints = 0;
    float cash;
    float stock;
    float amountToDealNextTime;
    private WaitForSeconds waitFornextStockChange = new WaitForSeconds(3);
    void Start()
    {
        StartCoroutine(eba1ag3asdg38());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Buy()
    {
        amountToDealNextTime += 100;
        cach_text.text = cash.ToString();
    }

    public void Sell()
    {
        amountToDealNextTime -= 100;
        cach_text.text = cash.ToString();
    }

    IEnumerator eba1ag3asdg38()
    {
        while (currentDealPoints < totolDealPoints)
        {
            yield return waitFornextStockChange;
            DoDeal();
        }

        cash += stock;
        Debug.Log($"cash = {cash}");
    }

    private void DoDeal()
    {
        float amountToDeal = cash >= amountToDealNextTime ? amountToDealNextTime : cash;
        cash -= amountToDeal;
        stock += amountToDeal;
        stock = stock / curve.Evaluate(currentDealPoints) * curve.Evaluate(currentDealPoints + 1);
        currentDealPoints += 1;
    }
}
