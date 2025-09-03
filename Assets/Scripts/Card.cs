using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public Unit unit;
    [HideInInspector]
    public DescriptionPanel descriptionPanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        //카드의 기본정보를 얻고 설명창을 열어 줄꺼야
        descriptionPanel.gameObject.SetActive(true);
        descriptionPanel.unit = unit;
        descriptionPanel.init();
    }
}
