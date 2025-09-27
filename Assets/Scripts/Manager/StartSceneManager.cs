using TMPro;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{

    [SerializeField] TMP_InputField nickNameInput;
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passInput;
    [SerializeField] GameObject Siginpanel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        FirebaseFireStoreManager.Instance.Init();
        FirebaseAuthManager.Instance.Init();
        FirebaseAuthManager.Instance.LoginState += OnChangedState;
        FirebaseAuthManager.Instance.SignState += SetData;
    }

    private void OnChangedState(bool sign)
    {
    }
    public void Create()
    {
        string e = emailInput.text;
        string p = passInput.text;
        FirebaseAuthManager.Instance.Creat(e, p);
    }

    private void SetData()
    {
        FirebaseFireStoreManager.Instance.WriteData(nickNameInput.text, emailInput.text, 0);
    }
    public void Login()
    {
        FirebaseAuthManager.Instance.Login(emailInput.text, passInput.text);

    }
    public void LogOut()
    {
        FirebaseAuthManager.Instance.LogOut();
    }

    public void SiginPanelControl()
    {
        Siginpanel.SetActive(!Siginpanel.activeSelf);
    }

}
