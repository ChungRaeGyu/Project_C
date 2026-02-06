using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Image[] view;
    Color origin = new Color(1, 1, 1, 1);
    Color invisible = new Color(1, 1, 1, 0.5f);

    bool isDragging = false;
    int length;
    int num;
    int current_num = 0;
    float pageWidth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        length = view.Length;

    }

    private void Update()
    {
        if (!isDragging) return;
        pageWidth = scrollRect.horizontalNormalizedPosition; //0~1;
        pageWidth = Mathf.Clamp(pageWidth, 0, 1);

        num = Mathf.RoundToInt(pageWidth * (length - 1));

        if (current_num != num)
        {
            view[current_num].color = invisible;
            current_num = num;
            view[num].color = origin;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scrollRect.horizontalNormalizedPosition = (float)num / (length - 1);
    }
}
