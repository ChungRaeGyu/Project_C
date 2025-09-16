using UnityEngine;

public class AttackBuff : UnitObj
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int plusDamage = 1;
    void Start()
    {
        foreach (GameObject obj in GameManager.Instance.objList[line])
        {
            UnitObj unit = obj.GetComponent<UnitObj>();
            unit.damage += plusDamage;
        }
    }
}
