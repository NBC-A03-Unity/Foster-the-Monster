using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourceManager : NonMonoSingleton<ResourceManager>
{
    Dictionary<System.Type, Dictionary<string, Object>> dic = new Dictionary<System.Type, Dictionary<string, Object>>();

    Dictionary<string, UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle> handleDic = new Dictionary<string, UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>();
    public async Task<T> LoadResource<T>(string key) where T : Object
    {
        if (!handleDic.ContainsKey(key))
        {
            handleDic.Add(key, Addressables.LoadAssetAsync<Object>(key));
        }
        await handleDic[key].Task;
        return handleDic[key].Result as T;
    }
    public async Task<T> LoadResource<T>(AssetReference address) where T : Object
    {

        if (!handleDic.ContainsKey(address.AssetGUID))
        {
            handleDic.Add(address.AssetGUID, address.LoadAssetAsync<Object>());
        }

        await handleDic[address.AssetGUID].Task;

        if (address.Asset != null)
        {
            return address.Asset as T;
        }
        else if (handleDic[address.AssetGUID].Result != null)
        {
            return handleDic[address.AssetGUID].Result as T;
        }
        else
        {
            return null;
        }
        
    }

    public T LoadPrefab<T> (string path, string name) where T : UnityEngine.Object
    {
        System.Type type = typeof(T);
        if (!dic.ContainsKey(type))
        {
            dic.Add(type, new Dictionary<string, UnityEngine.Object>());
        }

        if (!dic[type].ContainsKey(name))
        {
            dic[type].Add(name, Resources.Load<T>(path));
        }
        return (T)dic[type][name];
    }
}
