using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLogicController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject upgradedPrefab;
    private Building building;
    private bool isBuilt = false;
    
    [Header("건설 효과 프리팹 (파티클 등)")]
    [SerializeField] private GameObject constructionEffectPrefab;

    public void Initialize(Building buildingData)
    {
        building = buildingData;
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
            newController.Initialize(building); // 상태 넘겨줌
        }

        Debug.Log("✅ 업그레이드 완료!");
        Destroy(gameObject); // 현재 오브젝트 제거
    }
    
    public void TakeDamage(int amount)
    {
        building?.TakeDamage(amount, this);
    }

    public GameObject GetBulletPrefab() => bulletPrefab;
}