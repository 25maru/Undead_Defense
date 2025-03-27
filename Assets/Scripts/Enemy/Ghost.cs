using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Monster
{
    
    [SerializeField] private SkinnedMeshRenderer[] MeshR;
    private float Dissolve_value = 1;
    
    
    void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDead()
    {
        base.OnDead();
        PlayerDissolve();
    }

    // 고스트 부서지면서 사라지는 효과
    private void PlayerDissolve ()
    {
        Dissolve_value -= Time.deltaTime;
        for(int i = 0; i < MeshR.Length; i++)
        {
            MeshR[i].material.SetFloat("_Dissolve", Dissolve_value);
        }
        if(Dissolve_value <= 0)
        {
            anim.enabled = false;
        }
    }
}
