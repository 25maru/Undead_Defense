using UnityEngine;
using DG.Tweening;

public class BarrierUI : WorldUIBase
{
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Tween")]
    [SerializeField] private Ease transitionEase = Ease.OutQuad;
    [SerializeField] private float transitionDuration = 0.5f;

    private bool playerInZone;

    private void Start()
    {
        canvasGroup.alpha = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerInZone)
            {
                playerInZone = true;

                DOTween.Kill(canvasGroup);
                canvasGroup.DOFade(1f, transitionDuration)
                    .SetEase(transitionEase);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerInZone)
            {
                playerInZone = false;

                DOTween.Kill(canvasGroup);
                canvasGroup.DOFade(0f, transitionDuration)
                    .SetEase(transitionEase);
            }
        }
    }
}
