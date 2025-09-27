using UnityEngine;
using Firebase.Auth;
using System;

public class FirebaseAuthManager
{
    private static FirebaseAuthManager instance;
    public static FirebaseAuthManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new FirebaseAuthManager();
            }
            return instance; 
        }
    }
    private FirebaseAuth auth;//로그인/회원가입에 사용
    private FirebaseUser user;// 인증이 완료된 유저 정보
    public string UserId => user.UserId; //UserId를 반환하는 역할만 함(읽기전용)

    public Action<bool> LoginState;
    public event Action SignState;
    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null) LogOut();
        auth.StateChanged += OnChanged;
    }
    private void OnChanged(object sender,EventArgs e)
    {
        if(auth.CurrentUser == null)
        {
            Debug.Log("로그아웃");
            user = null;
            LoginState?.Invoke(false);
        }
        else
        {
            user = auth.CurrentUser; //근데 만약 이걸위한 user였으면 쓸 필요가 없겠는데
            Debug.Log("로그인");
            LoginState?.Invoke(true);

        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Creat(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("회원가입 실패" + task.Exception); 

                // AggregateException 내부 메시지 보기 (좀 더 읽기 쉽게)
                foreach (var ex in task.Exception.Flatten().InnerExceptions)
                    Debug.LogError("Inner: " + ex.Message);
                return;
            }
            AuthResult result = task.Result;
            FirebaseUser newUser = result.User;
            Debug.LogError("회원가입 완료");
            SignState?.Invoke();
        });
    }
    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("로그인 실패");
                return;
            }
            AuthResult result = task.Result;
            FirebaseUser newUser = result.User;
            Debug.LogError("로그인 완료");

            FirebaseFireStoreManager.Instance.ReadData();

            NetworkManager.Instance.ConnectBtn();//로그인 눌렀을때 다 데이터 넣고 씬넘기기

            //새로운 사람이면 닉네임 정하는 칸 만들어주기
        });
    }

    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }
}
