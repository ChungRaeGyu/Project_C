using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour
{
    public static AddressableManager Instance;
    List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();
    public Dictionary<string,Sprite> imageDictionary = new Dictionary<string, Sprite>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async Task<Sprite> LoadImage(string path)
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(path);
        Sprite loadedSprite = await handle.Task;

        Debug.Log("Image Load Success");
                //이미지 로드 성공 시 처리할 내용
                //패스를 가지고 위치를 받는다.
        handles.Add(handle);
        imageDictionary[path] = loadedSprite;
            
        return loadedSprite;
    }

    public void ReleaseAll()
    {
        foreach (var handle in handles)
        {
            Addressables.Release(handle);
        }
        handles.Clear();
    }
    /*  
     *  SO도 어드레서블을 사용해서 불러오려고 했었음  -> 근데 이게 용량이 크지 않는데 굳이 이럴 필요가 없다. 라는 결론
     *  그래서 Image만 어드레서블을 사용해서 받아오기로 함
        public void LoadSO()
        {
            AsyncOperationHandle handle = Addressables.LoadAssetsAsync<Unit>("SO", (so) =>
            {
                AsyncOperationHandle spriteHandle = Addressables.LoadAssetAsync<Sprite>(so.imagePath);
                Addressables.InstantiateAsync("Card", content.transform).Completed += (obj) =>
                {
                    obj.Result.GetComponent<Card>().unit = so;
                    obj.Result.GetComponent<Card>().descriptionPanel = descriptionPanel;
                };
            });
            handle.Completed += (obj) =>
            {
                //나중에 Release시켜주기 위함
                handles.Add(obj);
                print("완료");
            };
        }*/

}

