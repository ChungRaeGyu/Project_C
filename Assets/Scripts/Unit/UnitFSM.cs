using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    SEARCH,
    ATTACK
}
public class UnitFSM : MonoBehaviour
{
/*    
 *  문제 : 
    장애물에 너무 가까이 간다  
*/
    
    public State currentState;
    Collider col;
    Bounds bounds; //캐릭터의 Collider크기
    Collider[] searchCo = new Collider[10];

    private LayerMask layerMask;
    [HideInInspector]
    public NavMeshAgent agent; //여기서 속도 조절 해줘야한다.
    private UnitObj unitObj; //나중에 직렬화 없애줄꺼임
    private bool target = false;

    private double nextAttackTime = 0f;

    private Obstacle obstacle;

    private Animator animator;

    private PhotonView pv;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
        unitObj = GetComponent<UnitObj>();
        animator = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!pv.IsMine)
        {
            agent.enabled = false;
            col.enabled = false;
            return;
        }
        bounds = col.bounds;
        layerMask = LayerMask.GetMask("Obstacle"); //장애물 넣어주기
        agent.speed = unitObj.speed;
        GoToGoal();
        //첫 시작은 해야지
    }


    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine) return;
        switch (currentState)
        {
            case State.SEARCH:
                Search();
                break;
            case State.ATTACK:
                Attack();
                break;
        }
    }

    private void Attack()
    {
        //공격메서드
        //공격속도에 맞춰 공격 애니메이션 재생 및 데미지 주기, 어디에? 방해물에 공격자체는 또 그 오버렙으로 해버리고
        if (PhotonNetwork.Time < nextAttackTime) {
            return;
        }
        else
        {
            nextAttackTime = PhotonNetwork.Time + unitObj.attackSpeed;
            animator.SetTrigger("Attack");
            obstacle.GetDamage(unitObj.damage);
        }  // 아직 쿨타임 남음  
            
    }

    public void GoToGoal()
    {
        //장애물이 없어졌을때
        agent.SetDestination(GameManager.Instance.tPosition[unitObj.line].transform.position);
        Debug.Log("장애물 죽음");
        ChangeState(State.SEARCH);
    }

    private void Search()
    {
        animator.SetTrigger("Run");
        //장애물을 찾고 발견하면 부수고 지나가야 한다.
        if (!target)
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, bounds.extents.magnitude * 2, searchCo, layerMask);
            if (count > 0)
            {

                //장애물 발견, 장애물을 목표로 지정해주어야 한다. **이거 나중에 포지션값때매 정중앙으로 모일꺼 같아**
                target = agent.SetDestination(searchCo[0].transform.position);
            }
        }
        else
        {
            //목표에 도착하면 

            if (agent.remainingDistance < bounds.extents.magnitude * 1.5f)
            {
                agent.ResetPath();//목표 초기화 //그자리에 멈춘다.
                target = false;
                //여러마리를 설치하니까 여기서 null뜸
                obstacle = searchCo[0].gameObject.GetComponent<Obstacle>();
                obstacle.dieEvent += GoToGoal; //장애물이 사라졌을때 받을 이벤트
                ChangeState(State.ATTACK);
            }
            
        }

    }
    private void ChangeState(State state)
    {
        currentState = state;
    }

    void OnDrawGizmos()
    {

        // 코드에서 사용한 반지름 계산
        float radius = bounds.extents.magnitude * 2;

        // 구체 색상 설정
        Gizmos.color = Color.red;

        // 구체 그리기
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}
