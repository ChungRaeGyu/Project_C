using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Button quickMatchBtn;
    [Header("CardPanel")]
    public Button cardCheckBtn;
    public Button closeCardPanel;
    public GameObject cardPanel;
    public Transform cardContent;
    DataManager dataManager;

    [SerializeField] DescriptionPanel descriptionPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        dataManager = DataManager.Instance;
    }

    void Start()
    {
        quickMatchBtn.onClick.AddListener(() =>
        {
            NetworkManager.Instance.QuickMatchBtn();
        });
        cardCheckBtn.onClick.AddListener(OpenCardBoard);
        closeCardPanel.onClick.AddListener(OpenCardBoard);

        StartCoroutine(init());



    }

    IEnumerator init()
    {
        foreach (var unit in dataManager.unitSO)
        {
            GameObject obj = Instantiate(dataManager.cardPrefab, cardContent.transform);
            obj.GetComponent<Card>().unit = unit;
            obj.GetComponent<Card>().descriptionPanel = descriptionPanel;
            AddressableManager.Instance.LoadImage(unit.imagePath);
            yield return new WaitUntil(() => AddressableManager.Instance.imageDictionary.ContainsKey(unit.imagePath));
            obj.GetComponent<Image>().sprite = AddressableManager.Instance.imageDictionary[unit.imagePath];
        }
    }
    private void OpenCardBoard()
    {
        print("½ÇÇà");
        cardPanel.SetActive(!cardPanel.activeSelf);
    }

}
