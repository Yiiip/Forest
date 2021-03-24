using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CityManager : MonoBehaviour
{
    [SerializeField] GameObject endCard;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TinyGame_Stocks.StartOrRestartGame();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            // TinyGame_Stocks_Game.StartOrRestartGame();
        }
    }

    public void OnSelectBuilding1()
    {
        TinyGame_Stocks.StartOrRestartGame();
    }
    public void OnSelectBuilding2()
    {

    }

    public void ReturnToForest()
    {
        SceneManager.LoadScene("Forest", LoadSceneMode.Single);
    }

}
