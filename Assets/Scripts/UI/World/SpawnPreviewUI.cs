using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 적 스폰 위치에 표시되는 미리보기 UI 컴포넌트입니다.
/// </summary>
public class SpawnPreviewUI : WorldUIBase
{
    [Header("UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;

    [Header("Tween")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Ease transitionEase = Ease.OutQuad;
    [SerializeField] private float transitionDuration = 0.5f;

    private void Start()
    {
        canvasGroup.alpha = 0f;
    }

    /// <summary>
    /// 아이콘과 수량 정보를 설정합니다.
    /// </summary>
    public void SetInfo(int count, Sprite icon)
    {
        if (iconImage != null)
            iconImage.sprite = icon;

        if (countText != null)
            countText.text = count.ToString();
    }

    /// <summary>
    /// 미리보기 UI의 표시 여부를 설정합니다.
    /// </summary>
    public void SetPreviewVisible(bool visible, System.Action onComplete = null)
    {
        DOTween.Kill(canvasGroup);

        canvasGroup.DOFade(visible ? 1f : 0f, transitionDuration)
            .SetEase(transitionEase)
            .OnComplete(() => onComplete?.Invoke());
    }
}
