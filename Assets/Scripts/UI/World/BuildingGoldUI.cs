using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class GoldGroup
{
    public RectTransform groupParent;
    public List<RectTransform> golds;
}

/// <summary>
/// 건물 위에 표시되는 골드 UI를 관리하는 클래스입니다.
/// 건설에 필요한 골드 수량에 따라 슬롯을 배치하고, 
/// 소비된 골드 수만큼 채워진 상태를 갱신합니다.
/// </summary>
public class BuildingGoldUI : WorldUIBase
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform buildingName;
    [SerializeField] private List<GoldGroup> goldGroups;

    [Header("사운드")]
    [SerializeField] private AudioClip goldSoundClip;
    [Range(0f, 1f)]
    [SerializeField] private float goldSoundVolume;

    [Header("Tween")]
    [SerializeField] private Ease transitionEase = Ease.OutQuad;
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private float fillInterval = 0.2f;
    [SerializeField] private float fillDuration = 0.2f;

    private int totalGold;
    private int currentFilled;

    public float FillInterval => fillInterval;

    private void Start()
    {
        canvasGroup.alpha = 0f;
    }

    /// <summary>
    /// 골드 UI를 초기화하고 필요한 개수만큼 활성화합니다.
    /// </summary>
    /// <param name="requiredGold">필요한 골드 수</param>
    public void Setup(int requiredGold)
    {
        totalGold = requiredGold;
        currentFilled = 0;

        int remaining = requiredGold;

        foreach (var group in goldGroups)
        {
            group.groupParent.gameObject.SetActive(false);
            foreach (var gold in group.golds)
            {
                gold.parent.gameObject.SetActive(false);
                gold.anchoredPosition = Vector2.zero;
            }

            if (remaining <= 0) continue;

            int countToActivate = Mathf.Min(remaining, group.golds.Count);
            for (int i = 0; i < countToActivate; i++)
            {
                group.golds[i].parent.gameObject.SetActive(true);
            }

            group.groupParent.gameObject.SetActive(true);
            ApplyDropEffect(group.golds.GetRange(0, countToActivate));

            remaining -= countToActivate;
        }
        
        float highestY = float.MinValue;
        RectTransform topGold = null;

        foreach (var group in goldGroups)
        {
            if (group.groupParent.gameObject.activeSelf)
            {
                float groupY = group.groupParent.anchoredPosition.y;
                if (groupY > highestY)
                {
                    highestY = groupY;
                    topGold = group.groupParent;
                }
            }
        }

        // foreach (var group in goldGroups)
        // {
        //     foreach (var gold in group.golds)
        //     {
        //         if (gold.parent.gameObject.activeSelf)
        //         {
        //             float y = gold.anchoredPosition.y + ((RectTransform)gold.parent).anchoredPosition.y;
        //             if (y > highestY)
        //             {
        //                 highestY = y;
        //                 topGold = (RectTransform)gold.parent;
        //             }
        //         }
        //     }
        // }

        if (topGold != null)
        {
            float offset = 10f;
            buildingName.anchoredPosition = new Vector2(
                buildingName.anchoredPosition.x,
                topGold.anchoredPosition.y + offset
            );
        }
        else
        {
            buildingName.anchoredPosition = Vector2.zero;
        }
    }

    /// <summary>
    /// 골드 하나를 채우고 UI를 갱신합니다.
    /// </summary>
    public void FillNext()
    {
        int filled = 0;
        foreach (var group in goldGroups)
        {
            foreach (var gold in group.golds)
            {
                if (gold.gameObject.activeSelf)
                {
                    if (filled == currentFilled)
                    {
                        var fill = gold.GetChild(0);

                        if (fill.TryGetComponent(out Image image))
                        {
                            var color = image.color;
                            color.a = 1f;
                            image.color = color;
                        }

                        gold.GetChild(0).DOScale(1f, fillDuration)
                            .SetEase(Ease.OutBack);
                        currentFilled++;

                        if (goldSoundClip != null)
                            AudioSource.PlayClipAtPoint(goldSoundClip, transform.position, goldSoundVolume);
                        return;
                    }
                    filled++;
                }
            }
        }
    }

    /// <summary>
    /// 양쪽 끝 골드 슬롯을 아래로 살짝 내립니다.
    /// </summary>
    private void ApplyDropEffect(List<RectTransform> activeGolds)
    {
        foreach (var gold in activeGolds)
        {
            gold.anchoredPosition = Vector2.zero;
        }

        int count = activeGolds.Count;
        if (count >= 3)
        {
            Vector2 dropOffset = new(0, -4f);
            activeGolds[0].anchoredPosition += dropOffset;
            activeGolds[count - 1].anchoredPosition += dropOffset;
        }
    }

    /// <summary>
    /// 모든 골드를 초기 상태로 리셋합니다.
    /// </summary>
    public void ResetUI()
    {
        currentFilled = 0;

        foreach (var group in goldGroups)
        {
            foreach (var gold in group.golds)
            {
                var fill = gold.GetChild(0).GetComponent<RectTransform>();

                if (fill.TryGetComponent(out Image image))
                {
                    DOTween.Kill(fill);
                    DOTween.Kill(image);
                    
                    fill.DOScale(0f, transitionDuration)
                        .SetEase(transitionEase);

                    image.DOFade(0f, fillDuration)
                        .SetEase(transitionEase);
                }
            }
        }
    }

    /// <summary>
    /// 골드 UI의 표시 여부를 설정합니다.
    /// </summary>
    public void SetGoldVisible(bool visible, Action onComplete = null)
    {
        DOTween.Kill(canvasGroup);

        canvasGroup.DOFade(visible ? 1f : 0f, transitionDuration)
            .SetEase(transitionEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    /// <summary>
    /// 현재 골드가 모두 채워졌는지 여부를 반환합니다.
    /// </summary>
    public bool IsComplete() => currentFilled >= totalGold;
}
