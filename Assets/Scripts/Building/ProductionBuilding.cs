using System.Collections;
using UnityEngine;

public class ProductionBuilding : Building
{
    private float tickTimer = 0f;
    private float goldInterval = 2f;
    private int goldAmount = 10;

    public ProductionBuilding() : base("Gold Mine", 80, 1.5f)
    {
        MaxHP = 80;
        CurrentHP = MaxHP;
    }

    public override void Tick(BuildingLogicController controller)
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= goldInterval)
        {
            // GameManager.Instance.AddGold(goldAmount);
            tickTimer = 0f;
            Debug.Log("💰 골드 획득 +10");
        }
    }
    
    public override void OnDestroyed(BuildingLogicController controller)
    {
        base.OnDestroyed(controller);
        Debug.Log("💥 타워 파괴됨!");
    }
}

