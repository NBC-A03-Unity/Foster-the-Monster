using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectManager : Singleton<ObjectManager>
{
    private Dictionary<string, List<GameObject>> ObjectPool;
    protected override void Awake()
    {
        base.Awake();
        ObjectPool = new Dictionary<string, List<GameObject>>();
        DontDestroyOnLoad(gameObject);
    }
    public async Task<T> UsePool<T>(AssetReference address, Transform parent = null)
    {
        GameObject obj;
        string key;


        if (address.Asset == null)
        {
            var handle = address.LoadAssetAsync<GameObject>();
            await handle.Task;

        }

        key = address.Asset.name;
        AddKey(address.Asset.name);

        if (ObjectPool[key].Count <= 0)
        {
            obj = await address.InstantiateAsync(parent).Task;
            obj.name = key;

        }
        else
        {
            obj = ObjectPool[key][ObjectPool[key].Count - 1];
            ObjectPool[key].RemoveAt(ObjectPool[key].Count - 1);
            obj.transform.SetParent(parent);

       
            obj.SetActive(true);
            
        }
            

        return obj.GetComponent<T>();
    }

    public async Task<T> UsePool<T>(AssetReference address, Vector3 position, Transform parent = null)
    {
        GameObject obj;
        string key;

        if (address.Asset == null)
        {
            var handle = address.LoadAssetAsync<Object>();
            await handle.Task;
            AddKey(address.Asset.name);
        }

        key = address.Asset.name;

        if (ObjectPool[key].Count <= 0)
        {
            obj = await address.InstantiateAsync(
                position, Quaternion.identity, parent).Task;
            obj.name = key;
        }
        else
        {
            obj = ObjectPool[key][ObjectPool[key].Count - 1];
            ObjectPool[key].RemoveAt(ObjectPool[key].Count - 1);
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            obj.transform.rotation = Quaternion.identity;


            obj.SetActive(true);

        }


        return obj.GetComponent<T>();
    }

    public async Task <T> UsePool<T>(string addressableName, Transform parent = null)  where T : Component
    {
        AddKey(addressableName);

        if (ObjectPool[addressableName].Count <= 0)
        {
            var handle = Addressables.InstantiateAsync(addressableName, parent);
            await handle.Task;
            handle.Result.gameObject.name = addressableName;
            return handle.Result.GetComponent<T>();
        }
        else
        {
            int index = ObjectPool[addressableName].Count - 1;
            T obj = ObjectPool[addressableName][index].GetComponent<T>();
            ObjectPool[addressableName].RemoveAt(index);
            obj.transform.SetParent(parent);
            obj.gameObject.SetActive(true);
            return obj;
        }
        
    }

    public async Task <T> UsePool<T>(string addressableName, Vector3 position, Transform parent = null) where T : Component
    {
        AddKey(addressableName);

        if (ObjectPool[addressableName].Count <= 0)
        {
            var handle = Addressables.InstantiateAsync(addressableName, position, Quaternion.identity, parent);
            await handle.Task;
            handle.Result.gameObject.name = addressableName;
            return handle.Result.GetComponent<T>();
        }
        else
        {
            int index = ObjectPool[addressableName].Count - 1;
            T obj = ObjectPool[addressableName][index].GetComponent<T>();
            ObjectPool[addressableName].RemoveAt(index);
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            obj.gameObject.SetActive(true);
            return obj;
        }
    }


    public void ReturnPool(GameObject go)
    {
        if (go ==null) return;  
        string key = go.name;
        ObjectPool[key].Add(go);
        go.transform.SetParent(transform);
        go.SetActive(false);
    }

    public void ReturnPool<T>(T component) where T : Component
    {
        if (component == null) return;
        GameObject go = component.gameObject;
        ReturnPool(go);
    }

    public void AddKey(string key)
    {
        if (!ObjectPool.ContainsKey(key))
        {
            ObjectPool.Add(key, new List<GameObject>());
        }
    }

    public void Clear()
    {
        foreach( List<GameObject> lgo in ObjectPool.Values) 
        {
            foreach ( GameObject go in lgo )
            {
                Destroy(go);
            }
            lgo.Clear();
        }
        ObjectPool.Clear();
    }

}
