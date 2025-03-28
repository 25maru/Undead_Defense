using UnityEngine;
using System.Collections;

public class ConstructionController : MonoBehaviour
{
    [Header("건설 완료 시 생성될 프리팹")]
    [SerializeField] private GameObject completedBuildingPrefab;

    [Header("건설 효과 프리팹 (파티클 등)")]
    [SerializeField] private GameObject constructionEffectPrefab;

    [Header("테스트용 데이터")]
    [SerializeField] private bool testMode = true;

    private Building building;

    private void Start()
    {
        if (testMode)
        {
            Debug.Log("[건설 테스트] 테스트 모드로 초기화");
            // StartConstruction(new DefensiveBuilding());
            StartConstruction(new ProductionBuilding());
        }
        
    }

    public void StartConstruction(Building buildingData)
    {
        building = buildingData;
        StartCoroutine(ConstructionRoutine());
    }

    private IEnumerator ConstructionRoutine()
    {
        Debug.Log($"건설 시작 - {building.Name}, 시간: {building.BuildTime}");

        GameObject effect = null;
        if (constructionEffectPrefab != null)
        {
            effect = Instantiate(constructionEffectPrefab, transform.position, Quaternion.identity);
            effect.transform.SetParent(transform);
        }

        yield return new WaitForSeconds(building.BuildTime);

        Debug.Log("건설 완료!");

        if (effect != null)
            Destroy(effect);

        if (completedBuildingPrefab != null)
        {
            GameObject go = Instantiate(completedBuildingPrefab, transform.position, Quaternion.identity);

            var logic = go.GetComponent<BuildingLogicController>();
            if (logic != null)
            {
                logic.Initialize(building);
            }
        }

        Destroy(gameObject); // 건설 프리팹 제거
    }
}