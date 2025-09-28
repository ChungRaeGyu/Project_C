using TMPro;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{

    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passInput;
    [SerializeField] GameObject Siginpanel;


    [SerializeField] TMP_InputField signnickNameInput;
    [SerializeField] TMP_InputField signemailInput;
    [SerializeField] TMP_InputField signpassInput;



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
        string e = signemailInput.text;
        string p = signpassInput.text;
        FirebaseAuthManager.Instance.Creat(e, p);
    }

    private void SetData()
    {
        FirebaseFireStoreManager.Instance.WriteData(signnickNameInput.text, signemailInput.text, 0);
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
