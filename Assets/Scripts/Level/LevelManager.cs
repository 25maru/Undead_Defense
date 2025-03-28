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
    [Header("레벨 데이터")]
    [SerializeField] private LevelData levelData;

    [Header("사이클 컨트롤러")]
    [SerializeField] private LevelCycle levelCycle;

    private int enemiesAlive = 0;
    
    private void Start()
    {
        // 사이클 이벤트 등록
        levelCycle.OnNightStarted += HandleNightStarted;
        levelCycle.OnDayStarted += HandleDayStarted;

        Debug.Log("LevelManager: 레벨 시작됨");
    }

    /// <summary>
    /// 밤이 시작되면 해당 일차에 맞는 적 웨이브를 소환합니다.
    /// </summary>
    /// <param name="day">현재 밤의 날짜 (1일부터 시작)</param>
    private void HandleNightStarted(int day)
    {
        Debug.Log($"LevelManager: {day}일차 밤 시작 → 적 웨이브 확인");

        var wave = levelData.enemyWaves.Find(w => w.day == day);
        if (wave != null)
        {
            foreach (var group in wave.spawnGroups)
            {
                foreach (var spawn in group.enemies)
                {
                    StartCoroutine(SpawnEnemies(spawn, group.spawnPoint));
                }
            }
        }
        else
        {
            Debug.LogWarning($"LevelManager: {day}일차에 해당하는 웨이브가 없습니다.");
        }
    }

    /// <summary>
    /// 낮 시작 시 처리할 로직 (예: 리셋, 보상 등)
    /// </summary>
    private void HandleDayStarted(int day)
    {
        Debug.Log($"LevelManager: {day}일차 낮 시작");
        // TODO: 낮 시간 처리 로직 추가 (예: 건설, 업그레이드)
    }

    /// <summary>
    /// 적 개별 스폰 코루틴. 일정 간격으로 적을 생성합니다.
    /// </summary>
    private IEnumerator SpawnEnemies(EnemySpawnInfo spawnInfo, SpawnPoint point)
    {
        for (int i = 0; i < spawnInfo.count; i++)
        {
            Vector3 offset = Random.insideUnitSphere * 1.5f;
            offset.y = 0;
            Vector3 spawnPos = point.transform.position + offset;

            GameObject enemy = Instantiate(spawnInfo.enemyPrefab, spawnPos, Quaternion.identity);

            if (enemy.TryGetComponent<Monster>(out var monster))
            {
                monster.action += ReportEnemyDeath;
                enemiesAlive++;
            }

            Debug.Log($"적 소환: {spawnInfo.enemyPrefab.name} ({i + 1}/{spawnInfo.count}) @ {point.name}");
            yield return new WaitForSeconds(spawnInfo.delayBetweenSpawn);
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
