using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public Button ReadyOrStartBtn;
    public const string ReadyKey = "readyKey";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ReadyOrStartBtn.interactable = PhotonNetwork.IsMasterClient ? false : true;
        ReadyOrStartBtn.GetComponentInChildren<TMP_Text>().text = PhotonNetwork.IsMasterClient?"Start":"Ready";
        ReadyOrStartBtn.onClick.AddListener(PhotonNetwork.IsMasterClient ? StartBtn : Readybtn);
        if(PhotonNetwork.IsMasterClient)
            Readybtn();
    }
    void StartBtn()
    {
        PhotonNetwork.LoadLevel("Game");
    }
    void Readybtn()
    {
        print("준비버튼");
        Hashtable props = new Hashtable();
        bool current = false;

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(ReadyKey, out object val)) //준비 취소를 위한
            current = (bool)val;

        props[ReadyKey] = !current; // 반전
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
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
                    print($"{p.NickName} 아직 준비 안 됨");
                    return; // 누가 아직 준비 안 됨
                }
            }

            ReadyOrStartBtn.interactable = true; // 모두 준비 완료
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
