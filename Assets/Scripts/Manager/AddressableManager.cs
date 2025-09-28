using Photon.Pun;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour, IPunPrefabPool
{
    //handles을 이게 아니라 각 생성된 애들이 핸들을 보관하고
    //action event를 사용해서 event를 달아준다. 
    //그리고 ReleaseAll하면 event를 호출해서 삭제한다. 

    //내가 지금 하고 있는 것 뭔가를 만들때마다 handles에 딕에 넣어주고 해제하고 싶을때 다시 받아와서 해제한다.
    public static AddressableManager Instance;
    public event Action OnreleaseHandle;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public async Task<AsyncOperationHandle<Sprite>> LoadImage(string path)
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(path);
        await handle.Task;
        return handle;
    }
    public async Task<AsyncOperationHandle<GameObject>> Instantiate(string path, Transform parent = null)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(path, parent);
        GameObject loadedObject = await handle.Task;
        return handle;
    }
    public void ReleaseObj(AsyncOperationHandle obj)
    {
        //유닛 삭제 됌
        Addressables.ReleaseInstance(obj);
        Debug.Log("로컬삭제");
    }
    public void ReleaseImage(AsyncOperationHandle handle)
    {
        if (!handle.IsValid()) return;
        Addressables.Release(handle);
    }
    public void ReleaseAll()
    {
        OnreleaseHandle?.Invoke();
    }

    public GameObject Instantiate(string path, Vector3 position, Quaternion rotation)
    {
        // Addressables 키/주소가 prefabId라고 가정
        var handle = Addressables.InstantiateAsync(path, position, rotation, null);

        // 동기 대기 (간단하지만 hitch 가능)
        handle.WaitForCompletion();
        var go = handle.Result;
        UnitObj unitobj = go.GetComponent<UnitObj>();
        unitobj.handle = handle;
        unitobj.HandleInit();
        return go;
    }

    public void Destroy(GameObject gameObject)
    {
        Addressables.ReleaseInstance(gameObject);
        Debug.Log("삭제");
    }
    #region 포톤네트워크와의 연계를 위한
    /*    public GameObject Instantiate(string path, Vector3 position, Quaternion rotation)
        {
            Debug.Log("실행");
            // Addressables 키/주소가 prefabId라고 가정
            var handle = Addressables.InstantiateAsync(path, position, rotation, null);

            // 동기 대기 (간단하지만 hitch 가능)
            handle.WaitForCompletion();
            var go = handle.Result;
            UnitObj unitobj = go.GetComponent<UnitObj>();
            unitobj.handle = handle;
            unitobj.HandleInit();
            return go;
        }*/

    /*   public void Destroy(GameObject gameObject)
       {

           Addressables.ReleaseInstance(gameObject);
       }*/
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

