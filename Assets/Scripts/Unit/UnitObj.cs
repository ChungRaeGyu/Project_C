using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UnitObj : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //여기는 효과를 적자 어때
    //얘를 상속받아서 효과를 쓰자

    [HideInInspector] public int line;
    [HideInInspector] public string unitName;
    [HideInInspector] public int cost;
    [HideInInspector] public int damage;
    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float speed;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public AsyncOperationHandle handle;
    protected PhotonView pv;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        pv = GetComponent<PhotonView>();
    }

    public void Init(int line, string unitName, int cost, int damage, float attackSpeed, float speed)
    {
        this.line = line;
        this.unitName = unitName;
        this.cost = cost;
        this.damage = damage;
        this.attackSpeed = attackSpeed;
        this.speed = speed;
    }

    public void HandleInit()
    {
        AddressableManager.Instance.OnreleaseHandle += Remove;
    }
    public void Remove()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}
