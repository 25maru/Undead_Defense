using UnityEngine;

/// <summary>
/// 적 스폰 위치를 나타내는 마커 컴포넌트입니다.
/// 에디터에서 배치하고 LevelManager에서 참조합니다.
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up);
    }
}