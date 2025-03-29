using UnityEngine;

/// <summary>
/// 적 스폰 위치를 정의하는 컴포넌트입니다.
/// 필요 시 랜덤 오프셋 또는 범위 설정이 가능합니다.
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    [Tooltip("스폰 위치에 적용할 랜덤 반경 (0이면 위치 고정)")]
    [SerializeField] private float randomRadius = 0f;

    [Tooltip("미리보기 UI 프리팹 (World Space Canvas)")]
    [SerializeField] private GameObject previewUIPrefab;

    private GameObject previewInstance;

    public int SpawnPointIndex { get; private set; } // 씬에서 수동 설정하거나 자동 인덱싱 가능

    /// <summary>
    /// 적이 스폰될 실제 위치를 반환합니다.
    /// </summary>
    public Vector3 GetSpawnPosition()
    {
        if (randomRadius > 0f)
        {
            Vector2 offset = Random.insideUnitCircle * randomRadius;
            return transform.position + new Vector3(offset.x, 0f, offset.y);
        }
        return transform.position;
    }

    /// <summary>
    /// 낮 시간에 적 미리보기 UI를 표시합니다.
    /// </summary>
    public void ShowPreview(int count, Sprite enemyIcon)
    {
        if (previewUIPrefab == null || previewInstance != null) return;

        previewInstance = Instantiate(previewUIPrefab, transform.position, Quaternion.identity, transform);

        if (previewInstance.TryGetComponent(out SpawnPreviewUI ui))
        {
            ui.SetInfo(count, enemyIcon);
        }
    }

    /// <summary>
    /// 밤 시작 시 미리보기 UI를 제거합니다.
    /// </summary>
    public void HidePreview()
    {
        if (previewInstance != null)
        {
            Destroy(previewInstance);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, randomRadius);
    }
#endif
}