using UnityEngine;
using UnityEngine.UI;

public class TitleUI : BaseUI
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        playButton.onClick.AddListener(OnClickPlayButton);
        quitButton.onClick.AddListener(OnClickQuitButton);
    }

    public void OnClickPlayButton()
    {
        UIManager.Instance.ChangeState(UIState.STAGE);
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    protected override UIState GetUIState()
    {
        return UIState.TITLE;
    }
}
