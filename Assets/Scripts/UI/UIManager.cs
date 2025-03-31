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
    [SerializeField] private TitleUI titleUi;
    [SerializeField] private StageUI stageUi;
    [SerializeField] private GameUI gameUi;
    [SerializeField] private GameClearUI gameClearUi;
    [SerializeField] private GameOverUI gameOverUi;

    public TitleUI titleUI { get; private set; }
    public StageUI stageUI { get; private set; }
    public GameUI gameUI{ get; private set; }
    public GameClearUI gameClearUI { get; private set; }
    public GameOverUI gameOverUI { get; private set; }

    UIState currentState;


    protected override void Awake()
    {
        base.Awake();

        titleUI = titleUi;
        stageUI = stageUi;
        gameUI = gameUi;
        gameClearUI = gameClearUi;
        gameOverUI = gameOverUi;

        titleUi.Init(this);
        stageUi.Init(this);
        gameUi.Init(this);
        gameClearUi.Init(this);
        gameOverUi.Init(this);

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
        titleUi.SetUIActive(currentState);
        stageUi.SetUIActive(currentState);
        gameUi.SetUIActive(currentState);
        gameClearUi.SetUIActive(currentState);
        gameOverUi.SetUIActive(currentState);
    }
}
