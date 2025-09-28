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
        if (!pv.IsMine) return;

        for (int i=0; i < 3; i++)
        {
            if (i != line && GameManager.Instance.objList[i].Count<4 && GameManager.Instance.lineBool[i])
            {
                Vector3 pos = PhotonNetwork.IsMasterClient ? GameManager.Instance.spawnPosition[i].position : GameManager.Instance.spawnPosition[i + 3].position;
                GameObject obj = PhotonNetwork.Instantiate($"Assets/Prefabs/Unit/{babyWolf.unitName}.prefab", pos, Quaternion.identity);
                if (!PhotonNetwork.IsMasterClient) obj.transform.rotation = Quaternion.Euler(0, 180, 0);
                UnitObj unitObj = obj.GetComponent<UnitObj>();
                unitObj.Init(line, babyWolf.unitName, babyWolf.cost, babyWolf.damage, babyWolf.attackSpeed, babyWolf.speed);
                GameManager.Instance.LindAdd(obj, i);
                //소환위치
            }
        }   
    }

}
//디버프 관련 메서드를 모두 알고 있는 메서드를 만들기


