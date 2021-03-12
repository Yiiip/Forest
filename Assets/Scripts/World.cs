using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class World : Singleton<World>
{
    public float GlobalTimer { get; private set; }

    public event Action<int> OnNewDay;

    private WorldConfig worldConfig;

    private int _day;

    public void Init(SaveData saveData, WorldConfig worldConfig)
    {
        this.worldConfig = worldConfig;
        this.GlobalTimer = saveData.playerProfile.globalTimer;
        this._day = -1;
    }

    public void UpdateLogic()
    {
        GlobalTimer += Time.unscaledDeltaTime;

        int curDay = GetGlobalDay();
        if (curDay != _day)
        {
            OnNewDay?.Invoke(curDay);
        }
    }

    /// <summary>
    /// 第几天（从1开始）
    /// </summary>
    /// <returns></returns>
    public int GetGlobalDay()
    {
        return 1 + Mathf.FloorToInt(GlobalTimer / worldConfig.secondsPerDay);
    }
}
