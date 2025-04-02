using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stage
{
    public int stageNumber;
    public bool isCleared;
    public Button button;
    public Image image;
}


public class StageUI : BaseUI
{
    [SerializeField] private List<Stage> stages;

    [SerializeField] private Button firstStageButton;
    [SerializeField] private Button secondStageButton;
    [SerializeField] private Button thirdStageButton;


    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        firstStageButton.onClick.AddListener(OnClickfirstStageButton);
        secondStageButton.onClick.AddListener(OnClickSecondStageButton);
        thirdStageButton.onClick.AddListener(OnClickThirdStageButton);
    }

    private void Start()
    {
        UpdateUI();
    }

    /// <summary>
    /// 스테이지 클리어 할 경우 UI 업데이트
    /// </summary>

    public void UpdateUI()
    {
        foreach (Stage stage in stages)
        {
            stage.image.color = stage.isCleared ? new Color(1f, 1f, 1f) : new Color(85f / 255f, 85f / 255f, 85f / 255f);
            stage.button.interactable = stage.isCleared;
            stage.button.image.color = stage.isCleared ? stage.button.colors.normalColor : Color.gray;
        }
    }

    // Button
    public void OnClickfirstStageButton()
    {
        UIManager.Instance.ChangeState(UIState.GAME);
    }

    public void OnClickSecondStageButton()
    {
        UIManager.Instance.ChangeState(UIState.GAME);       // temp
    }

    public void OnClickThirdStageButton()
    {
        UIManager.Instance.ChangeState(UIState.GAME);       // temp
    }

    protected override UIState GetUIState()
    {
        return UIState.STAGE;
    }
}
