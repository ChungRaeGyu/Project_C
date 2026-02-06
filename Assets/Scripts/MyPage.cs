using TMPro;
using UnityEngine;

public class MyPage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TMP_Text text;
    PlayerData data;
    private void Awake()
    {
        data = PlayerData.Instance;
    }
    private void OnEnable()
    {
        text.text = $"닉네임 : {data.GetNickName()} \n점수 : {data.GetScore()}";
    }

    public void PageControl()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }


}
