using System;

public class PlayerData
{
    private static PlayerData instance;
    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerData();
            }
            return instance;
        }
    }
    private string nickName;
    private string id;
    private int score;
    private int currentScore;
    private bool newPlayer;
    public void Init(string n,string i,int s, bool b)
    {
        nickName = n;
        id = i;
        score = s;
        newPlayer = b;
        currentScore = score;
    }
    public string GetNickName()
    {
        return nickName;
    }
    public string GetId()
    {
        return id;
    }
    public int GetScore()
    {
        return score;
    }
    public bool GetBool()
    {
        return newPlayer;
    }
    public void AddScore(int s)
    {
        currentScore += s;
        currentScore = Math.Min(currentScore, 0);
        if (currentScore != score)
        {
            FirebaseFireStoreManager.Instance.UpdateField(currentScore);
            score = currentScore;
        }
    }

}
