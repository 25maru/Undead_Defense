using TMPro;    
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 플레이어가 밤 시작 트리거 영역에 진입하고, 일정 시간 스페이스바를 누르고 있으면 밤이 시작됩니다.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class NightStartTrigger : MonoBehaviour
{
    [Header("Hold Time 설정")]
    [SerializeField] private float holdDuration = 2f;

    [Header("UI 구성요소")]
    [SerializeField] private GameObject nightUIRoot;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Image radialFillImage;

    [Header("낮 / 밤")]
    [SerializeField] private Light directionalLight;

    private float holdTimer = 0f;
    private bool playerInZone = false;
    private bool isHolding = false;

    private float lightDayXRotation = 50f;
    private float lightNightXRotation = 230f;

    private LevelCycle levelCycle;

    private void Start()
    {
        levelCycle = LevelManager.Instance.Cycle;
        nightUIRoot.SetActive(false);
    }

    private void Update()
    {
        if (!playerInZone || levelCycle.CurrentState != LevelCycle.CycleState.Day) return;

        if (Input.GetKey(KeyCode.Space))
        {
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
                DOTween.Kill(directionalLight);
                directionalLight.transform.DORotate(new Vector3(lightNightXRotation, 0f, 0f), 1f)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        levelCycle.ForceStartNight();
                        ResetHold();
                    });

                nightUIRoot.SetActive(false);
            }
        }
        else if (isHolding)
        {
            ResetHold();
        }
    }

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0f;
        nightUIRoot.SetActive(false);
        countdownText.text = string.Empty;
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
    /// 낮으로 회전 애니메이션 (레벨 사이클이 낮이 될 때 호출하도록)
    /// </summary>
    public void RotateToDayLight()
    {
        DOTween.Kill(directionalLight);
        directionalLight.transform.DORotate(new Vector3(lightDayXRotation, 0f, 0f), 1f)
            .SetEase(Ease.InOutSine);
    }
}