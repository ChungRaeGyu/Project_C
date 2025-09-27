using TMPro;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passInput;
    [SerializeField] TMP_Text outputText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        FirebaseFireStoreManager.Instance.Init();
        FirebaseAuthManager.Instance.Init();
        FirebaseAuthManager.Instance.LoginState += OnChangedState;
    }

    private void OnChangedState(bool sign)
    {
        outputText.text = sign ? "Login" : "LogOut";
        //outputText.text += FirebaseAuthManager.Instance.UserId;
    }
    public void Create()
    {
        string e = emailInput.text;
        string p = passInput.text;
        FirebaseAuthManager.Instance.Creat(e, p);
    }
    public void Login()
    {
        FirebaseAuthManager.Instance.Login(emailInput.text, passInput.text);

    }
    public void LogOut()
    {
        FirebaseAuthManager.Instance.LogOut();
    }

}
