using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseUI
{
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button stageButton;

    [SerializeField] private TextMeshProUGUI goldText;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

        pauseButton.onClick.AddListener(OnClickPauseButton);
        continueButton.onClick.AddListener(OnClickContinueButton);
        stageButton.onClick.AddListener(OnClickStageButton);

        ResourceManager.Instance.OnGoldChanged += ChangeGold;

        pausePanel.SetActive(false);
    }

    public void OnClickPauseButton()
    {
        pauseButton.gameObject.SetActive(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnClickContinueButton()
    {
        pauseButton.gameObject.SetActive(true);
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnClickStageButton()
    {
        UIManager.Instance.stageUI.UpdateUI();
        UIManager.Instance.ChangeState(UIState.STAGE);
    }

    public void ChangeGold(int gold)
    {
        goldText.text = gold.ToString();
    }

    protected override UIState GetUIState()
    {
        return UIState.GAME;
    }
}
