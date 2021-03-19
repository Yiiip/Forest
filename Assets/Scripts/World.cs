﻿using System;
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

        InitChatacters();
        InitBuildings();
    }

    private void InitChatacters()
    {
        if (saveData.worldPO.characters.Count == 0)
        {
            //给新玩家一个兔子
            saveData.worldPO.characters.Add(CharacterPO.CreateDefaultCharacter());
        }

        for (int i = 0; i < saveData.worldPO.characters.Count; i++)
        {
            var characterPo = saveData.worldPO.characters[i];
            var staticData = StaticDataManager.Instance.GetCharacterVO(characterPo.staticDataId);
            GameObject go = UIUtility.InstantiatePrefab(staticData.m_prefab, GameManager.Instance.MovablesNode);
            float x = UnityEngine.Random.Range(-50f, 50f);
            float y = UnityEngine.Random.Range(-50f, 50f);
            go.transform.localPosition = new Vector3(x, y, go.transform.localPosition.z);
            var characterEntity = go.GetComponent<CharacterEntity>();
            if (characterEntity != null)
            {
                characterEntity.Init(characterPo);
            }
        }
    }

    private void InitBuildings()
    {
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
