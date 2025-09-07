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
        Init();
    }

    private void Init()
    {
        //덱 종류별로 content바꿔주기
        _= dataManager.ShowDeck((int)Deck.FirstDeck, cardContent);
        _= dataManager.ShowDeck((int)Deck.SecondDeck, cardContent);
        _= dataManager.ShowDeck((int)Deck.ThirdDeck, cardContent);
    }

    private void OpenCardBoard()
    {
        print("실행");
        cardPanel.SetActive(!cardPanel.activeSelf);
    }

}
