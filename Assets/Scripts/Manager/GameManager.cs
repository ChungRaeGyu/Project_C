using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Test Settings")]
    public Button GoToLobbyBtn;
    public Button GoToRoomBtn;

    private Queue<Unit> currentDeck; //이거에서 하나씩 빼서 쓰면 됨
    private List<Unit> currentHands = new List<Unit>(); //현재 손에 들고 있는 카드들
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    }

    private void DrawCard()
    {
        //카드 뽑기
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
