using System.Collections;
using UnityEngine;

public class ProductionBuilding : Building
{
    private float tickTimer = 0f;
    private float goldInterval = 2f;
    private int goldAmount = 10;
    
    private ResourceManager resourceManager;
    

    public ProductionBuilding() : base("Gold Mine", 80, 1.5f)
    {
        MaxHP = 80;
        CurrentHP = MaxHP;
        Type = BuildingType.Production;
    }

    public override void Tick(BuildingLogicController controller)
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= goldInterval)
        {
            resourceManager.AddGold(goldAmount);
            tickTimer = 0f;
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();
        goldAmount += 5;
        goldInterval -= 0.1f;
    }
}

