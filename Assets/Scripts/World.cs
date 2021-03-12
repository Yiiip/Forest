using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class World : Singleton<World>
{
    public event Action<int> OnNewDay;

    private SaveData saveData;
    private WorldConfig worldConfig;

    private int _day;

    public void Init(SaveData saveData, WorldConfig worldConfig)
    {
        this._day = -1;
        this.saveData = saveData;
        this.worldConfig = worldConfig;
        this.OnNewDay += OnNewDayChanged;
    }

    public void UpdateLogic()
    {
        saveData.playerProfile.globalTimer += Time.unscaledDeltaTime;

        int curDay = GetGlobalDay();
        if (curDay != _day)
        {
            _day = curDay;
            OnNewDay?.Invoke(curDay);
        }
    }

    /// <summary>
    /// 第几天（从1开始）
    /// </summary>
    /// <returns></returns>
    public int GetGlobalDay()
    {
        return 1 + Mathf.FloorToInt(saveData.playerProfile.globalTimer / worldConfig.secondsPerDay);
    }

    private void OnNewDayChanged(int curDay)
    {
        Debug.Log($"第{curDay}天");
    }
}
