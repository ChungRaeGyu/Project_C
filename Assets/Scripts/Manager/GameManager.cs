using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//TODO : 
/* 점령시 그 쪽 라인 닫아주기(설치불가능 및 해당 라인에 몬스터 삭제)
 * GhostObject.cs 쪽에 설치불가능한 조건 추가
 * 각 라인 관리
 * 점령에 대한 정보는 몬스터나 탑에서 한다.
 * 
 * 몬스터를 소환할때 
 * 1. 탑에 몬스터가 닿였을때 GameManager에 있는 점령을 호출한다.
 * 2. 점령상태는 공유하돼 점령했다는 자체는 로컬로 관리해도 되겠지?
 * 3. 점령상태 : 라인 닫기, 라인 닫을 껀데 이건 네트워크 공유 PhotonView를 그럼 GameManager에서 호출 할 필요가 있긴 하네
 * 4. 점령을 2개 완료하면 네트워크에다가 다 공유 게임이 끝났음을 알림
 */

//할일  : 라인 만들어서 currentLine 입력시켜 주기, 탑 만들어서 몬스터가 닿았을 때 점령 완료 호출
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Test Settings")]
    public Button GoToLobbyBtn;
    public Button GoToRoomBtn;

    private Queue<Unit> currentDeck; //이거에서 하나씩 빼서 쓰면 됨
    private List<Unit> currentHands = new List<Unit>(); //현재 손에 들고 있는 카드들

    private int startHandCount = 5; //시작할 때 들고 있는 카드 수
    [SerializeField] private Transform handParent; //손에 들고 있는 카드들 부모

    //인수
    private float startTime = 0;
    private float time = 0;
    private bool isStart = false;
    
    [HideInInspector]
    public int cost = 0;
    private float costTime = 0;
    private int occupation = 3; //점령확인

    private PhotonView pv;

    [HideInInspector]
    public List<GameObject>[] objList = { new List<GameObject>(), new List<GameObject>(), new List<GameObject>() };
    [HideInInspector] 
    public bool[] lineBool = new bool[]{ true, true, true };
    
    public Transform[] tPosition; //탑의 위치 값으로 쓰면 되겠다

    //UI
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text CostText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {

        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        pv = GetComponent<PhotonView>();

    }
    public void LindAdd(GameObject obj,int n)
    {
        objList[n].Add(obj);
    }
    private void Update()
    {
        if (!isStart) return;

        //StatrTime을 조절 해줘야한다. 지금 시간이 개판임
        time = (int)(PhotonNetwork.Time - startTime);
        if (time - costTime >= 1)
        {
            costTime = time;
            cost++;
            cost = Math.Min(cost, 10);
        }

        timeText.text = System.TimeSpan.FromSeconds(time).ToString(@"mm\:ss"); //시간 동기화
        CostText.text = $"cost : {cost}";

    }

    public void Occupation(int num)
    {
        //탑에서 호출
        occupation--;
        pv.RPC("LineClose", RpcTarget.All,num);
        CheckWinner();
    }
    [PunRPC]
    private void LineClose(int n)
    {
        //n번째 라인 닫아주기 - 유닛 소환을 못하게 한다.
        //몇번째 라인인지 어떻게 받아올까
        //길을 나눠서 트리거로 해가지고 들어오면 되긴할꺼같네
        lineBool[n] = false; //라인닫기
        foreach (var obj in objList[n])
        {
            AddressableManager.Instance.ReleaseObj(obj);
        }
        objList[n].Clear();
    }
    private void CheckWinner()
    {
        if (occupation == 1)
        {
            pv.RPC("GameOver", RpcTarget.All);
        }
    }
    [PunRPC]
    private void GameOver()
    {
        isStart = false;
        if (occupation == 1)
        {
            Debug.Log("승리");
        }
        else
        {
            Debug.Log("패배");
        }
    }

    private void SetTime()
    {
        int startTime = PhotonNetwork.ServerTimestamp;
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "StartTime", startTime } });
    }

    IEnumerator Progress()
    {
        //시간의 흐름
        //자원 올려주기
        //점령
        //승,패
        
        yield return null;

    }
    private void Init()
    {
        //첫 시작
        if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("StartTime", out var t))
        {
            startTime = (float)t;
        }

        isStart = true;
        for (int i = 0; i < startHandCount; i++)
        {
           DrawCard();
        }

    }
    private void Reset()
    {
        //Enable떄 init()을 해버려도 될
    }
    private async Task DrawCard()
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
        await DataManager.Instance.ShowCard(handParent, unit);
    }

    public void RemoveCard(Unit u)
    {
        currentHands.Remove(u);
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
        if (PhotonNetwork.IsMasterClient)
        {
            SetTime();
        }
        currentDeck = new Queue<Unit>(DataManager.Instance.SuffleDeck());
        Init();
    }
}
