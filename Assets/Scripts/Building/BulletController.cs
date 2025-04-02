using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float damage = 10;
    [SerializeField] private GameObject crackPrefab;

    private Transform target;
    private Vector3 lastDirection;
    private readonly List<Monster> hitTarget = new();

    public void SetDamage(float newDamage) => damage = newDamage;
    public void SetTarget(Transform t)
    {
        target = t;
        if (target != null)
        {
            lastDirection = GetDirectionToTarget();
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (target != null)
        {
            lastDirection = GetDirectionToTarget();
            Quaternion targetRotation = Quaternion.LookRotation(lastDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        transform.position += speed * Time.deltaTime * lastDirection;
    }

    private Vector3 GetDirectionToTarget()
    {
        Vector3 targetPos = target.position;
        if (target.TryGetComponent<Collider>(out var targetCol))
            targetPos = targetCol.bounds.center;

        return (targetPos - transform.position).normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            if (collision.gameObject.TryGetComponent(out Monster enemy))
            {
                enemy.OnHit(damage);

                // foreach (Monster monster in hitTarget)
                // {
                //     monster.OnHit(damage);
                // }
            }

            if (crackPrefab != null)
            {
                Instantiate(crackPrefab, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //     {
    //         if (other.TryGetComponent(out Monster enemy))
    //         {
    //             hitTarget.Add(enemy);
    //         }
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //     {
    //         if (other.TryGetComponent(out Monster enemy))
    //         {
    //             hitTarget.Remove(enemy);
    //         }
    //     }
    // }
}