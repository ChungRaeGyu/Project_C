using ExitGames.Client.Photon;
using System.Threading;
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
    private async void Spawn()
    {
        //유닛 소환
        //고스트 삭제
        if(CanPlaced(transform.position))
        {
            //고스트 삭제, 유닛 소환
            _= await AddressableManager.Instance.Instantiate($"Assets/Prefabs/{card.unit.unitName}.prefab", null); //유닛 소환
            card.CardRemove(); //카드 삭제
            //카드 삭제
            //게임매니저에 있는 currenthand에서 이 카드 Unit으로 찾아서 삭제
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
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그");

    }

    private bool CanPlaced(Vector3 pos)
    {
        LayerMask buildBlockLayer = LayerMask.GetMask("Monster","Wall");
        Collider[] colliders = Physics.OverlapSphere(pos, 0.5f, buildBlockLayer);
        return colliders.Length == 0;
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
        if (CanPlaced(transform.position))
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
