using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    
    public GameObject mouseIndicator;
    [SerializeField]
    private GameObject cellIndiccator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    //피벗은 항상 왼쪽 밑;
    private void Update()
    {
        if (mouseIndicator == null) return;
        //마우스 포지션에 있는 땅레이어에 부딪힌 지점을 받아와서 표현
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(new Vector3 (mousePosition.x,1,mousePosition.z));
        mouseIndicator.transform.position = mousePosition;

        cellIndiccator.transform.position = grid.CellToWorld(gridPosition);
    }

    public void StartPlace(GameObject c)
    {
        mouseIndicator = c;
        cellIndiccator.SetActive(true);
        GameManager.Instance.SpawnArea(true);
    }
    public void EndPlace()
    {
        mouseIndicator = null;
        cellIndiccator.SetActive(false);
        GameManager.Instance.SpawnArea(false);

    }
}
