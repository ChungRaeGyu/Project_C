using Photon.Pun;
using Photon.Pun.Demo.SlotRacer.Utils;
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
public enum EDeBuff
{
    ATKDOWN,
    BRINGANYWAY,
    ATKSPEEDDOWN
}
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    [Header("Test Settings")]
    public Button GoToLobbyBtn;
    public Button GoToRoomBtn;

    private Queue<Unit> currentDeck; //이거에서 하나씩 빼서 쓰면 됨
    private List<Unit> currentHands = new List<Unit>(); //현재 손에 들고 있는 카드들

    private int startHandCount = 4; //시작할 때 들고 있는 카드 수
    [SerializeField] private Transform handParent; //손에 들고 있는 카드들 부모

    //인수
    private double startTime = 0;
    private double time = 0;
    private bool isStart = false;
    
    [HideInInspector]
    public int cost = 0;
    private double costTime = 0;
    private int occupation = 3; //점령확인

    private PhotonView pv;

    [HideInInspector]
    public List<GameObject>[] objList = { new List<GameObject>(), new List<GameObject>(), new List<GameObject>() };
    [HideInInspector] 
    public bool[] lineBool = new bool[]{ true, true, true };

    [SerializeField] GameObject[] masterSpawn;
    [SerializeField] GameObject[] slaveSpawn;

    public Transform[] spawnPosition;
    public Transform[] tPosition; //탑의 위치 값으로 쓰면 되겠다
    

    
    
    //UI
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text CostText;
    [SerializeField] private EndPanel EndPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {

        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        pv = GetComponent<PhotonView>();
        SpawnArea();

        if (PhotonNetwork.OfflineMode)
        {
            Debug.Log("오프라인 끄기");
            PhotonNetwork.OfflineMode = false;
        }
    }

    private void SpawnArea()
    {
        bool check = PhotonNetwork.IsMasterClient;
        for (int i = 0; i < masterSpawn.Length; i++)
        {
            masterSpawn[i].SetActive(check);
            slaveSpawn[i].SetActive(!check);
        }
    }

    public void LindAdd(GameObject obj, int n)
    {
        objList[n].Add(obj);
        n = PhotonNetwork.IsMasterClient ? n : n + 3;
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "UnitCount", 1 },{"Line",n } });
    }
    public void LineRemove(List<GameObject> list, int line)
    {
        int i = 0;
        foreach (GameObject obj in list)
        {
            i++;
            objList[line].Remove(obj);
        }
        line = PhotonNetwork.IsMasterClient ? line : line + 3;
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "UnitCount", -i }, { "Line", line } });
    }

    public void Occupation(int num) 
    {
        
        //탑에서 호출
        occupation--;
        Debug.Log("점령완료 : " + occupation);
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
            obj.GetComponent<UnitObj>().Remove();
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
        EndPanel.gameObject.SetActive(true);
        if (occupation == 1)
        {
            PlayerData.Instance.AddScore(10);
            EndPanel.Init(true);
        }
        else
        {
            PlayerData.Instance.AddScore(-10);
            EndPanel.Init(false);
        }
    }

    private void Reset()
    {
        //Enable떄 init()을 해버려도 될
    }
    public async Task DrawCard()
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

    public void CallRPC(float[] num) //enum, line, 디버프 수치
    {
        pv.RPC("PDeBuff", RpcTarget.Others, num);
    }
    [PunRPC]
    public void PDeBuff(float[] num)//enum받기
    {
        switch ((int)num[0]) //enum으로 디버프 종류 알려주기
        {
            case (int)EDeBuff.ATKDOWN: //공격력 깍기
                ATKDown(num);
                break;
            case (int)EDeBuff.ATKSPEEDDOWN://공격속도 줄이기
                ATKSpeedDown(num);
                break;
            case (int)EDeBuff.BRINGANYWAY://상대 위치 바꾸기
                BringAnyWay(num);
                break;
        }
    }

    private void BringAnyWay(float[] num)
    {
        Debug.Log("옮김 당함");
        List<int> ints = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            if (i != (int)num[1] && objList[i].Count < 4 && lineBool[i])
            {
                ints.Add(i);
            }
        }
        if (objList[(int)num[1]].Count == 0)
        {
            return;
        }
        int rand = UnityEngine.Random.Range(0, objList[(int)num[1]].Count);
        GameObject temp = objList[(int)num[1]][rand]; //index outRange 261000
        Debug.Log($"rand : {rand}, ints.Count : {ints.Count}, ints[0] : {ints[0]}");
        switch (ints.Count)
        {
            case 0://자리 못바꿈
                break;
            case 1:
                SwitchLine((int)num[1], ints[0], temp);
                break;
            case 2:
                int random = UnityEngine.Random.Range(0, 100);
                int num2 = random > 50 ? ints[0] : ints[1];
                SwitchLine((int)num[1], num2, temp);
                break;
        }
    }

    private void SwitchLine(int num, int num2, GameObject temp)
    {
        //원래라인, 바꿀라인, 게임오브젝트
        objList[num].Remove(temp);
        objList[num2].Add(temp);
        UnitFSM fsm = temp.GetComponent<UnitFSM>();
        fsm.agent.enabled = false;
        temp.transform.position = PhotonNetwork.IsMasterClient ? spawnPosition[num2].position : spawnPosition[num2 + 3].position;
        fsm.agent.enabled = true;
        fsm.GoToGoal();
    }

    private void ATKSpeedDown(float[] num)
    {
        foreach (GameObject obj in objList[(int)num[1]])
        {
            UnitObj unit = obj.GetComponent<UnitObj>();
            unit.attackSpeed *= num[2];
        }
    }

    private void ATKDown(float[] num)
    {
        foreach (GameObject obj in objList[(int)num[1]])
        {
            UnitObj unit = obj.GetComponent<UnitObj>();
            unit.damage -= (int)num[2];
        }
    }
    public void LobbyBtn()
    {
        GoToLobbyBtn.interactable = false;
        NetworkManager.Instance.GotoLobby();

    }
    void Start()
    {
        currentDeck = new Queue<Unit>(DataManager.Instance.SuffleDeck());
        GoToLobbyBtn.onClick.AddListener(LobbyBtn);
        GoToRoomBtn.onClick.AddListener(() =>
        {
            NetworkManager.Instance.ReJoindRoom(PhotonNetwork.CurrentRoom.Name);
        });
        SetTime();
        Init();
    }
    private void Update()
    {
        if (!isStart) return;

        //StatrTime을 조절 해줘야한다. 지금 시간이 개판임
        time = PhotonNetwork.Time - startTime;
        if (time - costTime >= 1)
        {
            costTime = time;
            cost++;
            cost = Math.Min(cost, 10);
        }

        timeText.text = System.TimeSpan.FromSeconds((int)time).ToString(@"mm\:ss"); //시간 동기화
        CostText.text = $"{cost}";

    }
    private void SetTime()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startTime = PhotonNetwork.Time;
            Debug.Log("시작 시간 " + startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "StartTime", startTime } });
        }

    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("StartTime", out var t))
        {
            startTime = Convert.ToDouble(t);
            Debug.Log("시간");
        }
    }
 
    private void Init()
    {
        isStart = true;
        for (int i = 0; i < startHandCount; i++)
        {
            DrawCard();
        }
    }
}
