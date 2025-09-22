
using NUnit.Framework;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Consume : UnitObj
{
    //쉽게 하면 모든라인의 저코스트를 흡수한다. 
    //와 근데 그럼 너무 쌔지겠는데
    //2코스트 이하의 유닛을 잡아먹는다 다른 라인의
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    List<GameObject> list = new List<GameObject>();
    void Start()
    {

        if (!pv.IsMine) return;
        for (int i=0; i < 3; i++)
        {
            if (i != line)
            {
                foreach (GameObject obj in GameManager.Instance.objList[i])
                {
                    UnitObj unit = obj.GetComponent<UnitObj>();
                    if (unit.cost <= 2)
                    {
                        damage += unit.damage;
                        list.Add(obj);
                        PhotonNetwork.Destroy(obj);
                    }
                }
                if(list.Count > 0)
                {
                    RemoveObject(i);
                }
            }
        }
        Debug.Log("강해짐 : " + damage);
    }
    void RemoveObject(int line)
    {
        foreach(GameObject obj in list)
        {
            GameManager.Instance.objList[line].Remove(obj);
        }
        list.Clear();
    }
}
