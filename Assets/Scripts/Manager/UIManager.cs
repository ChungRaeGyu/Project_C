using Photon.Pun;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Transform[] canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PhotonNetwork.IsMasterClient) return;
        foreach(Transform t in canvas)
        {
            t.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

}
