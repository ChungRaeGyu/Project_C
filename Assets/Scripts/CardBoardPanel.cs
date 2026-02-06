using UnityEngine;
using UnityEngine.UI;

public class CardBoardPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] forestView; //Ãß°¡ µ¦
    [SerializeField] private Image[] buttonColor;
    private Color color = new Color(1,1,1,0.5f);
    private GameObject currentView;
    private Image currentButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentView = forestView[0];
        currentButton = buttonColor[0];
    }

    public void BtnChangeView(int num)
    {
        if (currentView != null)
        {
            currentView.SetActive(false);
            currentButton.color = color;
        }
        forestView[num].SetActive(true);
        buttonColor[num].color = new Color(1, 1, 1, 1);
        currentView = forestView[num];
        currentButton = buttonColor[num];
    }
}
