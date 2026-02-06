using UnityEngine;

public class AttackSpeedBuff : UnitObj
{
    // Start is called
    // once before the first execution of Update after the MonoBehaviour is created
    // AttackSpeed를 컨트롤
    // 모든건 소환효과다
    [SerializeField] float atkSpeed = 1.2f;
    void Start()
    {
        if (!pv.IsMine) return;

        foreach (GameObject obj in GameManager.Instance.objList[line])
        {
            UnitObj unit = obj.GetComponent<UnitObj>();
            unit.attackSpeed = unit.speed * atkSpeed;
        }
    }
}
