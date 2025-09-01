using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Text text;

    void Start()
    {
        text.GetComponent<Text>();
    }
    private void Update()
    {
        text.text = PhotonNetwork.IsMasterClient ? "∏∂Ω∫≈Õ" : "º’¥‘";
    }


}
