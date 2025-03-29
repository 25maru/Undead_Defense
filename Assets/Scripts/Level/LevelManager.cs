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

    public LevelCycle Cycle => levelCycle;

    private int enemiesAlive = 0;
    
    private void Start()
    {
        levelCycle.SetLevelData(levelData);
        levelCycle.OnNightStarted += HandleNightStarted;
        levelCycle.OnDayStarted += HandleDayStarted;

        Debug.Log("LevelManager: 레벨 시작됨");
    }

    private void OnDestroy()
    {
        levelCycle.OnNightStarted -= HandleNightStarted;
        levelCycle.OnDayStarted -= HandleDayStarted;
    }

    /// <summary>
    /// 밤이 시작될 때 호출되어 적 스폰을 트리거합니다.
    /// </summary>
    private void HandleNightStarted(int day)
    {
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

        foreach (var sp in sceneSpawnPoints)
        {
            sp.HidePreview();
        }
    }

    /// <summary>
    /// 낮이 시작될 때 → 적 미리보기 UI 표시
    /// </summary>
    private void HandleDayStarted(int day)
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
                SpriteRenderer renderer = info.enemyPrefab.GetComponentInChildren<SpriteRenderer>();
                Sprite icon = renderer != null ? renderer.sprite : null;
                sceneSpawnPoints[index].ShowPreview(info.count, icon);
            }
        }
    }

    /// <summary>
    /// 스폰 그룹 내 적들을 순차적으로 생성합니다.
    /// </summary>
    private IEnumerator SpawnEnemies(EnemyWaveData.SpawnGroup group, SpawnPoint spawnPoint)
    {
        foreach (var enemyInfo in group.enemies)
        {
            for (int i = 0; i < enemyInfo.count; i++)
            {
                GameObject enemy = Instantiate(enemyInfo.enemyPrefab, spawnPoint.GetSpawnPosition(), Quaternion.identity);

                if (enemy.TryGetComponent<Monster>(out var monster))
                {
                    monster.action += ReportEnemyDeath;
                    enemiesAlive++;
                }

                yield return new WaitForSeconds(enemyInfo.delayBetweenSpawn);
            }
        }
    }

    /// <summary>
    /// 적이 죽었을 때 호출되어 남은 적 수를 관리하고, 모두 죽으면 낮을 시작합니다.
    /// </summary>
    public void ReportEnemyDeath()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            Debug.Log("모든 적이 처치됨! 낮으로 전환합니다.");
            levelCycle.ForceStartDay();
        }
    }
}
