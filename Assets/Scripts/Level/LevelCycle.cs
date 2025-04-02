using System;
using UnityEngine;

/// <summary>
/// 낮/밤 주기를 관리하고 상태 변경 시 이벤트를 발생시키는 클래스입니다.
/// </summary>
public class LevelCycle : MonoBehaviour
{
    public enum CycleState { Day, Night }

    /// <summary>
    /// 현재 낮/밤 상태를 반환합니다.
    /// </summary>
    public CycleState CurrentState { get; private set; } = CycleState.Day;

    /// <summary>
    /// 현재 날짜를 반환합니다.
    /// </summary>
    public int CurrentDay { get; private set; } = 1;

    /// <summary>
    /// 밤에서 낮으로 전환될 때 호출됩니다.
    /// </summary>
    public event Action<int> OnDayStarted;

    /// <summary>
    /// 낮에서 밤으로 전환될 때 호출됩니다.
    /// </summary>
    public event Action OnNightStarted;

    private LevelData levelData;

    public void SetLevelData(LevelData data) => levelData = data;

    /// <summary>
    /// 레벨 시작 시 최초 낮 시작을 외부에서 명시적으로 호출해야 합니다.
    /// </summary>
    public void InvokeInitialDay()
    {
        CurrentState = CycleState.Day;
        OnDayStarted?.Invoke(CurrentDay);
    }

    /// <summary>
    /// 외부에서 낮 상태로 전환합니다.
    /// </summary>
    public void StartDay()
    {
        if (CurrentDay >= levelData.enemyWaves.Count)
        {
            UIManager.Instance.SetGameClearUI();
            return;
        }

        CurrentDay++;
        CurrentState = CycleState.Day;
        OnDayStarted?.Invoke(CurrentDay);
    }

    /// <summary>
    /// 밤으로 전환되었을 때 호출합니다.
    /// </summary>
    public void StartNight()
    {
        CurrentState = CycleState.Night;
        OnNightStarted?.Invoke();
    }

    /// <summary>
    /// 현재 날짜에 해당하는 적 웨이브 데이터를 반환합니다.
    /// </summary>
    public EnemyWaveData GetWaveDataForCurrentDay()
    {
        return levelData != null ? levelData.GetWaveByDay(CurrentDay) : null;
    }
}
