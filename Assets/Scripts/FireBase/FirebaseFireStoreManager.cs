using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class FirebaseFireStoreManager
{
    private const string collection = "PlayerList";
    private const string nickName = "NickName";
    private const string userId = "ID";
    private const string score = "Score";

    public bool isUpdate = true;
    private static FirebaseFireStoreManager instance;
    public static FirebaseFireStoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FirebaseFireStoreManager();
            }
            return instance;
        }
    }
    FirebaseFirestore db;
    public void Init()
    {
        db = FirebaseFirestore.DefaultInstance;
    }
    public void WriteData(string nickname,string email, int sco)
    {
        Debug.Log("데이터를 저장합니다.");   
        DocumentReference docRef = db.Collection(collection).
        Document(FirebaseAuthManager.Instance.UserId);
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add(nickName, nickname);
        data.Add(userId, email);
        data.Add(score, sco);

        docRef.SetAsync(data).ContinueWith(task =>
        {
            Debug.Log($"쓰기완료 nickName : {nickname},userId : {email}, score : { sco}");
        });

        Debug.Log("새로운 데이터 생성");
        PlayerData.Instance.Init(nickname, userId, sco);
    }
    public async void ReadData()
    {
        DocumentSnapshot player =await db.Collection(collection).Document(FirebaseAuthManager.Instance.UserId).GetSnapshotAsync();
        var dic = player.ToDictionary();
        if (player.Exists)
        {
            Debug.Log("데이터를 불러옵니다");

            PlayerData.Instance.Init(dic[nickName].ToString(), dic[userId].ToString(), int.Parse(dic[score].ToString()));
        }

    }
    public async Task<QuerySnapshot> RankingRead()
    {
        Debug.Log("랭킹");
        CollectionReference playlistref = db.Collection(collection);
        Query query = playlistref.OrderByDescending(score).Limit(10);//10개까지 뽑기

        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        isUpdate = false;
        return snapshot;

    }

    public void UpdateField(int newValue)
    {
        DocumentReference docRef = db.Collection(collection).Document(FirebaseAuthManager.Instance.UserId);
        var updates = new Dictionary<string, object> { { score, newValue } };
        docRef.UpdateAsync(updates).ContinueWithOnMainThread(t =>
        {
            if (t.IsCompleted) Debug.Log("업데이트 성공");
            else Debug.LogError("업데이트 실패: " + t.Exception);
        });
        isUpdate = true;
    }
}
// 점수를 내림차순으로, 점수가 같으면 이름을 오름차순으로 정렬
//Query query = usersRef.OrderBy("score", Direction.Descending).OrderBy("name", Direction.Ascending);
// 점수를 내림차순으로, 점수가 같으면 이름을 오름차순으로 정렬
//Query query = usersRef.OrderBy("score", Direction.Descending).OrderBy("name", Direction.Ascending);

