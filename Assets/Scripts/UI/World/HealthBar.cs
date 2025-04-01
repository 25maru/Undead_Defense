using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 적과 아군의 체력바 UI를 관리하는 클래스입니다.
/// 현재 체력 비율을 즉시 반응하는 체력바와, 일정 시간 후 부드럽게 줄어드는 체력바로 구성되어있습니다.
/// </summary>
public class HealthBar : WorldUIBase
{
    [Header("체력바")]
    [SerializeField] private Image mainBar;
    [SerializeField] private Image delayBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Tween")]
    [SerializeField] private Ease animEase = Ease.OutQuad;
    [SerializeField] private float delayTime = 0.5f;
    [SerializeField] private float animDuration = 0.5f;

    private float currentPercent = 1f;
    private Tween delayTween;
    private Coroutine delayRoutine;

    private void Start()
    {
        mainBar.fillAmount = currentPercent;
        delayBar.fillAmount = currentPercent;
    }

    /// <summary>
    /// 체력 상태를 0~1로 설정합니다.
    /// </summary>
    /// <param name="normalized">정규화된 체력 비율</param>
    public void SetHealth(float normalized)
    {
        normalized = Mathf.Clamp01(normalized);
        currentPercent = normalized;

        mainBar.fillAmount = normalized;

        if (Mathf.Approximately(normalized, 0f))
        {
            if (delayRoutine != null)
                StopCoroutine(delayRoutine);

            delayBar.DOFillAmount(currentPercent, animDuration)
                .SetEase(animEase);
            return;
        }

        if (delayRoutine != null)
        {
            StopCoroutine(delayRoutine);
        }
        if (delayTween != null && delayTween.IsActive())
        {
            delayTween.Kill();
        }

        delayRoutine = StartCoroutine(DelayedAnimate());

        if (healthText != null)
        {
            // TODO: 몬스터 스크립트와 체력바 사이의 연결 관계 조정 후 마저 구현현
            healthText.text = "";
        }
    }

    private IEnumerator DelayedAnimate()
    {
        yield return new WaitForSeconds(delayTime);

        delayTween = delayBar.DOFillAmount(currentPercent, animDuration)
            .SetEase(animEase);
    }
}
