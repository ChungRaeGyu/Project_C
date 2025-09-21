using UnityEngine;

public class Consume : UnitObj
{
    //쉽게 하면 모든라인의 저코스트를 흡수한다. 
    //와 근데 그럼 너무 쌔지겠는데
    //2코스트 이하의 유닛을 잡아먹는다 다른 라인의
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!pv.IsMine) return;

        for (int i=0; i < 3; i++)
        {
            if (i != line)
            {
                foreach(GameObject obj in GameManager.Instance.objList[i])
                {
                    UnitObj unit = GetComponent<UnitObj>();
                    if (unit.cost <= 2)
                    {
                        damage += unit.damage;
                        AddressableManager.Instance.Destroy(obj);
                    }
                }
            }
        }   
    }
}
