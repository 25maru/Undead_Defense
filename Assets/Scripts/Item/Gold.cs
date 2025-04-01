using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] private int gold;
    
    [Header("Sound")]
    [SerializeField] private AudioClip clips;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.5f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ResourceManager.Instance.AddGold(gold);
            if(clips != null)
                AudioSource.PlayClipAtPoint(clips, transform.position, volume);
            
            Destroy(gameObject);
            
        }
    }
}
