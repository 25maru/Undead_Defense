using TMPro;
using UnityEngine;

public class GoldIndicator : WorldUIBase
{
    [SerializeField] private TextMeshProUGUI goldText;

    /// <summary>
    /// 건물 건설할 때 필요한 골드 수량 표시
    /// </summary>
    public void ChangeGold(int gold)
    {
        goldText.text = $"X {gold}";
    }
}
