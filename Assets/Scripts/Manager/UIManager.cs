using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text[] canvas;
    private int[] countA = new int[6];
    private const string UnitCount = "UnitCount";
    private const string Line = "Line";

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
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (!propertiesThatChanged.ContainsKey(Line)) return;
        int line = int.Parse(propertiesThatChanged[Line].ToString());
        int count = int.Parse(propertiesThatChanged[UnitCount].ToString());
        countA[line] += count;
        UpdateLineCount(line);
    }
    private void UpdateLineCount(int line)
    {
        canvas[line].text = $"{countA[line].ToString()}/4";
    }
    private void MasterSetting()
    {
        cam.transform.position = masterCamera;
        //인원수 세팅
    }

    private void Rotateobj()
    {
        @switch(canvas[0].gameObject.transform, canvas[2].gameObject.transform);
        @switch(canvas[3].gameObject.transform, canvas[5].gameObject.transform);

        @switch(canvas[0].gameObject.transform, canvas[3].gameObject.transform);
        @switch(canvas[2].gameObject.transform, canvas[5].gameObject.transform);


    }

    private void SlaveSetting()
    {
        cam.transform.position = slaveCamera;
        cam.transform.rotation = Quaternion.Euler(45, 180, 0);
        HpSet();
        Rotateobj();
    }

    private void HpSet()
    {
        @switch(masterHP, slaveHP);
        @switch(slave0, slave2);
    }

    private void @switch(Transform a, Transform b)
    {
        Vector3 temp = a.position;
        a.position = b.position;
        b.position = temp;
    }
}
