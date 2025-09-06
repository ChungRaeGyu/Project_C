using System.Collections;
using System.Threading.Tasks;
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

        //StartCoroutine(init());
        _ = Init();
    }

    private async Task Init()
    {
        foreach (var unit in dataManager.unitSO)
        {
            GameObject obj = Instantiate(dataManager.cardPrefab, cardContent.transform);
            obj.GetComponent<Card>().unit = unit;
            obj.GetComponent<Card>().descriptionPanel = descriptionPanel;
            Sprite sprite = await AddressableManager.Instance.LoadImage(unit.imagePath);
            obj.GetComponent<Image>().sprite = sprite;
        }
    }
    private void OpenCardBoard()
    {
        print("½ÇÇà");
        cardPanel.SetActive(!cardPanel.activeSelf);
    }

}
