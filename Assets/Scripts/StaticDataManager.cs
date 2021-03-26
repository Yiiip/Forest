using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDataManager : SingletonMonoEntire<StaticDataManager>
{
    [SerializeField] public sBuildingSheet buildingSheet;
    [SerializeField] public sCharacterSheet characterSheet;
    [SerializeField] public List<sDialogSheet> dialogSheetList;

    public sBuildingVO GetBuildingVO(string id)
    {
        if (buildingSheet == null) return null;
        foreach (var i in buildingSheet.buildings)
        {
            if (i.m_id == id) return i;
        }
        return null;
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
        foreach (var i in dialogSheetList)
        {
            if (i.m_sheetId == sheetId) return i;
        }
        return null;
    }
}
