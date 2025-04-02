using TMPro;    
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

/// <summary>
/// 플레이어가 밤 시작 트리거 영역에 진입하고, 일정 시간 스페이스바를 누르고 있으면 밤이 시작됩니다.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class NightStartTrigger : MonoBehaviour
{
    [Header("트리거 영역")]
    [SerializeField] private SphereCollider triggerZone;
    [SerializeField] private float radius = 20f;

    [Header("입력 설정")]
    [SerializeField] private KeyCode holdKey = KeyCode.Space;
    [SerializeField] private float holdDuration = 2f;

    [Header("UI")]
    [SerializeField] private GameObject nightUIRoot;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Image radialFillImage;

    [Header("낮 / 밤")]
    [SerializeField] private Volume globalVolume;
    [SerializeField] private Light directionalLight;
    [SerializeField] private Ease transitionEase = Ease.OutQuad;
    [SerializeField] private float transitionDuration = 1f;

    [Header("디버그 (테스트용)")]
    [SerializeField] private bool testMode;

    private LevelCycle levelCycle;

    private ColorAdjustments colorAdjustments;
    private readonly float dayExposure = 0.6f;
    private readonly float nightExposure = 0f;

    private readonly float lightDayXRotation = 50f;
    private readonly float lightNightXRotation = 230f;

    private bool inputBlocked;
    private bool hasReleasedKeySinceBlock = true;

    private bool playerInZone = false;
    private bool isHolding = false;
    private float holdTimer = 0f;

    private void Start()
    {
        levelCycle = LevelManager.Instance.Cycle;
        nightUIRoot.SetActive(false);

        if (!globalVolume.profile.TryGet(out colorAdjustments))
        {
            Debug.LogWarning("NightStartTrigger: ColorAdjustments가 Volume 프로파일에 없습니다.");
        }
    }

    private void Update()
    {
        if (testMode && playerInZone && levelCycle.CurrentState != LevelCycle.CycleState.Day)
        {
            if (Input.GetKey(holdKey))
            {
                if (!isHolding)
                {
                    isHolding = true;
                    nightUIRoot.SetActive(true);
                }

                holdTimer += Time.deltaTime;
                float remain = Mathf.Clamp(holdDuration - holdTimer, 0f, holdDuration);

                countdownText.text = $"DAY IN  {remain:F1}s";
                radialFillImage.fillAmount = 1f - remain / holdDuration;

                if (holdTimer >= holdDuration)
                {
                    levelCycle.StartDay();
                    ResetHold();
                }
            }
            else if (isHolding)
            {
                ResetHold();
            }
        }

        if (!playerInZone || inputBlocked || levelCycle.CurrentState != LevelCycle.CycleState.Day) return;

        if (Input.GetKey(holdKey))
        {
            if (!hasReleasedKeySinceBlock) return;

            if (!isHolding)
            {
                isHolding = true;
                nightUIRoot.SetActive(true);
            }

            holdTimer += Time.deltaTime;
            float remain = Mathf.Clamp(holdDuration - holdTimer, 0f, holdDuration);

            countdownText.text = $"NIGHT IN  {remain:F1}s";
            radialFillImage.fillAmount = 1f - remain / holdDuration;

            if (holdTimer >= holdDuration)
            {
                levelCycle.StartNight();
                ResetHold();
                hasReleasedKeySinceBlock = false;
            }
        }
        else
        {
            ResetHold();
            hasReleasedKeySinceBlock = true;
        }
    }

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0f;
        nightUIRoot.SetActive(false);
        radialFillImage.fillAmount = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            ResetHold();
        }
    }

    /// <summary>
    /// 낮 전환 시 조명 회전 연출과 볼륨 노출값을 복원합니다.
    /// </summary>
    public void PlayDayTransition(System.Action onComplete)
    {
        if (colorAdjustments != null)
        {
            DOTween.To(() => colorAdjustments.postExposure.value,
                v => colorAdjustments.postExposure.value = v,
                dayExposure,
                transitionDuration);
        }

        if (directionalLight == null)
        {
            onComplete?.Invoke();
            return;
        }

        Vector3 currentRotation = directionalLight.transform.eulerAngles;
        Vector3 targetRotation = new(lightDayXRotation, currentRotation.y, currentRotation.z);

        directionalLight.transform.DOLocalRotate(targetRotation, transitionDuration)
            .SetEase(transitionEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    /// <summary>
    /// 밤 전환 시 조명 회전 연출과 볼륨 노출값을 낮춥니다.
    /// </summary>
    public void PlayNightTransition(System.Action onComplete)
    {
        if (colorAdjustments != null)
        {
            DOTween.To(() => colorAdjustments.postExposure.value,
                v => colorAdjustments.postExposure.value = v,
                nightExposure,
                transitionDuration);
        }

        if (directionalLight == null)
        {
            onComplete?.Invoke();
            return;
        }

        Vector3 currentRotation = directionalLight.transform.eulerAngles;
        Vector3 targetRotation = new(lightNightXRotation, currentRotation.y, currentRotation.z);

        directionalLight.transform.DOLocalRotate(targetRotation, transitionDuration)
            .SetEase(transitionEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    /// <summary>
    /// 외부에서 입력 차단 여부를 설정합니다.
    /// </summary>
    public void BlockInput(bool block)
    {
        inputBlocked = block;

        if (block)
        {
            hasReleasedKeySinceBlock = false;
            ResetHold();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}