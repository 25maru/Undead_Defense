using UnityEngine;
using UnityEngine.UI;

public class GameClearUI : BaseUI
{
    [SerializeField] private Button stageButton;
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        stageButton.onClick.AddListener(OnClickStageButton);
    }
    public void OnClickStageButton()
    {
        UIManager.Instance.stageUI.UpdateUI();
        UIManager.Instance.ChangeState(UIState.STAGE);
    }
    protected override UIState GetUIState()
    {
        return UIState.GAMECLEAR;
    }
}
