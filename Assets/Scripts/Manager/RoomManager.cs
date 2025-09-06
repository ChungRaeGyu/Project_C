using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_Text RoomNameTxt;
    public Button ReadyOrStartBtn;
    private TMP_Text ButtonText;
    
    [SerializeField] Image[] readyImage;
    [SerializeField] Image[] kindImage;  //어떤 덱인지 알려줄 꺼임
    
    public const string ReadyKey = "readyKey";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //내가 하고 싶은 것 properties이용해서 준비상태를 결정

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //나중에 커스텀 룸 만들때 필요함 근데 이게 마스터가 바뀌면서 Room연동이 안됌
        base.OnMasterClientSwitched(newMasterClient);
        StartCoroutine(waitJoined());

    }
    private void Awake()
    {
        ButtonText = ReadyOrStartBtn.GetComponentInChildren<TMP_Text>();

    }
    void Start()
    {
        StartCoroutine(waitJoined());
        print("마스터 클라이언트 여부 : " + PhotonNetwork.IsMasterClient);
    }

    IEnumerator waitJoined()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { ReadyKey, true } });
        else
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { ReadyKey, false } });

        ReadybtnSetting();
    }

    private void ReadybtnSetting()
    {
        ReadyOrStartBtn.interactable = PhotonNetwork.IsMasterClient ? false : true;
        ButtonText.text = PhotonNetwork.IsMasterClient ? "Start" : "Not Ready";
        ReadyOrStartBtn.onClick.AddListener(PhotonNetwork.IsMasterClient ? StartBtn : Readybtn);

    }

    void StartBtn()
    {
        Debug.Log($"[SYNC] IsMaster={PhotonNetwork.IsMasterClient}, " +
          $"AutoSync={PhotonNetwork.AutomaticallySyncScene}, " +
          $"InRoom={PhotonNetwork.InRoom}, " +
          $"State={PhotonNetwork.NetworkClientState}, " +
          $"MsgQueue={PhotonNetwork.IsMessageQueueRunning}");
        if (PhotonNetwork.AutomaticallySyncScene)
            PhotonNetwork.LoadLevel("Game");
        else
        {
            print("동기화 꺼짐");
        }

    }
    void Readybtn()
    {
        Hashtable props = new Hashtable();
        bool current = false;

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(ReadyKey, out object val)) //준비 취소를 위한
            current = (bool)val;

        props[ReadyKey] = !current; // 반전
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        ButtonText.text = (bool)props[ReadyKey] ? "Ready" : "Not Ready";

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        //TODO : UI 갱신 준비상태 변경시 보여주기 필요
        if (!changedProps.ContainsKey(ReadyKey)) return;

        if (PhotonNetwork.CurrentRoom.PlayerCount != 2) return;

        // 마스터만 시작 조건 체크
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var p in PhotonNetwork.PlayerList)
            {
                if (!p.CustomProperties.TryGetValue(ReadyKey, out var v) || !(bool)v)
                {  //값이 없거나
                    ReadyOrStartBtn.interactable = false;
                    return; // 누가 아직 준비 안 됨
                }
            }

            ReadyOrStartBtn.interactable = true; // 모두 준비 완료
        }
    }

    private void ReadyUI()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        RoomNameTxt.text = PhotonNetwork.CurrentRoom.Name;
    }
}
