using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using WebSocketSharp;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public static NetworkManager Instance;
    bool reStart = false;
    string roomName="";
    public void Quit()
    {
        Application.Quit();
    }
    public void DisConnect()
    {
        PhotonNetwork.Disconnect();
    }
    public void OnApplicationQuit()
    {
        GotoLobby();
        DisConnect();
        AddressableManager.Instance.ReleaseAll();
    }
    public void ReJoindRoom(string name)
    {
        PhotonNetwork.LeaveRoom();
        reStart = true;
        roomName = name;

        //방을 떠나고 OnJoinedLobby에서 다시 JoinRoom을 시도
        //OnLeftRoom에서 바로 JoinRoom을 시도하면 안됨
    }
    public void GotoLobby()
    {
        reStart = false;
        AddressableManager.Instance.ReleaseAll();
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        JoinLobby();
    }
    public void ConnectBtn()
    {
        //시작씬 버튼
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.LocalPlayer.NickName.IsNullOrEmpty())
        {
            PhotonNetwork.LocalPlayer.NickName = PlayerData.Instance.GetNickName();
        }
        JoinLobby();
    }
    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {
        if (reStart)
        {
            Debug.Log("여기여기여여기때문인가??");
            PhotonNetwork.JoinRoom(roomName);
            return;
        }
        PhotonNetwork.LoadLevel("Lobby");

    }

    public void QuickMatchBtn()
    {
        //여기다가 조건을 달면 이게 mmr매칭이나 이런게 된다.
        AddressableManager.Instance.ReleaseAll();
        DataManager.Instance.DeckSetting();
        PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Deck", DataManager.Instance.deckIndex } });
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("Room");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }
    public void CreateRoom()
    {
        string roomName = "Room" + Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 2; //최대인원
        roomOptions.EmptyRoomTtl = 5;
        roomOptions.CleanupCacheOnLeave = true;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
//        Screen.SetResolution(540, 960, false);
        Application.runInBackground = true; // 포커스 잃어도 네트워크 유지에 도움
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.PrefabPool = new AddressableManager();
        Debug.Log("[BOOT] Pool = " + (PhotonNetwork.PrefabPool?.GetType().FullName ?? "NULL"));
    }
}
