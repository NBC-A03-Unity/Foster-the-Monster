using System;
using System.Collections.Generic;
using static Utility;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

public class EventManager : NonMonoSingleton<EventManager>
{
    EventData _data;
    List<AssetReference> eventReferenceList;

    public void Init()
    {
        _data = DataManager.Instance.eventData;
    }

    public async void Load(Action action = null)
    {
        if (eventReferenceList == null)
        {
            AssetReferenceList list = await ResourceManager.Instance.LoadResource<AssetReferenceList>("EventSOList");
            eventReferenceList = list.list;
        }

        

        if (await TryLoadEvent(action))
        {
            
            
        }
        else
        {
            action?.Invoke();
        }

    }
    private async Task<EventSO> LoadSO()
    {
        int count = eventReferenceList.Count;
        int index = Math.Abs(RandomInt() % count);
        return await ResourceManager.Instance.LoadResource<EventSO>(eventReferenceList[index]);
    }

    private async Task<bool> TryLoadEvent(Action action)
    {
        if (_data.eventDate.Contains(DataManager.Instance.date))
        {
            _data.eventDate.RemoveAt(0);
            await DefaultRandomEvent(action);
            return true;
        }
        else if(_data.goldDate.Contains(DataManager.Instance.date))
        {
            _data.goldDate.RemoveAt(0);
            await DefaultEvent("GoldEventData", action);
            return true;
        }
        else if (_data.bossDate == DataManager.Instance.date)
        {
            await WaringEvent("BossEventData", action);
            return true;
        }
    
        
        return false;
    }

    public async Task DefaultRandomEvent(Action action = null)
    {
        UIEvent uIEvent = UIManager.Instance.OpenUI<UIEvent>();
        EventSO thisEvnetSO = await LoadSO();
        uIEvent.Init(thisEvnetSO, action);
    }

    public async Task DefaultEvent(string key, Action action = null)
    {
        UIEvent uIEvent = UIManager.Instance.OpenUI<UIEvent>();
        EventSO thisEventSO = await ResourceManager.Instance.LoadResource<EventSO>(key);
        uIEvent.Init(thisEventSO, action);
    }

    public async Task WaringEvent(string key, Action action = null)
    {
        UIEvent uIEvent = UIManager.Instance.OpenUI<UIWaringEvent>();
        EventSO thisEventSO = await ResourceManager.Instance.LoadResource<EventSO>(key);
        uIEvent.Init(thisEventSO, action);
    }
    public int ReturnNextEventDate()
    {
        if (_data.eventDate.Count == 0) return 99;
        if (_data.eventDate[0] < DataManager.Instance.date)
        {
            _data.eventDate.RemoveAt(0);
            return ReturnNextEventDate();
        }
        return _data.eventDate[0];
    }

    public int ReturnNextGoldDate()
    {
        if (_data.goldDate.Count == 0) return 99;
        if (_data.goldDate[0] < DataManager.Instance.date)
        {
            _data.goldDate.RemoveAt(0);
            return ReturnNextEventDate();
        }
        return _data.goldDate[0];
    }
}