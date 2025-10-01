using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator,cellIndiccator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    //피벗은 항상 왼쪽 밑;
    private void Update()
    {
        //마우스 포지션에 있는 땅레이어에 부딪힌 지점을 받아와서 표현
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndiccator.transform.position = grid.CellToWorld(gridPosition);
    }
}
