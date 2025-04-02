using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 하나의 레벨(스테이지)을 구성하는 ScriptableObject입니다.
/// </summary>
[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Object/Level Data")]
public class LevelData : ScriptableObject
{
    [Tooltip("레벨 이름 (디버그 또는 UI 용도)")]
    public string levelName;

    [Tooltip("이 레벨에서 진행될 웨이브 에셋 목록")]
    public List<EnemyWaveData> enemyWaves = new();

    /// <summary>
    /// 일차(day)에 해당하는 웨이브를 반환합니다.
    /// </summary>
    public EnemyWaveData GetWaveByDay(int day)
    {
        foreach (var wave in enemyWaves)
        {
            if (wave.day == day)
                return wave;
        }
        return null;
    }
}