using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : BaseUI
{
    [SerializeField] private Button stageButton;
    [SerializeField] private Button retryButton;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        stageButton.onClick.AddListener(OnClickStageButton);
        retryButton.onClick.AddListener(OnClickRetryButton);
    }
    public void OnClickStageButton()
    {
        UIManager.Instance.ChangeState(UIState.STAGE);
    }
    public void OnClickRetryButton()
    {
        UIManager.Instance.ChangeState(UIState.GAME);
        // 게임 초기화?
    }
    protected override UIState GetUIState()
    {
        return UIState.GAMEOVER;
    }
}
