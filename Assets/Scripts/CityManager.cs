using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CityManager : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayMusic(AudioConst.citybgm1);
        FirstOpenGame.InTutorial = false;
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
        TinyGame_Coding.Init();
        AudioManager.Instance.PlayMusic(AudioConst.codingbgm);
    }
    public void OnSelectBuilding2()
    {
        TinyGame_Stocks.StartOrRestartGame();
        AudioManager.Instance.PlayMusic(AudioConst.stocksbgm);
    }

    public void ReturnToForest()
    {
        GameManager.FromCityToForest = true;
        SceneManager.LoadScene("Forest", LoadSceneMode.Single);
    }

}
