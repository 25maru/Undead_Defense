using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggeTest : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("적 트리거됨");

        if (other.gameObject.layer == layerMask)
        {
            player.targets.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Debug.Log("적 트리거 해제됨");

        if (other.gameObject.layer == layerMask)
        {
            player.targets.Remove(other.transform);
        }
    }
}
