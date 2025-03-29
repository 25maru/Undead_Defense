using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 적 스폰 위치에 표시되는 미리보기 UI 컴포넌트입니다.
/// </summary>
public class SpawnPreviewUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;

    private void Start()
    {
        canvas.worldCamera = Camera.main;
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
}
