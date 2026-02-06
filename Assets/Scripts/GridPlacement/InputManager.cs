using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField] private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField] private LayerMask placemetLayermask;

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        //mousePos.x = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit; 
        if(Physics.Raycast(ray, out hit,Mathf.Infinity, placemetLayermask))
        {
            lastPosition = hit.point;
        }

        return lastPosition;

    }

/*    void OnDrawGizmos()
    {
        if (sceneCamera == null) return;

        // 현재 마우스 위치 기준 Ray
        Vector3 mousePos = Input.mousePosition;      
        

        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        // Ray 시각화
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placemetLayermask))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(ray.origin, hit.point);
            Gizmos.DrawSphere(hit.point, 0.2f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * Mathf.Infinity);
        }
    }
*/
}
