using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Button quickMatchBtn;
    [Header("CardPanel")]
    public GameObject cardPanel;
    public Transform cardContent;
    DataManager dataManager;
    [SerializeField] private GameObject rankingPanel;


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

        Init();
    }

    private async void Init()
    {
        //덱 종류별로 content바꿔주기
        try
        {
            //이러면 비동기가 1도 의미 없긴해
            await dataManager.ShowDeck((int)Deck.FirstDeck, cardContent);
            await dataManager.ShowDeck((int)Deck.SecondDeck, cardContent);
            await dataManager.ShowDeck((int)Deck.ThirdDeck, cardContent);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Init 에러 : "+ex);
            
        }

    }
    public void ButtonRankingControl()
    {
        rankingPanel.SetActive(!rankingPanel.activeSelf);
    }

}
