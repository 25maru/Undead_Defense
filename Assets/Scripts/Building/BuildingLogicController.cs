using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 건물 타입 이넘
public enum BuildingType
{
    Production,
    Defense,
    Main,
    Spawner
}

public class BuildingLogicController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject upgradedPrefab;
    private Building building;
    private bool isBuilt = false;
    
    [Header("건설 효과 프리팹 (파티클 등)")]
    [SerializeField] private GameObject constructionEffectPrefab;
    
    [Header("스폰 유닛")]
    [SerializeField] private List<GameObject> soldierPrefabs; // 🔥 이게 진짜 프리팹 리스트

    [Header("시각화")]
    [SerializeField] private LineRenderer rangeRenderer;
    [SerializeField] private int circleSegments = 60;
    
    [Header("체력바")]
    [SerializeField] private GameObject healthBarPrefab;
    private HealthBar healthBarInstance;



    private void DrawRangeCircle(float radius)
    {
        if (rangeRenderer == null) return;

        rangeRenderer.positionCount = circleSegments + 1;

        float angle = 0f;
        for (int i = 0; i <= circleSegments; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, 1f, z); // 살짝 띄움
            rangeRenderer.SetPosition(i, pos);
            angle += 2 * Mathf.PI / circleSegments;
        }
    }

    public void Initialize(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.Spawner:
                if (soldierPrefabs != null && soldierPrefabs.Count > 0)
                    building = new SpawnerBuilding(soldierPrefabs);
                else
                    Debug.LogWarning("⚠️ 병사 프리팹 리스트가 비어있습니다.");
                break;
            case BuildingType.Defense:
                building = new DefensiveBuilding();
                break;

            case BuildingType.Production:
                building = new ProductionBuilding();
                break;
        }
        
        if (building is DefensiveBuilding def)
        {
            DrawRangeCircle(def.Range);
        }
        
        // 체력바 프리팹 생성 및 세팅
        if (healthBarPrefab != null)
        {
            GameObject bar = Instantiate(healthBarPrefab, transform);

            // 머리 위로 띄우기
            bar.transform.localPosition = new Vector3(0, 20f, 0);

            // 부모 크기로 조절
            bar.transform.localScale = transform.localScale * 0.2f;

            healthBarInstance = bar.GetComponent<HealthBar>();

            if (healthBarInstance != null)
            {
                float hp = (float)building.CurrentHP / building.MaxHP;
                healthBarInstance.SetHealth(hp);
            }
        }
        
        isBuilt = true;
    }

    private void Update()
    {
        if (isBuilt && building != null)
        {
            building.Tick(this);
        }
    }
    
    public void Upgrade()
    {
        if (building == null || upgradedPrefab == null) return;

        StartCoroutine(UpgradeRoutine());
    }

    private IEnumerator UpgradeRoutine()
    {
        Debug.Log("🔧 업그레이드 시작...");

        isBuilt = false; // ⛔ Tick 멈추기

        // 파티클 출력
        GameObject effect = null;
        if (constructionEffectPrefab != null)
        {
            effect = Instantiate(constructionEffectPrefab, transform.position, Quaternion.identity);
            effect.transform.SetParent(transform);
        }

        yield return new WaitForSeconds(3f); // 업그레이드 시간 대기

        if (effect != null)
            Destroy(effect);

        building.Upgrade(); // 스탯 상승

        // 새로운 업그레이드 프리팹 생성
        GameObject newObj = Instantiate(upgradedPrefab, transform.position, transform.rotation);
        var newController = newObj.GetComponent<BuildingLogicController>();
        if (newController != null)
        {
            newController.Initialize(building.Type); // 상태 넘겨줌
        }

        Debug.Log("✅ 업그레이드 완료!");
        Destroy(gameObject); // 현재 오브젝트 제거
    }
    
    public void TakeDamage(int amount)
    {
        building?.TakeDamage(amount, this);

        if (healthBarInstance != null)
        {
            float hp = (float)building.CurrentHP / building.MaxHP;
            healthBarInstance.SetHealth(hp);
        }
    }
    
    public GameObject GetBulletPrefab() => bulletPrefab;
}