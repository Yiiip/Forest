using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;

public class BuildingEntity : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer spriteRenderer;
    [SerializeField]
    public Collider2D entityCollider;
    private List<Light2D> light2Ds;

    [SerializeField]
    public int presetUniqueId = 0;
    [SerializeField]
    public string staticDataId;

    [HideInInspector]
    public BuildingPO buildingPO;
    [HideInInspector]
    public sBuildingVO staticData;

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
        lis.onEnter = OnEnter;
        lis.onExit = OnExit;
    }

    private void OnEnter(GameObject go)
    {
        // Debug.Log("OnEnter");
        if (staticData == null)
        {
            return;
        }
        switch (staticData.m_buildingType)
        {
            case eBuildingType.AppleTree:
            {
                UIManager.Instance.GetUI<BillboardsUI>().ShowAppleTreeBillboard(this);
                break;
            }
        }
    }

    private void OnExit(GameObject go)
    {
        // Debug.Log("OnExit");
        if (staticData == null)
        {
            return;
        }
        switch (staticData.m_buildingType)
        {
            case eBuildingType.AppleTree:
            {
                UIManager.Instance.GetUI<BillboardsUI>().HideAppleTreeBillboard();
                break;
            }
        }
    }

    private void OnClick(GameObject go)
    {
        if (staticData == null)
        {
            Debug.LogWarning($"'{go.name}' staticData is null");
            return;
        }
        Debug.Log(staticData.m_name);

        if (FirstOpenGame.InTutorial)
        {
            return;
        }

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
                if (buildingPO.workState == eWorkState.None)
                {
                    ToastUI.Toast("开始采摘");
                    StateToWorking();
                }
                else if (buildingPO.workState == eWorkState.Working)
                {
                    ToastUI.Toast("正在采摘中");
                }
                else if (buildingPO.workState == eWorkState.ReadyToHavest)
                {
                    StateToNone();
                    ToastUI.Toast("采摘完成");
                    SaveData.current.playerProfile.coin += 100;
                }
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

        UpdateWork();
        UpdateLight();
    }

    private void UpdateWork()
    {
        if (buildingPO == null)
        {
            return;
        }
        switch (buildingPO.workState)
        {
            case eWorkState.None:
                break;
            case eWorkState.Working:
                buildingPO.workTimer += Time.unscaledDeltaTime;
                if (buildingPO.workTimer >= staticData.m_workingDuration)
                {
                    StateToReadyToHavest();
                }
                break;
            case eWorkState.ReadyToHavest:
                break;
        }

        if (staticDataId == "AppleTree")
        {
            int appleCount = 8;
            if (buildingPO.workState == eWorkState.Working)
            {
                var percent = Mathf.Clamp01(buildingPO.workTimer / staticData.m_workingDuration);
                for (int i = 0; i <= appleCount; i++)
                {
                    transform.Find($"apple{i}").gameObject.SetActiveOptimize(percent >= 1f*i/appleCount);
                }
            }
            else if (buildingPO.workState == eWorkState.ReadyToHavest)
            {
                for (int i = 0; i <= appleCount; i++)
                {
                    transform.Find($"apple{i}").gameObject.SetActiveOptimize(true);
                }
            }
            else
            {
                for (int i = 0; i <= appleCount; i++)
                {
                    transform.Find($"apple{i}").gameObject.SetActiveOptimize(false);
                }
            }
        }
    }

    public void StateToWorking()
    {
        if (buildingPO != null)
        {
            buildingPO.workTimer = 0f;
            buildingPO.workState = eWorkState.Working;
        }
    }

    public void StateToNone()
    {
        if (buildingPO != null)
        {
            buildingPO.workState = eWorkState.None;
        }
    }

    public void StateToReadyToHavest()
    {
        if (buildingPO != null)
        {
            buildingPO.workState = eWorkState.ReadyToHavest;
        }
    }

    private void UpdateLight()
    {
        foreach (var light in light2Ds)
        {
            var percent = GameManager.Instance.World.GetTodayPercent();
            if (percent >= World.LightPercent && percent <= 0.99f)
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
