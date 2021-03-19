using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldPO : IPersistentObject
{
    public List<CharacterPO> characters;
    public List<BuildingPO> buildings;

    public void Init()
    {
        characters = new List<CharacterPO>();
        buildings = new List<BuildingPO>();
    }
}
