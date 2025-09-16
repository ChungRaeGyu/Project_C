using UnityEngine;

public class MoveToAnotherWay : UnitObj
{
    //다른 라인으로 이동시키기
    //이 캐릭터가 소환된 라인의 상대편 몬스터를 다른 라인으로 이동시킨다.
    void Start()
    {
        GameManager.Instance.CallRPC(new float[] { (int)EDeBuff.BRINGANYWAY,line,0 });
    }
}
