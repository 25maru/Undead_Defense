using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building
{
    public string Name;
    public int Level = 1;
    public int BuildCost = 100;
    public float BuildTime = 2f;

    public int MaxHP = 100;
    public int CurrentHP;
    public bool IsCritical = false;
    
    public BuildingType Type { get; protected set; }

    public Building(string name, int cost, float buildTime = 2f)
    {
        Name = name;
        BuildCost = cost;
        BuildTime = buildTime;
        CurrentHP = MaxHP;
    }

    public virtual void Upgrade()
    {
        Level++;
        BuildCost += 50;
        MaxHP += 50;
        CurrentHP = MaxHP;
    }

    public virtual void TakeDamage(int damage, BuildingLogicController controller)
    {
        CurrentHP -= damage;
        Debug.Log($"{Name} 피해 입음: {damage} → 현재 HP: {CurrentHP}");

        if (CurrentHP <= 0)
        {
            OnDestroyed(controller);
        }
    }

    public virtual void OnDestroyed(BuildingLogicController controller)
    {
        Debug.Log($"{Name} 파괴됨!");

        if (IsCritical)
        {
            // GameManager.Instance.GameOver();
        }

        GameObject.Destroy(controller.gameObject);
    }

    public abstract void Tick(BuildingLogicController controller); // 생산/공격 처리용
}