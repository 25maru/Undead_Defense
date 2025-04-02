using System.Collections;
using UnityEngine;

public class ProductionBuilding : Building
{
    private float timer = 0f;
    private float cooldown = 5f;

    private int goldAmount = 1;
    private GameObject goldPrefab;

    private readonly float spawnRadius = 4f; // 생성 반경
    private readonly float minDistance = 2f; // 중심에서 너무 가까운 곳 방지

    public ProductionBuilding(int gold = 1) : base("Gold Mine", 80, 1.5f)
    {
        Type = BuildingType.Production;
        MaxHP = 80;
        CurrentHP = MaxHP;
        goldAmount = gold;
    }

    public override void Tick(BuildingLogicController controller)
    {
        if (LevelManager.Instance.Cycle.CurrentState == LevelCycle.CycleState.Day) return;

        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            timer = 0f;
            goldPrefab = controller.GetGoldPrefab();

            for (int i = 0; i < goldAmount; i++)
            {
                Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minDistance, spawnRadius);
                Vector3 offset = new(randomCircle.x, 0, randomCircle.y);
                Vector3 spawnPos = controller.transform.position + offset + Vector3.up * 0.25f;

                GameObject.Instantiate(goldPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();
        cooldown = Mathf.Max(2.5f, cooldown - 0.1f);
        goldAmount++;
    }

    public override void OnDestroyed(BuildingLogicController controller)
    {
        base.OnDestroyed(controller);
        Debug.Log("💥 타워 파괴됨!");
    }
}