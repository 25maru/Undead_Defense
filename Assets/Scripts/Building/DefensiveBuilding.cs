using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBuilding : Building
{
    private float timer;
    private float cooldown = 2.5f;

    public float attackRange = 20f;

    private float realDamage = 0f;
    private readonly Collider[] enemyBuffer = new Collider[50];

    public DefensiveBuilding(float damage = 50f, float range = 20f) : base("Tower", 100, 2f)
    {
        Type = BuildingType.Defense;
        MaxHP = 100;
        CurrentHP = MaxHP;
        realDamage = damage;
        attackRange = range;
    }

    public override void Tick(BuildingLogicController controller)
    {
        // 밤에만 작동하도록 수정했습니다. 충돌 시 이 코드도 포함해주세요!
        if (LevelManager.Instance.Cycle.CurrentState == LevelCycle.CycleState.Day) return;
        
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            int enemyCount = Physics.OverlapSphereNonAlloc(
                controller.transform.position,
                attackRange,
                enemyBuffer,
                LayerMask.GetMask("Enemy")
            );

            if (enemyCount > 0)
            {
                Transform closest = null;
                float closestDistSqr = Mathf.Infinity;
                Vector3 towerPos = controller.transform.position;

                for (int i = 0; i < enemyCount; i++)
                {
                    Collider col = enemyBuffer[i];
                    Vector3 toEnemy = col.transform.position - towerPos;
                    float distSqr = toEnemy.sqrMagnitude;

                    if (distSqr < closestDistSqr)
                    {
                        closestDistSqr = distSqr;
                        closest = col.transform;
                    }
                }

                if (closest != null)
                {
                    // 타겟 위치 계산 (Collider bounds 고려)
                    Vector3 targetPoint = closest.TryGetComponent(out Collider enemyCol)
                        ? enemyCol.bounds.center
                        : closest.position;

                    // 방향 먼저 계산
                    Vector3 dir = (targetPoint - controller.transform.position).normalized;

                    // spawnPos는 위로 살짝 띄움
                    Vector3 spawnPos = controller.transform.position + dir + Vector3.up * 7f;

                    GameObject bullet = GameObject.Instantiate(
                        original: controller.GetBulletPrefab(),
                        position: spawnPos,
                        rotation: Quaternion.LookRotation(dir)
                    );

                    if (bullet.TryGetComponent(out Rigidbody rb))
                        rb.velocity = dir * 10f;

                    if (bullet.TryGetComponent(out BulletController bc))
                    {
                        bc.SetDamage(realDamage);
                        bc.SetTarget(closest);
                    }

                    if (bullet.TryGetComponent(out Collider bulletCol) &&
                        controller.TryGetComponent(out Collider towerCol))
                        Physics.IgnoreCollision(bulletCol, towerCol);

                    Debug.DrawLine(towerPos, targetPoint, Color.red, 1f);
                }

                timer = 0;
            }
        }
    }
    
    public override void Upgrade()
    {
        base.Upgrade();
        cooldown = Mathf.Max(1f, cooldown - 0.5f); // 최대 속도 제한
        attackRange = Mathf.Min(50f, attackRange + 5);
        realDamage = Mathf.Min(500, realDamage * 2f);
    }

    public override void OnDestroyed(BuildingLogicController controller)
    {
        base.OnDestroyed(controller);
        Debug.Log("💥 타워 파괴됨!");
    }
}
