using System.Collections.Generic;
using UnityEngine;

public class SpawnerBuilding : Building
{
    private float timer;
    private int currentPrefabIndex = 0; // 👉 현재 순서용 인덱스

    private readonly int MaxSoldierCount = 10;
    private readonly float Cooldown = 3f;

    private List<GameObject> spawnedUnits = new List<GameObject>();
    private List<GameObject> soldierPrefabs;

    public SpawnerBuilding(List<GameObject> unitPrefabs) : base("Spawner", 500, 3f)
    {
        Type = BuildingType.Spawner;
        MaxHP = 300;
        CurrentHP = MaxHP;
        soldierPrefabs = unitPrefabs;
    }

    public override void Tick(BuildingLogicController controller)
    {
        timer += Time.deltaTime;

        if (timer >= Cooldown)
        {
            CleanUpDeadUnits();

            if (spawnedUnits.Count < MaxSoldierCount && soldierPrefabs.Count > 0)
            {
                // 순차적으로 프리팹 선택
                GameObject selectedPrefab = soldierPrefabs[currentPrefabIndex];

                float spawnDistance = 4f; // 건물 앞 4m
                Vector3 forward = controller.transform.forward;
                Vector3 spawnPos = controller.transform.position + forward * spawnDistance + Vector3.up * 1f;

                GameObject unit = GameObject.Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
                spawnedUnits.Add(unit);

                Debug.Log($"🪖 유닛 생성됨 ({spawnedUnits.Count}/{MaxSoldierCount}) → {selectedPrefab.name}");

                // 다음 인덱스로 이동 (반복)
                currentPrefabIndex = (currentPrefabIndex + 1) % soldierPrefabs.Count;
            }

            timer = 0f;
        }
    }

    private void CleanUpDeadUnits()
    {
        spawnedUnits.RemoveAll(unit => unit == null);
    }
}