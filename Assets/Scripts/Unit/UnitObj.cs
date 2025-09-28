using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UnitObj : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //여기는 효과를 적자 어때
    //얘를 상속받아서 효과를 쓰자
    //
    [HideInInspector] public int line;
    [HideInInspector] public string unitName;
    public int cost;
    [HideInInspector] public int damage;
    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float speed;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public AsyncOperationHandle handle;
    [SerializeField] TMP_Text text;
    public RectTransform[] canvas;
    protected PhotonView pv;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        pv = GetComponent<PhotonView>();
        canvas = GetComponentsInChildren<RectTransform>();
    }
    public void InitRPC(int line, string unitName, int cost, int damage, float attackSpeed, float speed)
    {
        pv.RPC("Init", RpcTarget.All, line, unitName, cost,damage,attackSpeed,speed);
    }
    [PunRPC]
    public void Init(int line, string unitName, int cost, int damage, float attackSpeed, float speed)
    {
        this.line = line;
        this.unitName = unitName;
        this.cost = cost;
        this.damage = damage;
        this.attackSpeed = attackSpeed;
        this.speed = speed;
        text.text = cost.ToString();
        if (!pv.IsMine)
        {
            Vector3 rect = canvas[0].localScale;
            rect.x *= -1;
            canvas[0].localScale = rect;
        }

    }
    public void HandleInit()
    {
        AddressableManager.Instance.OnreleaseHandle += Remove;
    }
    public void Remove()
    {
        if (this == null) return;   // UnitObj 자체가 Destroy된 경우
        if (gameObject == null) return; // GameObject가 Destroy된 경우
        AddressableManager.Instance.OnreleaseHandle -= Remove;
        PhotonNetwork.Destroy(this.gameObject);
    }
}
