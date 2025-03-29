using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBuilding : Building
{
    public float Range = 30f;
    public float Cooldown = 3f;
    private float timer;

    public DefensiveBuilding() : base("Tower", 100, 2f)
    {
        MaxHP = 100;
        CurrentHP = MaxHP;
    }

    public override void Tick(BuildingLogicController controller)
    {
        timer += Time.deltaTime;
        if (timer >= Cooldown)
        {
            Collider[] enemies = Physics.OverlapSphere(controller.transform.position, Range, LayerMask.GetMask("Enemy"));
            if (enemies.Length > 0)
            {
                Transform closest = null;
                float closestDist = Mathf.Infinity;

                foreach (var enemy in enemies)
                {
                    float dist = Vector3.Distance(controller.transform.position, enemy.transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = enemy.transform;
                    }
                }

                if (closest != null)
                {
                    Collider enemyCol = closest.GetComponent<Collider>();
                    Vector3 targetPoint = enemyCol != null ? enemyCol.bounds.center : closest.position;

                    // ✅ dir 먼저 계산
                    Vector3 dir = (targetPoint - controller.transform.position).normalized;

                    // ✅ spawnPos는 위로 + 방향으로 살짝 띄움
                    Vector3 spawnPos = controller.transform.position + dir + Vector3.up * 7f;

                    GameObject bullet = GameObject.Instantiate(controller.GetBulletPrefab(), spawnPos, Quaternion.identity);

                    Rigidbody rb = bullet.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.velocity = dir * 10f;
                        bullet.transform.rotation = Quaternion.LookRotation(dir);
                    }

                    Collider bulletCol = bullet.GetComponent<Collider>();
                    Collider towerCol = controller.GetComponent<Collider>();

                    BulletController bc = bullet.GetComponent<BulletController>();
                    if (bc != null)
                    {
                        bc.SetTarget(closest); // 적 Transform 전달
                    }

                    
                    if (bulletCol != null && towerCol != null)
                    {
                        Physics.IgnoreCollision(bulletCol, towerCol);
                    }

                    Debug.DrawLine(controller.transform.position, targetPoint, Color.red, 1f);
                    bullet.transform.forward = dir;
                }

                timer = 0;
            }
        }
    }
    
    public override void Upgrade()
    {
        base.Upgrade();

        Cooldown = Mathf.Max(0.5f, Cooldown - 0.3f); // 최대 속도 제한
    }


    public override void OnDestroyed(BuildingLogicController controller)
    {
        base.OnDestroyed(controller);
        Debug.Log("💥 타워 파괴됨!");
    }
}

