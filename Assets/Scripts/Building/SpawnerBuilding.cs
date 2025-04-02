using System.Collections.Generic;
using UnityEngine;

public class SpawnerBuilding : Building
{
    private float timer;
    private float cooldown = 4f;

    private int maxSoldierCount = 4;
    private readonly List<GameObject> soldierPrefabs;

    private readonly List<GameObject> spawnedUnits = new();
    private int currentPrefabIndex = 0; // 현재 순서 저장용 인덱스

    public SpawnerBuilding(List<GameObject> unitPrefabs, int unitCount = 4) : base("Spawner", 500, 3f)
    {
        Type = BuildingType.Spawner;
        MaxHP = 300;
        CurrentHP = MaxHP;
        soldierPrefabs = unitPrefabs;
        maxSoldierCount = unitCount;
    }

    public override void Tick(BuildingLogicController controller)
    {
        // 밤에만 작동하도록 수정했습니다. 충돌 시 이 코드도 포함해주세요!
        if (LevelManager.Instance.Cycle.CurrentState == LevelCycle.CycleState.Day) return;

        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            CleanUpDeadUnits();

            if (spawnedUnits.Count < maxSoldierCount && soldierPrefabs.Count > 0)
            {
                // 순차적으로 프리팹 선택
                GameObject selectedPrefab = soldierPrefabs[currentPrefabIndex];

                float spawnDistance = 4f; // 건물 앞 4m
                Vector3 forward = controller.transform.forward;
                Vector3 spawnPos = controller.transform.position + forward * spawnDistance + Vector3.up * 1f;

                GameObject unit = GameObject.Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
                spawnedUnits.Add(unit);

                Debug.Log($"🪖 유닛 생성됨 ({spawnedUnits.Count}/{maxSoldierCount}) → {selectedPrefab.name}");

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

    public override void Upgrade()
    {
        base.Upgrade();
        cooldown = Mathf.Max(2f, cooldown - 0.2f);
        maxSoldierCount++;
    }

    public override void OnDestroyed(BuildingLogicController controller)
    {
        base.OnDestroyed(controller);
        Debug.Log("💥 타워 파괴됨!");
    }
}