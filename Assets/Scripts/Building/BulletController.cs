using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private GameObject crackPrefab;

    private Transform target;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPos = target.position;
        Collider targetCol = target.GetComponent<Collider>();
        if (targetCol != null)
            targetPos = targetCol.bounds.center;

        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        transform.forward = dir; // 방향 맞춰주면 충돌 박음
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (collision.gameObject.TryGetComponent(out Monster enemy))
            {
                enemy.OnHit(damage);
            }

            if (crackPrefab != null)
            {
                Instantiate(crackPrefab, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}