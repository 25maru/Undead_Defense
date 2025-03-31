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

    [Header("디버그 (테스트용)")]
    [SerializeField] private bool testMode;

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
        building = new DefensiveBuilding();
    }

    private void Update()
    {
        if (testMode && playerInZone)
        {
            if (Input.GetKey(holdKey))
            {
                if (!isHolding)
                {
                    isHolding = true;
                }

                holdTimer += Time.deltaTime;
                float remain = Mathf.Clamp(holdDuration - holdTimer, 0f, holdDuration);

                if (holdTimer >= holdDuration)
                {
                    constructionController.StartConstruction(building);
                    ResetHold();
                }
            }
            else if (isHolding)
            {
                ResetHold();
            }
        }

        if (!playerInZone || resourceManager.Gold < building.BuildCost) return;

        if (Input.GetKey(holdKey))
        {
            if (!isHolding)
            {
                isHolding = true;
                goldUIRoot.SetActive(true);
            }

            holdTimer += Time.deltaTime;
            float remain = Mathf.Clamp(holdDuration - holdTimer, 0f, holdDuration);

            // TODO: 남은 시간에 비례하게 골드 UI 채우는 코드 작성

            if (holdTimer >= holdDuration)
            {
                resourceManager.SpendGold(building.BuildCost);
                constructionController.StartConstruction(building);
                ResetHold();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            ResetHold();
        }
    }
}
