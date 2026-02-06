using UnityEngine;

public class EndPanel : MonoBehaviour
{
    //보여줄 UI정하기
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject win;
    [SerializeField] GameObject lose;

    public void Init(bool Iswin)
    {
        if (Iswin)
        {
            win.SetActive(true);
        }
        else
        {
            lose.SetActive(true);
        }
    }
}
