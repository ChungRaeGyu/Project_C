using UnityEngine;

public class AttackDeBuff : UnitObj
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] int dmgDown=2;
    void Start()
    {
        if (!pv.IsMine) return;

        GameManager.Instance.CallRPC(new float[] {(int)EDeBuff.ATKDOWN,line, dmgDown});   
    }
}
