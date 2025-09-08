using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Test Settings")]
    public Button GoToLobbyBtn;
    public Button GoToRoomBtn;
    
    private Queue<Unit> currentDeck; //이거에서 하나씩 빼서 쓰면 됨
    private List<Unit> currentHands = new List<Unit>(); //현재 손에 들고 있는 카드들

    private int startHandCount = 5; //시작할 때 들고 있는 카드 수
    [SerializeField]private Transform handParent; //손에 들고 있는 카드들 부모
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        GoToLobbyBtn.onClick.AddListener(() =>
        {
            NetworkManager.Instance.GotoLobby();
        });
        GoToRoomBtn.onClick.AddListener(() =>
        {
            NetworkManager.Instance.ReJoindRoom(PhotonNetwork.CurrentRoom.Name);
        });

        currentDeck = new Queue<Unit>(DataManager.Instance.SuffleDeck());
        Init();
    }

    private void Init()
    {
        //첫 시작
        for(int i = 0; i < startHandCount; i++)
        {
            DrawCard();
        }

    }
    private void DrawCard()
    {
        //카드 뽑기
        if(currentDeck.Count == 0)
        {
            Debug.Log("덱이 다 떨어졌습니다.");
            return;
        }
        Unit unit = currentDeck.Dequeue();
        currentHands.Add(unit);
        //카드를 뽑아서 보여준다. 
        _= DataManager.Instance.ShowCard(handParent, unit);
    }

    public void RemoveCard(Unit u)
    {
        currentHands.Remove(u);
    }
}
