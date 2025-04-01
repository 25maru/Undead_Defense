using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    Transform target;
    float speed;
    float damage;
    public void Init(Transform target,float damage,float speed)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
    }
    private void Update()
    {
        if(target == null)
        {
            Destroy(this.gameObject);
        }
        OnHit();
    }
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
    void OnHit()
    {
        if(Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            target.GetComponent<Health>().OnDamaged(damage);
            Destroy(this.gameObject);  //오브젝트풀링 만들면 대체
        }
    }
}
