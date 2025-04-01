using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TitleUI : BaseUI
{
    [SerializeField] private GameObject titleText;
    [SerializeField] private GameObject descPanel;
    
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private RectTransform leftDoor;
    [SerializeField] private RectTransform rightDoor;
    [SerializeField] private RectTransform ghost;

    readonly WaitForSeconds wait = new(1f);


    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        playButton.onClick.AddListener(OnClickPlayButton);
        quitButton.onClick.AddListener(OnClickQuitButton);

        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }


    private void Start()
    {
        MoveGhost();
    }

    private IEnumerator GateOpen()
    {
        descPanel.SetActive(false);

        leftDoor.DOAnchorPosX(-750, 1.2f)
            .SetUpdate(true);
        rightDoor.DOAnchorPosX(750, 1.2f)
            .SetUpdate(true);

        leftDoor.DORotate(new Vector3(0, -65, 0), 1f, RotateMode.WorldAxisAdd)
            .SetUpdate(true);
        rightDoor.DORotate(new Vector3(0, 65, 0), 1f, RotateMode.WorldAxisAdd)
            .SetUpdate(true);

        yield return wait;

        titleText.SetActive(true);

        yield return wait;

        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    void MoveGhost()
    {
        ghost.DOLocalMoveY(40, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true);
    }


    public IEnumerator Unlock(RectTransform key)
    {
        key.DOAnchorPos(new Vector3(-25, -380, 0), 1f)
            .SetUpdate(true);
        key.DORotate(new Vector3(180, -90, 115), 1f)
            .SetUpdate(true);

        yield return wait;

        StartCoroutine(GateOpen());
    }

    public void OnClickPlayButton()
    {
        UIManager.Instance.ChangeState(UIState.STAGE);
    }

    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    protected override UIState GetUIState()
    {
        return UIState.TITLE;
    }
}
