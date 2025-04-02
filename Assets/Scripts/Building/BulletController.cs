using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float damage = 10;
    [SerializeField] private GameObject crackPrefab;

    private Transform target;

    public void SetDamage(float newDamage) => damage = newDamage;
    public void SetTarget(Transform t) => target = t;

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
        if (target.TryGetComponent<Collider>(out var targetCol))
            targetPos = targetCol.bounds.center;

        Vector3 dir = (targetPos - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(dir);

        // 이동, 회전 코드 리펙터링 했습니다. (rotateSpeed 활용)
        transform.SetPositionAndRotation(
            position: transform.position + speed * Time.deltaTime * transform.forward,
            rotation: Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime)
        );
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