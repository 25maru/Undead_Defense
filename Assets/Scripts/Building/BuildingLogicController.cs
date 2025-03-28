using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLogicController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject upgradedPrefab;
    private Building building;
    private bool isBuilt = false;

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
        if (building == null) return;

        building.Upgrade();

        if (upgradedPrefab != null)
        {
            GameObject newObj = Instantiate(upgradedPrefab, transform.position, transform.rotation);
            var newController = newObj.GetComponent<BuildingLogicController>();
            if (newController != null)
            {
                newController.Initialize(building); // 기존 상태 유지
            }

            Destroy(gameObject); // 기존 오브젝트 제거
        }
    }


    public void TakeDamage(int amount)
    {
        building?.TakeDamage(amount, this);
    }

    public GameObject GetBulletPrefab() => bulletPrefab;
}