using Photon.Pun.Demo.SlotRacer.Utils;
using UnityEngine;

public class AttackSpeedDeBuff : UnitObj
{
    [SerializeField] float atkSpeedDown = 0.2f;//20퍼센트 정도 낮추는 거다. 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!pv.IsMine) return;
        GameManager.Instance.CallRPC(new float[] { (int)EDeBuff.ATKDOWN, line, 1-atkSpeedDown });
    }
}
