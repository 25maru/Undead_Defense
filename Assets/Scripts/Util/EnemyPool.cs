using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 프리팹을 오브젝트 풀링 방식으로 관리하는 클래스입니다.
/// </summary>
public class EnemyPool : MonoSingleton<EnemyPool>
{
    private Dictionary<GameObject, Queue<GameObject>> poolDict = new();

    /// <summary>
    /// 지정된 프리팹으로부터 오브젝트를 가져옵니다.
    /// </summary>
    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation = default)
    {
        if (!poolDict.ContainsKey(prefab))
        {
            poolDict[prefab] = new Queue<GameObject>();
        }

        GameObject obj;

        if (poolDict[prefab].Count > 0)
        {
            obj = poolDict[prefab].Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab, position, rotation);
        }

        return obj;
    }

    /// <summary>
    /// 오브젝트를 풀로 반환합니다.
    /// </summary>
    public void Return(GameObject obj, GameObject originalPrefab)
    {
        obj.SetActive(false);
        poolDict[originalPrefab].Enqueue(obj);
    }
}