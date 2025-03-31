using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class SpawnEffectController : MonoBehaviour
{
    [Header("렌더러")]
    [SerializeField] private Renderer characterRenderer;
    [SerializeField] private Renderer shieldRenderer;

    [Header("파티클")]
    [SerializeField] private Transform particleRoot;
    [SerializeField] private ParticleSystem spawnParticle;

    [Header("Tween 설정")]
    [SerializeField] private Ease transitionEase = Ease.OutQuad;
    [SerializeField] private float transitionDelay = 0.5f;
    [SerializeField] private float transitionDuration = 1f;

    private Monster monster;
    private NavMeshAgent agent;
    private Material characterMat;
    private Material shieldMat;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (monster != null) monster.enabled = false;
        if (agent != null) agent.enabled = false;

        characterMat = characterRenderer.material;
        characterMat.SetFloat("_Dissolve", 0f);

        if (shieldRenderer != null)
        {
            shieldMat = shieldRenderer.material;
            shieldMat.SetFloat("_Alpha", 0f);
        }

        if (spawnParticle != null)
        {
            spawnParticle.Play();
        }

        DOVirtual.DelayedCall(transitionDelay, () =>
        {
            if (agent != null) agent.enabled = true;
            if (monster != null) monster.enabled = true;

            characterMat.DOFloat(1f, "_Dissolve", transitionDuration)
                .SetEase(transitionEase);

            if (shieldRenderer != null)
            {
                shieldMat.DOFloat(1f, "_Alpha", transitionDuration)
                    .SetEase(transitionEase);
            }
        });
    }

    private void LateUpdate()
    {
        particleRoot.rotation = Quaternion.Euler(Vector3.zero);
    }
}
