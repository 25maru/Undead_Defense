using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildTriggerTest : MonoBehaviour
{
    public enum TargetTask
    {
        None,
        Build,
        Upgrade
    }

    [Header("건물 설정")]
    [SerializeField] private TargetTask task;
    [SerializeField] private BuildingType type;
    [SerializeField] private int requiredGold;

    [Header("입력 설정")]
    [SerializeField] private KeyCode holdKey = KeyCode.Space;
    // [SerializeField] private float holdDuration = 2f;

    [Header("UI")]
    [SerializeField] private GameObject goldUIRoot;
    [SerializeField] private BuildingGoldUI goldUI;

    [Header("디버그 (테스트용)")]
    [SerializeField] private bool testMode;

    private ResourceManager resourceManager;
    private ConstructionController constructionController;
    private BuildingLogicController buildingLogicController;
    private Building building;

    private (ConstructionController, BuildingLogicController) controller;

    private int useGold = 0;
    private bool playerInZone = false;
    private bool isHolding = false;
    private bool isBuilt = false;
    private float holdTimer = 0f;

    private void Start()
    {
        resourceManager = ResourceManager.Instance;

        switch (task)
        {
            case TargetTask.Build:
                constructionController = GetComponent<ConstructionController>();
                break;

            case TargetTask.Upgrade:
                buildingLogicController = GetComponent<BuildingLogicController>();
                break;
        }

        switch (type)
        {
            case BuildingType.Defense:
                building = new DefensiveBuilding();
                break;

            case BuildingType.Production:
                building = new ProductionBuilding();
                break;

            case BuildingType.Spawner:
                building = new SpawnerBuilding(new(), 5);
                break;
        }

        building.BuildCost = requiredGold;
        goldUI.Setup(requiredGold);
    }

    private void Task()
    {
        switch (task)
        {
            case TargetTask.Build:
                constructionController.StartConstruction(building);
                break;

            case TargetTask.Upgrade:
                buildingLogicController.Upgrade();
                break;
        }
    }

    private void Update()
    {
        if (!playerInZone || isBuilt) return;

        // 테스트 모드 (골드 소비 X)
        if (testMode)
        {
            HandleHold(() =>
            {
                goldUI.FillNext();
                if (goldUI.IsComplete())
                {
                    LevelManager.Instance.NightTrigger.BlockInput(false);
                    Task();
                    Invoke(nameof(ResetHold), 0.5f);
                }
            });
            return;
        }

        // 일반 모드
        if (resourceManager.Gold <= 0)
        {
            ResetHold();
            ResourceManager.Instance.AddGold(useGold);
            return;
        }

        HandleHold(() =>
        {
            goldUI.FillNext();
            resourceManager.SpendGold(1);

            if (goldUI.IsComplete())
            {
                isBuilt = true;
                LevelManager.Instance.NightTrigger.BlockInput(false);
                Task();
                Invoke(nameof(ResetHold), 0.5f);
            }
        });
    }

    private void HandleHold(System.Action onCompleteStep)
    {
        if (Input.GetKey(holdKey))
        {
            if (!isHolding)
            {
                isHolding = true;
                goldUIRoot.SetActive(true);
                goldUI.Setup(building.BuildCost);
            }

            holdTimer += Time.deltaTime;

            // 시간에 따라 Fill 동작
            float interval = goldUI.FillInterval;
            while (holdTimer >= interval)
            {
                useGold++;
                holdTimer -= interval;
                onCompleteStep.Invoke();
            }
        }
        else if (isHolding)
        {
            ResetHold();
            ResourceManager.Instance.AddGold(useGold);
        }
    }

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0f;
        useGold = 0;
        goldUI.ResetUI();
    }

    private void ExitZone()
    {
        playerInZone = false;

        if (!isBuilt)
        {
            ResourceManager.Instance.AddGold(useGold);
        }

        LevelManager.Instance.NightTrigger.BlockInput(false);
        goldUI.SetGoldVisible(false);
        ResetHold();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isBuilt)
        {
            playerInZone = true;
            LevelManager.Instance.NightTrigger.BlockInput(true);
            goldUI.SetGoldVisible(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExitZone();
        }
    }
}
