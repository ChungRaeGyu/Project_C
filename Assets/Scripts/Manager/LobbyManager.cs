using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Button quickMatchBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quickMatchBtn.onClick.AddListener(()=>
        {
            NetworkManager.Instance.QuickMatchBtn();
        });
    }

}
