using System;
using UnityEngine;

/// <summary>
/// 낮/밤 주기를 관리하고 상태 변경 시 이벤트를 발생시키는 클래스입니다.
/// </summary>
public class LevelCycle : MonoBehaviour
{
    public enum CycleState { Day, Night }

    private LevelData levelData;

    private int currentDay = 1;
    private CycleState currentState = CycleState.Day;

    /// <summary>
    /// 낮에서 밤으로 전환될 때 호출됩니다.
    /// </summary>
    public event Action<int> OnNightStarted;

    /// <summary>
    /// 밤에서 낮으로 전환될 때 호출됩니다.
    /// </summary>
    public event Action<int> OnDayStarted;

    private void Start()
    {
        if (levelData == null)
        {
            Debug.LogError("LevelCycle: LevelData가 설정되지 않았습니다. LevelManager에서 SetLevelData()를 호출하세요.");
            enabled = false;
            return;
        }

        currentState = CycleState.Day;
        OnDayStarted?.Invoke(currentDay);
    }

    private void Update()
    {
        if (currentState == CycleState.Day && Input.GetKey(KeyCode.Space))
        {
            StartNight();
        }
    }

    private void StartNight()
    {
        currentState = CycleState.Night;
        Debug.Log($"밤 시작 - {currentDay}일차");
        OnNightStarted?.Invoke(currentDay);
    }

    private void StartDay()
    {
        currentState = CycleState.Day;
        currentDay++;
        Debug.Log($"낮 시작 - {currentDay}일차");
        OnDayStarted?.Invoke(currentDay);
    }

    /// <summary>
    /// 외부에서 LevelData를 설정합니다 (예: LevelManager에서 호출).
    /// </summary>
    public void SetLevelData(LevelData data)
    {
        levelData = data;
    }

    /// <summary>
    /// 외부에서 강제로 낮을 시작시킵니다. (예: 모든 적 처치 시 호출)
    /// </summary>
    public void ForceStartDay()
    {
        if (currentState == CycleState.Night)
        {
            StartDay();
        }
    }

    /// <summary>
    /// 외부에서 강제로 밤을 시작시킵니다. (예: 밤 진입 트리거)
    /// </summary>
    public void ForceStartNight()
    {
        if (currentState == CycleState.Day)
        {
            StartNight();
        }
    }

    /// <summary>
    /// 현재 낮/밤 상태를 반환합니다.
    /// </summary>
    public CycleState CurrentState => currentState;

    /// <summary>
    /// 현재 날짜를 반환합니다.
    /// </summary>
    public int CurrentDay => currentDay;

    /// <summary>
    /// 현재 일차에 해당하는 웨이브 정보를 가져옵니다.
    /// </summary>
    public EnemyWaveData GetWaveDataForCurrentDay()
    {
        return levelData.GetWaveByDay(currentDay);
    }
}
