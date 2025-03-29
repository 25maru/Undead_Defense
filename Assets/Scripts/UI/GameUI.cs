using TMPro;
using UnityEngine;

public class GameUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI goldText;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }

    // action
    public void ChangeGold(int gold)
    {
        goldText.text = gold.ToString();
    }

    protected override UIState GetUIState()
    {
        return UIState.GAME;
    }
}
