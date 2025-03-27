using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 각 스테이지의 설정을 담은 ScriptableObject입니다.
/// 밤마다 등장하는 적 웨이브를 정의합니다.
/// </summary>
[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Object/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("레벨 이름 (디버그용)")]
    public string levelName;

    [Header("밤이 되는 시간 (초)")]
    public float nightStartTime = 30f;

    [Header("밤마다 소환될 적 웨이브들")]
    public List<EnemyWave> enemyWaves = new();
}

/// <summary>
/// 특정 날짜(밤)에 등장할 적 웨이브를 정의합니다.
/// </summary>
[System.Serializable]
public class EnemyWave
{
    [Tooltip("이 웨이브가 등장할 날. (예: 1 = 첫째 날 밤)")]
    public int day;

    [Tooltip("스폰 포인트 단위로 구성된 적 그룹들")]
    public List<SpawnGroup> spawnGroups = new();
}

/// <summary>
/// 특정 스폰 포인트에서 소환될 적 종류 및 수량 그룹입니다.
/// </summary>
[System.Serializable]
public class SpawnGroup
{
    public SpawnPoint spawnPoint;
    public List<EnemySpawnInfo> enemies = new();
}

/// <summary>
/// 소환될 적 개체 정보입니다.
/// </summary>
[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public int count = 5;
    public float delayBetweenSpawn = 1f;
}