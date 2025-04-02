using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimationEvent : MonoBehaviour
{
    private Monster monster;

    private void Start()
    {
        if (monster == null)
        {
            monster = GetComponentInParent<Monster>();
        }
    }
    
    public void OnAttackHit()
    {
        if (monster != null)
        {
            monster.HitInDistance();
        }
    }
}
