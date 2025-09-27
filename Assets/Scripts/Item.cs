using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Item : MonoBehaviour
{
    [SerializeField] private TMP_Text nickName;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text rank;
    public AsyncOperationHandle handle;
    public void Init(string nick,string sc, string ra, AsyncOperationHandle hand)
    {
        nickName.text = nick;
        score.text = $"Score : {sc}";
        rank.text = $"{ra}µî";
        handle = hand;
        HandleInit();
    }

    public void HandleInit()
    {
        AddressableManager.Instance.OnreleaseHandle += Remove;
    }
    public void Remove()
    {
        AddressableManager.Instance.ReleaseObj(handle);
    }
}
