using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Test Settings")]
    public Button GoToLobbyBtn;
    public Button GoToRoomBtn;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
