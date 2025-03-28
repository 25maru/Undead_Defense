using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIState
{
    TITLE,
    STAGE,
    GAME,
    GAMECLEAR,
    GAMEOVER
}


public class UIManager : MonoSingleton<UIManager>
{
    TitleUI titleUI;
    StageUI stageUI;
    GameUI gameUI;
    GameClearUI gameClearUI;
    GameOverUI gameOverUI;

    UIState currentState;


    protected override void Awake()
    {
        base.Awake();

        titleUI = GetComponentInChildren<TitleUI>(true);
        titleUI.Init(this);
        stageUI = GetComponentInChildren<StageUI>(true);
        stageUI.Init(this);
        gameUI = GetComponentInChildren<GameUI>(true);
        gameUI.Init(this);
        gameClearUI = GetComponentInChildren<GameClearUI>(true);
        gameClearUI.Init(this);
        gameOverUI = GetComponentInChildren<GameOverUI>(true);
        gameOverUI.Init(this);

        ChangeState(UIState.TITLE);
    }

    public void SetStageUI()
    {
        ChangeState(UIState.STAGE);
    }
    public void SetGameUI()
    {
        ChangeState(UIState.GAME);
    }

    public void SetGameClearUI()
    {
        ChangeState(UIState.GAMECLEAR);
    }

    public void SetGameOverUI()
    {
        ChangeState(UIState.GAMEOVER);
    }

    public void ChangeState(UIState state)
    {
        currentState = state;
        titleUI.SetUIActive(currentState);
        stageUI.SetUIActive(currentState);
        gameUI.SetUIActive(currentState);
        gameClearUI.SetUIActive(currentState);
        gameOverUI.SetUIActive(currentState);
    }
}
