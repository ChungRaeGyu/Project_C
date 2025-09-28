using Photon.Pun;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Transform[] canvas;

    Camera cam;
    Vector3 masterCamera = new Vector3(-0.43f, 20.92f, -15.72f);
    Vector3 slaveCamera = new Vector3(-0.43f, 20.92f, 29.5f);

    [SerializeField] Transform masterHP;
    [SerializeField] Transform slaveHP;

    [SerializeField] Transform slave0;
    [SerializeField] Transform slave2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        cam = Camera.main;
    }
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MasterSetting();
        }
        else
        {
            SlaveSetting();
        }
    }
    private void MasterSetting()
    {
        cam.transform.position = masterCamera;
    }
    private void SlaveSetting()
    {
        cam.transform.position = slaveCamera;
        cam.transform.rotation = Quaternion.Euler(45, 180, 0);
        HpSet();

    }

    private void HpSet()
    {
        @switch(masterHP, slaveHP);
        @switch(slave0, slave2);
    }

    private void @switch(Transform a, Transform b)
    {
        Transform temp = a;
        a = b;
        b = temp;
        a.gameObject.transform.position = a.position;
        b.gameObject.transform.position = b.position;
    }
}
