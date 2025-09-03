using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionPanel : MonoBehaviour
{
    //얘를 쭉 한번 들고 다녀 볼까
    public Button closeDescriptionPanel;
    [SerializeField]Image image;
    [SerializeField]TMP_Text descriptionText;
    [HideInInspector]
    public Unit unit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        image.GetComponent<Sprite>();
        closeDescriptionPanel.onClick.AddListener(() =>
        {
            gameObject.SetActive(!gameObject.activeSelf);
        });
    }
    public void init()
    {
        image.sprite = AddressableManager.Instance.imageDictionary[unit.imagePath];
        descriptionText.text = $"이름 : {unit.unitName}\n비용 : {unit.cost}\n공격력 : {unit.damage}\n공격속도 : {unit.attackSpeed}\n이동속도 : {unit.speed}";
    }
}