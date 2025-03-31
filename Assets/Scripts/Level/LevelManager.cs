using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레벨 전체 흐름을 관리하는 매니저 클래스입니다.
/// 낮/밤 전환 이벤트를 수신하고 적 웨이브를 소환합니다.
/// 모든 적이 처치되면 밤이 종료되고 낮으로 전환됩니다.
/// </summary>
public class LevelManager : MonoSingleton<LevelManager>
{
    [Header("레벨 구성 요소")]
    [SerializeField] private LevelData levelData;
    [SerializeField] private LevelCycle levelCycle;

    [Header("씬에 배치된 스폰 포인트들")]
    [SerializeField] private List<SpawnPoint> sceneSpawnPoints;

    [Header("낮 / 밤 전환 연출")]
    [SerializeField] private NightStartTrigger nightTrigger;

    /// <summary>
    /// 외부 접근을 위한 Cycle 프로퍼티
    /// </summary>
    public LevelCycle Cycle => levelCycle;

    private int enemiesAlive = 0;

    protected override void Awake()
    {
        base.Awake();
        levelCycle.SetLevelData(levelData);
    }

    private void Start()
    {
        levelCycle.OnNightStarted += HandleNightStarted;
        levelCycle.OnDayStarted += HandleDayStarted;

        Debug.Log("LevelManager: 레벨 시작됨");
        levelCycle.InvokeInitialDay();
    }

    private void OnDestroy()
    {
        if (levelCycle != null)
        {
            levelCycle.OnNightStarted -= HandleNightStarted;
            levelCycle.OnDayStarted -= HandleDayStarted;
        }
    }

    /// <summary>
    /// 낮 상태 진입 처리. 연출 후 낮 관련 세팅 수행.
    /// </summary>
    private void HandleDayStarted(int day)
    {
        Debug.Log($"LevelManager: {day}일차 낮 시작됨 → 연출 및 프리뷰 표시");

        if (nightTrigger != null)
        {
            nightTrigger.PlayDayTransition(() =>
            {
                SetupDayPhase(day);
            });
        }
        else
        {
            SetupDayPhase(day);
        }
    }

    /// <summary>
    /// 밤 상태 진입 처리. 연출 후 밤 관련 세팅 수행
    /// </summary>
    private void HandleNightStarted()
    {
        if (nightTrigger != null)
        {
            nightTrigger.PlayNightTransition(() =>
            {
                SetupNightPhase();
            });
        }
        else
        {
            SetupNightPhase();
        }
    }

    /// <summary>
    /// 낮 시작 시 프리뷰 표시를 포함한 로직 처리
    /// </summary>
    private void SetupDayPhase(int day)
    {
        var wave = levelCycle.GetWaveDataForCurrentDay();
        if (wave == null) return;

        foreach (var group in wave.spawnGroups)
        {
            if (group.enemies.Count == 0) continue;
            var info = group.enemies[0];

            int index = group.spawnPointIndex;
            if (index >= 0 && index < sceneSpawnPoints.Count)
            {
                sceneSpawnPoints[index].ShowPreview(info.count, info.enemyIcon);
            }
        }
    }

    /// <summary>
    /// 밤 시작 시 적 웨이브 확인 및 스폰 처리
    /// </summary>
    private void SetupNightPhase()
    {
        int day = levelCycle.CurrentDay;
        Debug.Log($"LevelManager: {day}일차 밤 시작 → 적 웨이브 확인");

        var wave = levelCycle.GetWaveDataForCurrentDay();
        if (wave != null)
        {
            foreach (var group in wave.spawnGroups)
            {
                int index = group.spawnPointIndex;
                if (index >= 0 && index < sceneSpawnPoints.Count)
                {
                    StartCoroutine(SpawnEnemies(group, sceneSpawnPoints[index]));
                }
                else
                {
                    Debug.LogWarning($"LevelManager: 유효하지 않은 SpawnPointIndex {index} (씬에 등록된 포인트 수: {sceneSpawnPoints.Count})");
                }
            }
        }
        else
        {
            Debug.LogWarning($"LevelManager: {day}일차에 해당하는 웨이브가 없습니다.");
        }

        foreach (var spawn in sceneSpawnPoints)
        {
            spawn.HidePreview();
        }
    }

    private IEnumerator SpawnEnemies(EnemyWaveData.SpawnGroup group, SpawnPoint spawnPoint)
    {
        foreach (var enemyInfo in group.enemies)
        {
            for (int i = 0; i < enemyInfo.count; i++)
            {
                GameObject enemy = Instantiate(enemyInfo.enemyPrefab, spawnPoint.GetSpawnPosition(), Quaternion.identity);

                if (enemy.TryGetComponent(out Monster monster))
                {
                    void OnMonsterDeath()
                    {
                        monster.action -= OnMonsterDeath;
                        ReportEnemyDeath();
                    }

                    monster.action += OnMonsterDeath;
                    enemiesAlive++;
                }

                yield return new WaitForSeconds(enemyInfo.delayBetweenSpawn);
            }
        }
    }

    private void ReportEnemyDeath()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            Debug.Log("LevelManager: 모든 적 처치됨 → 낮 시작");
            levelCycle.StartDay();
        }
    }
}
