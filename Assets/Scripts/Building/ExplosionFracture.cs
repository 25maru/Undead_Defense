using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplosionFracture : MonoBehaviour
{
    [SerializeField] private float minForce = 100f;
    [SerializeField] private float maxForce = 200f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float delayBeforeDestroy = 1f;
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] private int maxExplodingPieces = 20;

    [Header("사운드")]
    [SerializeField] private AudioClip clip;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.5f;

    private bool hasExploded = false;
    private int remainingAnimatedPieces = 0;

    private void Start()
    {
        Explode();
    }

    public void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        Rigidbody[] allPieces = GetComponentsInChildren<Rigidbody>();
        int total = allPieces.Length;

        List<int> selectedIndexes = new();
        while (selectedIndexes.Count < Mathf.Min(maxExplodingPieces, total))
        {
            int random = Random.Range(0, total);
            if (!selectedIndexes.Contains(random))
            {
                selectedIndexes.Add(random);
            }
        }

        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        }

        for (int i = 0; i < total; i++)
        {
            Rigidbody rb = allPieces[i];
            Transform part = rb.transform;

            if (selectedIndexes.Contains(i))
            {
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius);
                remainingAnimatedPieces++;

                DOVirtual.DelayedCall(Random.Range(0f, delayBeforeDestroy), () =>
                {
                    part.DOScale(Vector3.zero, scaleDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            Destroy(part.gameObject);
                            remainingAnimatedPieces--;
                            if (remainingAnimatedPieces <= 0)
                            {
                                Destroy(gameObject);
                            }
                        });
                });
            }
            else
            {
                Destroy(part.gameObject);
            }
        }

        // 예외 처리: 만약 maxExplodingPieces == 0 이거나 선택된 파편이이 없을 경우
        if (remainingAnimatedPieces == 0)
        {
            Destroy(gameObject);
        }
    }
}
