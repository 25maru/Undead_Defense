using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 특정 날짜의 적 웨이브 정보를 담는 ScriptableObject입니다.
/// </summary>
[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Object/Enemy Wave Data")]
public class EnemyWaveData : ScriptableObject
{
    [Tooltip("몇 일차 웨이브인지 지정합니다.")]
    public int day;

    [Tooltip("해당 날짜에 소환될 스폰 그룹 리스트입니다.")]
    public List<SpawnGroup> spawnGroups;

    /// <summary>
    /// 한 그룹에서 스폰되는 적들의 위치와 종류를 정의합니다.
    /// </summary>
    [Serializable]
    public class SpawnGroup
    {
        [Tooltip("스폰 포인트 인덱스 (씬에 배치된 SpawnPoint들과 매칭됨)")]
        public int spawnPointIndex;

        [Tooltip("해당 포인트에서 스폰될 적 리스트")]
        public List<EnemySpawnInfo> enemies;
    }

    /// <summary>
    /// 개별 적의 종류, 수량, 소환 간격 정보를 담는 구조체입니다.
    /// </summary>
    [Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public Sprite enemyIcon;
        public int count = 1;
        public float delayBetweenSpawn = 0.5f;
    }
}