using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public static NetworkManager Instance;
    public TMP_InputField NickNameInput;
    public void ConnectBtn()
    {
        //시작씬 버튼
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        print("서버접속 완료");
        if (PhotonNetwork.LocalPlayer.NickName.IsNullOrEmpty())
        {
            PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        }
        JoinLobby();
        PhotonNetwork.LoadLevel("Lobby");
    }
    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {
        print("Room입장 : "+PhotonNetwork.LocalPlayer.NickName);
    }

    public void QuickMatchBtn()
    {
        //여기다가 조건을 달면 이게 mmr매칭이나 이런게 된다.
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        print("방입장");
        PhotonNetwork.LoadLevel("Room");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("빈방없음 방생성");
        CreateRoom();
    }
    public void CreateRoom()
    {
        string roomName = "Room" + Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 2; //최대인원
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Screen.SetResolution(1080, 600, false);
        PhotonNetwork.AutomaticallySyncScene = true; // 방장 씬 전환 → 모든 클라이언트 동기화
        Application.runInBackground = true; // 포커스 잃어도 네트워크 유지에 도움
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
