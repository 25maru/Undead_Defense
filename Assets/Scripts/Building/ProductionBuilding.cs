using System.Collections;
using UnityEngine;

public class ProductionBuilding : Building
{
    private float timer = 0f;
    private float cooldown = 5f;

    private int goldAmount = 1;
    
    // 리소스 매니저 선언을 없애고, 싱글톤으로 가져오도록 수정했습니다.
    // private ResourceManager resourceManager;

    public ProductionBuilding(int gold = 1) : base("Gold Mine", 80, 1.5f)
    {
        Type = BuildingType.Production;
        MaxHP = 80;
        CurrentHP = MaxHP;
        goldAmount = gold;
    }

    public override void Tick(BuildingLogicController controller)
    {
        // 밤에만 작동하도록 수정했습니다. 충돌 시 이 코드도 포함해주세요!
        if (LevelManager.Instance.Cycle.CurrentState == LevelCycle.CycleState.Day) return;

        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            // 수정한 부분
            ResourceManager.Instance.AddGold(goldAmount);
            timer = 0f;
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

