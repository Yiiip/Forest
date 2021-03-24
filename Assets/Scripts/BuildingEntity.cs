﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;

public class BuildingEntity : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Collider2D entityCollider;
    private List<Light2D> light2Ds;

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

    public void InitForTemp(string staticDataId)
    {
        this.staticDataId = staticDataId;
        this.staticData = StaticDataManager.Instance.GetBuildingVO(staticDataId);
    }

    void Awake()
    {
        light2Ds = new List<Light2D>();
        var lights = GetComponentsInChildren<Light2D>(true);
        if (lights != null)
        {
            light2Ds.AddRange(lights);
            foreach (var light in light2Ds)
            {
                light.gameObject.SetActiveOptimize(false);
                light.intensity = 0f;
            }
        }
    }

    void Start()
    {
        var lis = EventTriggerListener.Get(this.gameObject);
        lis.onClick = OnClick;
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

    private void OnClick(GameObject go)
    {
        if (staticData == null)
        {
            Debug.LogWarning($"'{go.name}' staticData is null");
            return;
        }
        Debug.Log(staticData.m_name);

        switch (staticData.m_buildingType)
        {
            case eBuildingType.WaterSource:
            {
                GameManager.Instance.World.CharacterEntities.ForEach(i =>
                {
                    i.MoveToTarget(gameObject.transform, 23);
                });
                break;
            }
            case eBuildingType.AppleTree:
            {
                int count = GameManager.Instance.World.CharacterEntities.Count;
                var radom = GameManager.Instance.World.CharacterEntities[UnityEngine.Random.Range(0, count)];
                radom.MoveToTarget(gameObject.transform, 7);
                break;
            }
        }
    }

    private void Update()
    {
        // Collider2D[] r = new Collider2D[10];
        // var contactFilter2D = new ContactFilter2D();
        // contactFilter2D.useTriggers = true;
        // entityCollider.OverlapCollider(contactFilter2D, r);
        // if (r != null)
        // {
        //     for (int i = 0; i < r.Length; i++)
        //     {
        //         if (r[i] != null)
        //         {
        //             Debug.Log($"{i} -> {r[i].name}");
        //         }
        //     }
        // }

        UpdateLight();
    }

    private void UpdateLight()
    {
        foreach (var light in light2Ds)
        {
            var percent = GameManager.Instance.World.GetTodayPercent();
            if (percent >= 0.7f && percent <= 0.99f)
            {
                if (!light.gameObject.activeSelf)
                {
                    light.gameObject.SetActive(true);
                    light.intensity = 0;
                }
                light.intensity += Time.deltaTime * 3f;
            }
            else
            {
                light.intensity -= Time.deltaTime * 3f;
            }
            light.intensity = Mathf.Clamp01(light.intensity);
        }
    }
}
