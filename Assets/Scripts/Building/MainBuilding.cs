using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuilding : Building
{
    public MainBuilding() : base("MainBuilding", 1000, 5f)
    {
        MaxHP = 1000;
        CurrentHP = MaxHP;
        IsCritical = true;
    }

    public override void Tick(BuildingLogicController controller)
    {
        // 메인 건물은 특별한 행동을 하지 않음
    }
}