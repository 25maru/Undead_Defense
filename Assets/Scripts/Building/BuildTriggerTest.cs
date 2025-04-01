using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildTriggerTest : MonoBehaviour
{
    [Header("입력 설정")]
    [SerializeField] private KeyCode holdKey = KeyCode.Space;
    [SerializeField] private float holdDuration = 2f;

    [Header("UI")]
    [SerializeField] private GameObject goldUIRoot;
    [SerializeField] private BuildingGoldUI goldUI;

    [Header("디버그 (테스트용)")]
    [SerializeField] private bool testMode;
    [SerializeField] private BuildingType type;
    [SerializeField] private int requiredGold;

    private ResourceManager resourceManager;
    private ConstructionController constructionController;
    private Building building;

    private bool playerInZone = false;
    private bool isHolding = false;
    private float holdTimer = 0f;

    private void Start()
    {
        resourceManager = ResourceManager.Instance;
        constructionController = GetComponent<ConstructionController>();

        switch (type)
        {
            case BuildingType.Defense:
                building = new DefensiveBuilding();
                break;

            case BuildingType.Production:
                building = new ProductionBuilding();
                break;

            case BuildingType.Spawner:
                building = new SpawnerBuilding(new());
                break;
        }
        
        if (testMode) building.BuildCost = requiredGold;
        else requiredGold = building.BuildCost;

        goldUI.Setup(requiredGold);
    }

    private void Update()
    {
        if (!playerInZone) return;

        // 테스트 모드
        if (testMode)
        {
            HandleHold(() =>
            {
                goldUI.FillNext(); // 골드 UI 채우기 (실제 자원 소비 없음)
                if (goldUI.IsComplete())
                {
                    LevelManager.Instance.NightTrigger.BlockInput(false);
                    constructionController.StartConstruction(building);
                    Invoke(nameof(ResetHold), 0.5f);
                }
            });
            return;
        }

        // 일반 모드
        if (resourceManager.Gold < building.BuildCost) return;

        HandleHold(() =>
        {
            goldUI.FillNext();
            resourceManager.SpendGold(1);

            if (goldUI.IsComplete())
            {
                LevelManager.Instance.NightTrigger.BlockInput(false);
                constructionController.StartConstruction(building);
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
            float interval = holdDuration / building.BuildCost;
            while (holdTimer >= interval)
            {
                holdTimer -= interval;
                onCompleteStep.Invoke();
            }
        }
        else if (isHolding)
        {
            ResetHold();
        }
    }

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0f;
        // goldUIRoot.SetActive(false);
        goldUI.ResetUI();
    }

    private void ExitZone()
    {
        playerInZone = false;
        LevelManager.Instance.NightTrigger.BlockInput(false);
        goldUI.SetGoldVisible(false);
        ResetHold();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
