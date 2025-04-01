using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    Player player;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileStartPosition;
    
    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    public void Attack()
    {
        ProjectileController controller = Instantiate(projectilePrefab, projectileStartPosition.position, Quaternion.LookRotation(player.target.position - transform.position)).GetComponent<ProjectileController>();
        controller.Init(player.target, player.damage, player.projectileSpeed);
    }
}
