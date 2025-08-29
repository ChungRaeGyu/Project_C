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
        //���۾� ��ư
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        print("�������� �Ϸ�");
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
        print("Room���� : "+PhotonNetwork.LocalPlayer.NickName);
    }

    public void QuickMatchBtn()
    {
        //����ٰ� ������ �޸� �̰� mmr��Ī�̳� �̷��� �ȴ�.
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        print("������");
        PhotonNetwork.LoadLevel("Room");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("������ �����");
        CreateRoom();
    }
    public void CreateRoom()
    {
        string roomName = "Room" + Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 2; //�ִ��ο�
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Screen.SetResolution(1080, 600, false);
        PhotonNetwork.AutomaticallySyncScene = true; // ���� �� ��ȯ �� ��� Ŭ���̾�Ʈ ����ȭ
        Application.runInBackground = true; // ��Ŀ�� �Ҿ ��Ʈ��ũ ������ ����
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
