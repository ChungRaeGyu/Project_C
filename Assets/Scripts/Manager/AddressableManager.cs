using Photon.Pun;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class AddressableManager : MonoBehaviour, IPunPrefabPool
{
    public static AddressableManager Instance;
    Dictionary<string,AsyncOperationHandle<Sprite>> handles = new Dictionary<string,AsyncOperationHandle<Sprite>>();

    Dictionary<GameObject,AsyncOperationHandle<GameObject>> objHandles = new Dictionary<GameObject, AsyncOperationHandle<GameObject>>();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async Task<Sprite> LoadImage(string path)
    {
        if (handles.ContainsKey(path))
        {
            Sprite sprite = handles[path].Result;
            return sprite;
        }
        else
        {
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(path);
            Sprite loadedSprite = await handle.Task;

            //이미지 로드 성공 시 처리할 내용
            //패스를 가지고 위치를 받는다.
            handles[path]=handle;

            return loadedSprite;
        }

    }

    public async Task<GameObject> Instantiate(string path, Transform parent = null)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(path, parent);
        GameObject loadedObject = await handle.Task;


        objHandles.Add(loadedObject,handle);
            
        return loadedObject;
    }
    public void ReleaseObj(GameObject obj)
    {
        //유닛 삭제 됌
        Addressables.ReleaseInstance(obj);
        objHandles.Remove(obj);
    }
    public void ReleaseImage(string sprite)
    {
        Addressables.Release(handles[sprite]);
        handles.Remove(sprite);
    }
    public void ReleaseAll()
    {
        foreach(var handle in objHandles)
        {
            Addressables.ReleaseInstance(handle.Value);
        }
        foreach (var handle in handles)
        {
            Addressables.Release(handle.Value);
        }
        handles.Clear();
        objHandles.Clear();
    }
    #region 포톤네트워크와의 연계를 위한
    public GameObject Instantiate(string path, Vector3 position, Quaternion rotation)
    {
        // Addressables 키/주소가 prefabId라고 가정
        var handle = Addressables.InstantiateAsync(path, position, rotation, null);

        // 동기 대기 (간단하지만 hitch 가능)
        handle.WaitForCompletion();

        var go = handle.Result;
        objHandles.Add(go, handle);
        return go;
    }

    public void Destroy(GameObject gameObject)
    {

        Addressables.ReleaseInstance(objHandles[gameObject]);
        objHandles.Remove(gameObject);
    }
    #endregion
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

