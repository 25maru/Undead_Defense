using UnityEngine;

/// <summary>
/// 모든 월드 공간 UI(World-Space Canvas)를 위한 기본 클래스입니다.
/// 자식 UI들은 이 클래스를 상속받아 구조적 일관성과 재사용성을 유지할 수 있습니다.
/// </summary>
public abstract class WorldUIBase : MonoBehaviour
{
    protected Canvas canvas;

    protected virtual void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
            canvas.worldCamera = Camera.main;
    }
}
