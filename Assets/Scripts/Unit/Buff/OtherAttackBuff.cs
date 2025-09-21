using UnityEngine;

public class OtherAttackBuff : UnitObj
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int plusDamage = 1;
    void Start()
    {
        if (!pv.IsMine) return;

        for (int i = 0; i < 3; i++)
        {
            if (i != line)
            {
                foreach (GameObject obj in GameManager.Instance.objList[i])
                {
                    UnitObj unit = obj.GetComponent<UnitObj>();
                    unit.damage += plusDamage;
                }
            }
        }
    }
}
