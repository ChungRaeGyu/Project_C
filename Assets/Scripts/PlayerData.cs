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
    public void Init(string n,string i,int s)
    {
        nickName = n;
        id = i;
        score = s;
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
    public void AddScore(int s)
    {
        currentScore += s;
        currentScore = currentScore < 0 ? 0 : currentScore;
        if (currentScore != score)
        {
            FirebaseFireStoreManager.Instance.UpdateField(currentScore);
            score = currentScore;
        }
    }

}
