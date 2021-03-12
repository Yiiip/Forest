﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    private World world;
    private Forest forest;


    public WorldConfig worldConfig;


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
        SaveData.current.playerProfile.globalTimer = world.GlobalTimer;
    }
}