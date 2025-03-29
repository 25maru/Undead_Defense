using UnityEngine;
using UnityEngine.UI;

public class StageUI : BaseUI
{
    [SerializeField] private Button firstStageButton;
    [SerializeField] private Button secondStageButton;
    [SerializeField] private Button thirdStageButton;
    [SerializeField] private Button fourthStageButton;
    // stage info?


    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        firstStageButton.onClick.AddListener(OnClickfirstStageButton);
        secondStageButton.onClick.AddListener(OnClickSecondStageButton);
        thirdStageButton.onClick.AddListener(OnClickThirdStageButton);
        fourthStageButton.onClick.AddListener(OnClickFourthStageButton);
    }

    public void OnClickfirstStageButton()
    {
        UIManager.Instance.ChangeState(UIState.GAME);
    }

    public void OnClickSecondStageButton()
    {
        UIManager.Instance.ChangeState(UIState.GAME);
    }

    public void OnClickThirdStageButton()
    {
        UIManager.Instance.ChangeState(UIState.GAME);
    }

    public void OnClickFourthStageButton()
    {
        UIManager.Instance.ChangeState(UIState.GAME);
    }

    protected override UIState GetUIState()
    {
        return UIState.STAGE;
    }
}
