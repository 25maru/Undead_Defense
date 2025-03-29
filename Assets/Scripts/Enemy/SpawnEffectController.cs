using UnityEngine;
using DG.Tweening;

public class SpawnEffectController : MonoBehaviour
{
    [Header("렌더러")]
    [SerializeField] private Renderer characterRenderer;
    [SerializeField] private Renderer shieldRenderer;

    [Header("파티클파티클")]
    [SerializeField] private ParticleSystem spawnParticle;

    [Header("Tween Settings")]
    [SerializeField] private Ease transitionEase = Ease.OutQuad;
    [SerializeField] private float transitiobDuration = 1f;

    private Material characterMat;
    private Material shieldMat;

    private void Start()
    {
        characterMat = characterRenderer.material;
        shieldMat = shieldRenderer.material;

        if (spawnParticle != null)
        {
            spawnParticle.Play();
        }

        shieldMat.DOFloat(1f, "_Alpha", transitiobDuration)
            .From(0f)
            .SetEase(transitionEase);

        characterMat.DOFloat(1f, "_Dissolve", transitiobDuration)
            .From(0f)
            .SetEase(transitionEase);
    }
}
