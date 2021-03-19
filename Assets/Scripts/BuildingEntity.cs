using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingEntity : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Collider2D entityCollider;

    [SerializeField]
    public int presetUniqueId = 0;
    [SerializeField]
    public string staticDataId;

    private BuildingPO buildingPO;
    private sBuildingVO staticData;

    public void Init(BuildingPO buildingPo)
    {
        this.buildingPO = buildingPo;
        this.staticData = StaticDataManager.Instance.GetBuildingVO(buildingPO.staticDataId);
        this.staticDataId = buildingPO.staticDataId;
        this.presetUniqueId = buildingPO.uniqueId;
    }

    void Start()
    {
        var lis = EventTriggerListener.Get(this.gameObject);
        lis.onClick = delegate(GameObject go)
        {
            // Debug.Log("onclick");
        };
        lis.onBeginDrag = delegate(PointerEventData eventData)
        {
            // Debug.Log("onBeginDrag");
        };
        lis.onDrag = delegate(PointerEventData eventData)
        {
            // Debug.Log("onDrag");
        };
        lis.onEndDrag = delegate(PointerEventData eventData)
        {
            // Debug.Log("onEndDrag");
        };
        lis.onEnter = delegate(GameObject go)
        {
            // Debug.Log("onEnter");
        };
        lis.onExit = delegate(GameObject go)
        {
            // Debug.Log("onExit");
        };
    }

}
