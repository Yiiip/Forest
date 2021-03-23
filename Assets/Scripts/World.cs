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

    private List<BuildingEntity> buildingEntities;
    private List<CharacterEntity> characterEntities;
    private TrainEntity trainEntity;

    public List<BuildingEntity> BuildingEntities { get => buildingEntities; }
    public List<CharacterEntity> CharacterEntities { get => characterEntities; }

    public void Init(SaveData saveData, WorldConfig worldConfig)
    {
        this._day = -1;
        this.saveData = saveData;
        this.worldConfig = worldConfig;
        this.OnNewDay += OnNewDayChanged;

        InitChatacters();
        InitBuildings();
        trainEntity = GameManager.Instance.MovablesNode.Find("Train").GetComponent<TrainEntity>();
    }

    private void InitChatacters()
    {
        characterEntities = new List<CharacterEntity>();

        if (saveData.worldPO.characters.Count == 0)
        {
            //给新玩家三个角色
            saveData.worldPO.characters.Add(CharacterPO.CreateRabbit());
            saveData.worldPO.characters.Add(CharacterPO.CreateBear());
            saveData.worldPO.characters.Add(CharacterPO.CreateMonkey());
        }

        var parentNode = GameManager.Instance.MovablesNode;

        for (int i = 0; i < saveData.worldPO.characters.Count; i++)
        {
            var characterPo = saveData.worldPO.characters[i];
            var staticData = StaticDataManager.Instance.GetCharacterVO(characterPo.staticDataId);
            GameObject go = UIUtility.InstantiatePrefab(staticData.m_prefab, parentNode);
            float x = UnityEngine.Random.Range(-25f, 25f);
            float y = UnityEngine.Random.Range(-25f, 25f);
            if (x >= 0) x += 10;
            if (x < 0) x -= 10;
            if (y >= 0) y += 10;
            if (y < 0) y -= 10;
            go.transform.localPosition = new Vector3(x, y, 0);
            var characterEntity = go.GetComponent<CharacterEntity>();
            if (characterEntity != null)
            {
                characterEntity.Init(characterPo);
                characterEntities.Add(characterEntity);
            }
        }
    }

    private void InitBuildings()
    {
        buildingEntities = new List<BuildingEntity>();

        var parentNode = GameManager.Instance.BuildingsNode;

        HashSet<int> presetIds = new HashSet<int>();

        for (int index = 0; index < parentNode.childCount; index++)
        {
            var child = parentNode.GetChild(index);
            var buildingEntity = child.gameObject.GetComponent<BuildingEntity>();
            if (buildingEntity != null)
            {
                int presetUniqueId = buildingEntity.presetUniqueId;
                if (presetUniqueId > 0) //load from map
                {
                    var buildingPo = saveData.worldPO.buildings.Find(i => i.uniqueId == presetUniqueId);
                    if (buildingPo == null)
                    {
                        buildingPo = BuildingPO.Generate(presetUniqueId, buildingEntity.staticDataId, child.position);
                        saveData.worldPO.buildings.Add(buildingPo);
                    }
                    buildingEntity.Init(buildingPo);
                    buildingEntities.Add(buildingEntity);
                    presetIds.Add(presetUniqueId);
                }
                else
                {
                    buildingEntity.InitForTemp(buildingEntity.staticDataId);
                    buildingEntities.Add(buildingEntity);
                }
            }
        }

        //load from saver
        foreach (var buildingPo in saveData.worldPO.buildings)
        {
            if (presetIds.Contains(buildingPo.uniqueId))
            {
                continue;
            }

            var staticData = StaticDataManager.Instance.GetBuildingVO(buildingPo.staticDataId);

            GameObject go = UIUtility.InstantiatePrefab(staticData.m_prefab, parentNode);
            go.transform.position = new Vector3(buildingPo.positionX, buildingPo.positionY, go.transform.position.z);
            var buildingEntity = go.GetComponent<BuildingEntity>();
            if (buildingEntity != null)
            {
                buildingEntity.Init(buildingPo);
                buildingEntities.Add(buildingEntity);
            }
        }
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

    public float GetTodayPercent()
    {
        return (saveData.playerProfile.globalTimer % worldConfig.secondsPerDay) / worldConfig.secondsPerDay;
    }

    private void OnNewDayChanged(int curDay)
    {
        Debug.Log($"第{curDay}天");

        AutoIncreaceCoin(curDay);
        AutoIncreaseWater(curDay);
    }

    private void AutoIncreaceCoin(int curDay)
    {
        if (curDay == 1)
        {
            SaveData.current.playerProfile.coin = 500;
        }
        else if (curDay >= 2)
        {
            SaveData.current.playerProfile.coin += 100;
        }
    }

    private void AutoIncreaseWater(int curDay)
    {
        if (curDay == 1)
        {
            SaveData.current.playerProfile.water = 500;
        }
        else if (curDay >= 2)
        {
            SaveData.current.playerProfile.water += 1;
        }
    }
}
