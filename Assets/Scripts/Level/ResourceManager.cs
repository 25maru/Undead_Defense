using System;
using UnityEngine;

/// <summary>
/// 게임 내 자원(골드)을 중앙에서 관리하는 매니저 클래스입니다.
/// </summary>
public class ResourceManager : MonoSingleton<ResourceManager>
{
    /// <summary>
    /// 현재 보유 중인 골드입니다.
    /// </summary>
    public int Gold { get; private set; }

    /// <summary>
    /// 골드가 변경될 때 호출되는 이벤트입니다.
    /// UI나 다른 시스템이 이 이벤트를 구독하여 실시간으로 반응할 수 있습니다.
    /// </summary>
    public event Action<int> OnGoldChanged;

    /// <summary>
    /// 골드를 추가합니다.
    /// </summary>
    /// <param name="amount">추가할 골드 양</param>
    public void AddGold(int amount)
    {
        Gold += amount;
        OnGoldChanged?.Invoke(Gold);
    }

    /// <summary>
    /// 골드를 소비합니다.
    /// </summary>
    /// <param name="amount">소비할 골드 양</param>
    /// <returns>골드가 충분하면 true, 부족하면 false를 반환합니다.</returns>
    public bool SpendGold(int amount)
    {
        if (Gold < amount)
            return false;

        Gold -= amount;
        OnGoldChanged?.Invoke(Gold);
        return true;
    }
}
