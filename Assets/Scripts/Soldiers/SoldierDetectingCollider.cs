using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierDetectingCollider : MonoBehaviour
{
    [SerializeField] Soldier soldier;
    [SerializeField] SphereCollider detectingCollider;

    private void Awake()
    {
        detectingCollider.radius = soldier.detectingDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<Monster>(out Monster monster))
        {
            soldier.targets.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        soldier.targets.Remove(other.transform);
    }
}
