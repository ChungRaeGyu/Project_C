using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Button quickMatchBtn;
    [Header("CardPanel")]
    public Button cardCheckBtn;
    public Button closeCardPanel;
    public GameObject cardPanel;
    [Header("DescriptionPanel")]
    public GameObject descriptionPanel;
    public Button closeDescriptionPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quickMatchBtn.onClick.AddListener(()=>
        {
            NetworkManager.Instance.QuickMatchBtn();
        });
        cardCheckBtn.onClick.AddListener(OpenCardBoard);
        closeCardPanel.onClick.AddListener(OpenCardBoard);
        closeDescriptionPanel.onClick.AddListener(OpenDescriptionPanel);
    }

    private void OpenCardBoard()
    {
        cardPanel.SetActive(!cardPanel.activeSelf);
    }
    private void OpenDescriptionPanel()
    {
        //카드를 클릭했을때 열리도록 만들면 된다.
        //카드의 기본정보를 넣고
        descriptionPanel.SetActive(!descriptionPanel.activeSelf);
    }

}
