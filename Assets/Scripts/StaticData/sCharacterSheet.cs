using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "角色表", menuName = "StaticData/角色表")]
public class sCharacterSheet : ScriptableObject
{
    public List<sCharacterVO> characters;
}
