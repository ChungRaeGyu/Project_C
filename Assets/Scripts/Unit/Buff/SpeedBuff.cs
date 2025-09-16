using UnityEngine;
using UnityEngine.AI;

public class SpeedBuff : UnitObj
{
    // Start is called once bef
    // ore the first execution of Update after the MonoBehaviour is created
    //SpeedControl
    //소환효과
    [SerializeField] float movSpeed = 1.2f;
    void Start()
    {
        //같은 라인의 모든 유닛의 이동속도를 상승시킨다. 20퍼 상승
        foreach (GameObject obj in GameManager.Instance.objList[line])
        {
            UnitObj unit = obj.GetComponent<UnitObj>();
            unit.agent.speed = unit.speed* movSpeed;
        }
    }
}
