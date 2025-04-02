using UnityEngine;
using UnityEngine.EventSystems;

public class DragKey : MonoBehaviour, IDragHandler, IDropHandler
{
    [SerializeField] private RectTransform keyRectTrans;
    [SerializeField] private RectTransform lockRectTrans;

    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.gameObject.transform.SetParent(canvas.transform);
        keyRectTrans.position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnDrop(PointerEventData eventData)
    {
        float distance = Vector2.Distance(keyRectTrans.anchoredPosition, lockRectTrans.anchoredPosition);

        if (distance > 0 && distance < 80)
        {
            Debug.Log("Overlap");
            UIManager.Instance.titleUI.StartCoroutine(UIManager.Instance.titleUI.Unlock(keyRectTrans));
        }
    }
}
