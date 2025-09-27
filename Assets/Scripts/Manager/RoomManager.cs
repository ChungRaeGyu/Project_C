using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_Text RoomNameTxt;
    public Button ReadyOrStartBtn;
    private TMP_Text ButtonText;
    [Header("DeckPanel")] //이거 지금 보여줘야함
    public GameObject DeckPanel;
    public Transform DeckContent;

    [SerializeField] Image[] readyImage;
    [SerializeField] Image[] kindImage;  //어떤 덱인지 알려줄 꺼임
    [SerializeField] TMP_Text masterNickname;
    [SerializeField] TMP_Text slaveNickname;

    public const string ReadyKey = "readyKey";

    private List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //내가 하고 싶은 것 properties이용해서 준비상태를 결정

    /*    public override void OnMasterClientSwitched(Player newMasterClient)
        {
            //나중에 커스텀 룸 만들때 필요함 근데 이게 마스터가 바뀌면서 Room연동이 안됌
            base.OnMasterClientSwitched(newMasterClient);
            StartCoroutine(waitJoined());

        }*/
    private void Awake()
    {
        ButtonText = ReadyOrStartBtn.GetComponentInChildren<TMP_Text>();

    }
    void Start()
    {
        StartCoroutine(waitJoined());
        AddressableManager.Instance.OnreleaseHandle += ReleaseAllImage;
    }

    IEnumerator waitJoined()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        foreach(var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("Deck"))
            {
                if (player.IsMasterClient)
                {

                    _ = kindUI(0, (int)player.CustomProperties["Deck"]);
                    masterNickname.text = player.NickName;
                }
                else
                {
                    _ = kindUI(1, (int)player.CustomProperties["Deck"]);
                    slaveNickname.text = player.NickName;
                }
            }
            else
            {
                Debug.Log(player.NickName);
            }
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "Deck", DataManager.Instance.deckIndex } });
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { ReadyKey, true } });
        else
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { ReadyKey, false } });

        _ = DataManager.Instance.ShowDeck(DataManager.Instance.deckIndex, DeckContent);

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
        if (PhotonNetwork.AutomaticallySyncScene)
        {
            AddressableManager.Instance.ReleaseAll();
            PhotonNetwork.LoadLevel("Game");

        }
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
        //종류 이미지 바꿔주기
        if(changedProps.ContainsKey("Deck"))
        {
            if (targetPlayer.IsMasterClient)
            {
                _ = kindUI(0, (int)changedProps["Deck"]);
            }
            else
            {
                _ = kindUI(1, (int)changedProps["Deck"]);
            }
        }
        if (!changedProps.ContainsKey(ReadyKey)) return;

        int temp = 0;
        // 마스터만 시작 조건 체크
        foreach (var p in PhotonNetwork.PlayerList)
        {
            if (!p.CustomProperties.TryGetValue(ReadyKey, out var v) || !(bool)v)
            {  //값이 없거나 //값이 false일때
                if (PhotonNetwork.IsMasterClient)
                {
                    ReadyOrStartBtn.interactable = false; // 마스터는 준비 가능
                }
                _ = ReadyUI(false, 1);
            }
            else
            {
                _ = ReadyUI(true, p.IsMasterClient ? 0 : 1);     
                temp++;
            }
        }
        if(temp == 2)
        {
            temp = 0;
            ReadyOrStartBtn.interactable = true; // 모두 준비 완료
        }
    }

    private async Task ReadyUI(bool bol, int index)
    {
        string path = bol ? "Assets/Images/O.png" : "Assets/Images/X.png";
        AsyncOperationHandle handle= await AddressableManager.Instance.LoadImage(path);
        handles.Add(handle);
        Sprite sprite = (Sprite)handle.Result;
        readyImage[index].sprite = sprite;
    }
    private async Task kindUI(int index, int deckIndex)
    {
        Deck eDeck = (Deck)deckIndex;
        string path = "Assets/Images/" + eDeck.ToString() + ".png";
        AsyncOperationHandle handle = await AddressableManager.Instance.LoadImage(path);
        handles.Add(handle);
        Sprite sprite = (Sprite)handle.Result;
        kindImage[index].sprite = sprite;
    }
    public void ReleaseAllImage()
    {
        foreach (var handle in handles)
        {
            AddressableManager.Instance.ReleaseImage(handle); 
        }
        handles.Clear();
    }
    #region DeckPanel
    public void DeckOpenBtnClick()
    {
        DeckPanel.SetActive(!DeckPanel.activeSelf);
    }
    #endregion
    // Update is called once per frame

}
