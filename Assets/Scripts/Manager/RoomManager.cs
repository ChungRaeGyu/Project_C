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
        print("�غ��ư");
        Hashtable props = new Hashtable();
        bool current = false;

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(ReadyKey, out object val)) //�غ� ��Ҹ� ����
            current = (bool)val;

        props[ReadyKey] = !current; // ����
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        //TODO : UI ���� �غ���� ����� �����ֱ� �ʿ�

        if (!changedProps.ContainsKey(ReadyKey)) return;

        if (PhotonNetwork.CurrentRoom.PlayerCount != 2) return;

        // �����͸� ���� ���� üũ
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var p in PhotonNetwork.PlayerList)
            {
                if (!p.CustomProperties.TryGetValue(ReadyKey, out var v) || !(bool)v)
                {  //���� ���ų�
                    print($"{p.NickName} ���� �غ� �� ��");
                    return; // ���� ���� �غ� �� ��
                }
            }

            ReadyOrStartBtn.interactable = true; // ��� �غ� �Ϸ�
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
