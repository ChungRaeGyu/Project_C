using Photon.Pun;
using UnityEngine;

public class Test : MonoBehaviour
{
    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        pv.RPC("TestRPC", RpcTarget.AllBuffered, "Hello, Photon!");
    }
    [PunRPC]
    void TestRPC(string message)
    {
        Debug.Log(message);
    }


}
