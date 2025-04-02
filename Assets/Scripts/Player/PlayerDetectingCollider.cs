using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectingCollider : MonoBehaviour
{
    [SerializeField] Player player; 
    [SerializeField] SphereCollider detectingCollider;

    private void Awake()
    {
        detectingCollider.radius = player.detectingDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.TryGetComponent<Monster>(out Monster monster))
        {
            player.targets.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        player.targets.Remove(other.transform);
    }
}
