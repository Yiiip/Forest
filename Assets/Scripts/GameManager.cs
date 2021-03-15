using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    private World world;
    private Forest forest;


    [SerializeField] public WorldConfig worldConfig;
    [SerializeField] public sBuildingSheet buildingSheet;
    [SerializeField] public sCharacterSheet characterSheet;
    [SerializeField] public List<sDialogSheet> dialogSheetList;


    protected override void Awake()
    {
        base.Awake();

        world = new World();
        forest = new Forest();

        world.Init(SaveData.current, worldConfig);
    }

    public void AddNewDayChangedListener(Action<int> act)
    {
        world.OnNewDay -= act;
        world.OnNewDay += act;
    }

    private void Update()
    {
        world.UpdateLogic();
    }

    public void OnSave()
    {
    }


    public sCharacterVO GetCharacterVO(string id)
    {
        if (characterSheet == null) return null;
        foreach (var i in characterSheet.characters)
        {
            if (i.m_id == id) return i;
        }
        return null;
    }

    public sDialogSheet GetDialogSheet(string sheetId)
    {
        if (dialogSheetList == null) return null;
        return dialogSheetList.Find(i => i.m_sheetId == sheetId);
    }
}
