using Photon.Pun;
using UnityEngine;

public class SpawnLowCost : UnitObj
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //몇마리나 소환할까나~ 일단 늑대만 하자 
    //소환 위치 지정하기 
    [SerializeField] Unit babyWolf;
    void Start()
    {
        for(int i=0; i < 3; i++)
        {
            if (i != line && GameManager.Instance.objList[i].Count<4)
            {
                GameObject obj = PhotonNetwork.Instantiate($"Assets/Prefabs/{babyWolf.unitName}.prefab", transform.position, Quaternion.identity);
                UnitObj unitObj = obj.GetComponent<UnitObj>();
                unitObj.Init(line, babyWolf.unitName, babyWolf.cost, babyWolf.damage, babyWolf.attackSpeed, babyWolf.speed);
                GameManager.Instance.LindAdd(obj, i);
                obj.transform.position = PhotonNetwork.IsMasterClient?GameManager.Instance.spawnPosition[i]: GameManager.Instance.spawnPosition[i+3];

                //소환위치
            }
        }   
    }

}
//디버프 관련 메서드를 모두 알고 있는 메서드를 만들기


