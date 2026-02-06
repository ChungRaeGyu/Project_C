using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    [SerializeField] private Transform content;

    private const string path = "Assets/Prefabs/Item.prefab";

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private async void OnEnable()
    {
        if (FirebaseFireStoreManager.Instance.isUpdate)
        {
            Debug.Log("OnEnable");
            var temp = await FirebaseFireStoreManager.Instance.RankingRead();
            await UpdateRanking(temp);

        }
    }

    private async Task UpdateRanking(QuerySnapshot temp)
    {
        int i = 1; 
        foreach (DocumentSnapshot document in temp.Documents)
        {
            Dictionary<string, object> documentDictionary = document.ToDictionary();
            Debug.Log("UpdateRanking");

            var handle = await AddressableManager.Instance.Instantiate(path, content);
            GameObject obj = await handle.Task;

            obj.GetComponent<Item>().Init(documentDictionary["NickName"].ToString(),
                documentDictionary["Score"].ToString(),
                i.ToString(),
                handle);
            i++;
        }
    }

}
