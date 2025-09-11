using Photon.Pun;
using Photon.Pun.Demo.SlotRacer.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class GhostObject : MonoBehaviour
{
    [HideInInspector]
    public Card card;
    Color red = new Color(1f, 0f, 0f, 0.2f);
    Color origin;
    private void Start()
    {
        origin = GetComponent<Renderer>().material.color;
    }
    private void Spawn()
    {
        //유닛 소환
        //고스트 삭제
        int line = GetLine();
        //설치 조건 몬스터가 겹치[는가 , 라인이 살아있는가, 최대값을 임의로 4로 지정
        if (CanPlaced(transform.position,line))
        {
            //고스트 삭제, 유닛 소환
            //GameObject obj= await AddressableManager.Instance.Instantiate($"Assets/Prefabs/{card.unit.unitName}.prefab", null); //유닛 소환
            GameObject obj = PhotonNetwork.Instantiate($"Assets/Prefabs/{card.unit.unitName}.prefab", transform.position, Quaternion.identity);
            UnitObj unitObj = obj.GetComponent<UnitObj>();
            unitObj.line = line;
            unitObj.unit = card.unit;
            GameManager.Instance.LindAdd(obj, line);
            obj.transform.position = transform.position;
            GameManager.Instance.cost -= card.unit.cost;
            card.CardRemove(); //카드 삭제
            //카드 삭제
        }
        else
        {
            card.image.enabled = true;
            //놓을 수 없는 곳입니다.
            //
            Debug.Log("놓을 수 없는 곳입니다.");
        }
        AddressableManager.Instance.ReleaseObj(this.gameObject); //고스트 삭제

    }

    private int GetLine()
    {
        List<float> temp = new List<float>();
        foreach (var t in GameManager.Instance.tPosition)
        {
            float distance = Vector3.Distance(transform.position, t.position);
            temp.Add(distance);
        }
        int num = temp.IndexOf(temp.Min());
        return num;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그");

    }

    private bool CanPlaced(Vector3 pos,int line)
    {
        LayerMask buildBlockLayer = LayerMask.GetMask("Monster","Wall");
        Collider[] colliders = Physics.OverlapSphere(pos, 0.5f, buildBlockLayer);
        return colliders.Length == 0 && GameManager.Instance.lineBool[line] && GameManager.Instance.objList[line].Count < 4;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero); // y=0 평면
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance); // 레이가 평면과 교차한 지점
            transform.position = hitPoint;
        }
        if (CanPlaced(transform.position, GetLine()))
        {
            GetComponent<Renderer>().material.color = origin;
        }
        else
        {
            GetComponent<Renderer>().material.color = red;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Spawn();
        }
    }
    //여기서 일단 드래그 중인걸 표현하고? PointerUp하면 실제 소환하는 거다.
    //마우스 따라오기 PointerUp이 필요하네
}
