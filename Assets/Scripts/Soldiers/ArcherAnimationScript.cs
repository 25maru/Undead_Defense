using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimationScript : MonoBehaviour
{
    Soldier soldier;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileStartPosition;

    private void Awake()
    {
        soldier = GetComponentInParent<Soldier>();
    }
    public void Attack()
    {
        ProjectileController controller = Instantiate(projectilePrefab, projectileStartPosition.position, Quaternion.LookRotation(soldier.target.position - transform.position)).GetComponent<ProjectileController>();
        controller.Init(soldier.target, soldier.damage, soldier.projectileSpeed);
    }
}
