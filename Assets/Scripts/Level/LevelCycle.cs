using System;
using UnityEngine;

/// <summary>
/// 낮/밤 주기를 관리하고 상태 변경 시 이벤트를 발생시키는 클래스입니다.
/// </summary>
public class LevelCycle : MonoBehaviour
{
    public enum CycleState { Day, Night }

    [Header("레벨 데이터")]
    [SerializeField] private LevelData levelData;

    private float timer;
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
        timer = 0f;
        currentState = CycleState.Day;
        OnDayStarted?.Invoke(currentDay);
    }

    private void Update()
    {
        if (currentState == CycleState.Day)
        {
            timer += Time.deltaTime;

            if (timer >= levelData.nightStartTime)
            {
                StartNight();
            }
        }
    }

    private void StartNight()
    {
        timer = 0f;
        currentState = CycleState.Night;
        Debug.Log($"밤 시작 - {currentDay}일차");
        OnNightStarted?.Invoke(currentDay);
    }

    private void StartDay()
    {
        timer = 0f;
        currentState = CycleState.Day;
        currentDay++;
        Debug.Log($"낮 시작 - {currentDay}일차");
        OnDayStarted?.Invoke(currentDay);
    }

    /// <summary>
    /// 외부에서 강제로 낮을 시작시킵니다 (예: 모든 적 처치 시 호출).
    /// </summary>
    public void ForceStartDay()
    {
        if (currentState == CycleState.Night)
        {
            StartDay();
        }
    }

    /// <summary>
    /// 현재 낮/밤 상태를 반환합니다.
    /// </summary>
    public CycleState CurrentState => currentState;

    /// <summary>
    /// 현재 날짜(며칠차)를 반환합니다.
    /// </summary>
    public int CurrentDay => currentDay;
}
