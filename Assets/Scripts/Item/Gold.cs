using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [Header("Gold Settings")]
    [SerializeField] private int gold = 1;

    [Header("Magnet Settings")]
    [SerializeField] private float magnetRange = 5f;            // 플레이어 감지 거리
    [SerializeField] private float magnetCancelRange = 7.5f;    // 너무 멀어지면 취소
    [SerializeField] private float baseMoveSpeed = 2.5f;        // 기본 이동 속도
    [SerializeField] private float speedMultiplier = 5f;        // 가까워질수록 속도 증가

    [Header("Sound")]
    [SerializeField] private AudioClip clip;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.5f;

    private Transform player;
    private bool isBeingAttracted = false;

    private void Start()
    {
        player = TestHomeTarget.Instance.Player;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (isBeingAttracted)
        {
            if (distance > magnetCancelRange)
            {
                isBeingAttracted = false;
                return;
            }

            float t = 1f - Mathf.Clamp01(distance / magnetRange);
            float moveSpeed = baseMoveSpeed + t * speedMultiplier;

            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += moveSpeed * Time.deltaTime * direction;
        }
        else
        {
            if (distance <= magnetRange)
            {
                isBeingAttracted = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResourceManager.Instance.AddGold(gold);

            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, transform.position, volume);
            }
            
            Destroy(gameObject);
        }
    }
}
