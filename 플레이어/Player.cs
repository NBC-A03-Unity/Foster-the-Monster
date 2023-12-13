using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static Enums;

[Serializable]
public class Player
{
    #region Player Data
    public int MaxTenacity { get; private set; }
    public int CurrentTenacity { get; private set; }
    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }
    public int Stress { get; private set; }
    public int StressLimit { get; private set; }
    public int MaxJump { get; set; }
    public float Attack { get; set; }
    public float AttackSpeed {get; set;}
    public float MoveSpeed { get; set; }
    public int Gold { get; set; }
    #endregion

    #region Check Variables
    public bool IsRight { get; set; }
    public bool IsSwinging { get; set; }
    public bool IsTackle { get; set; }

    #endregion

    #region Items
    [JsonIgnore]
    public WeaponType WeaponType { get; private set; }
    
    [JsonIgnore]
    public Dictionary<ItemType, Dictionary<int, Item>> Inventory { get; private set; }
    [JsonIgnore]
    public ItemSO ThrowableItem { get; private set; }
    #endregion

    public Player(int tenacity, int hp, int stress, int jump, WeaponType type, float speed, int gold)
    {
        MaxTenacity = tenacity;
        CurrentTenacity = tenacity;
        MaxHP = hp;
        CurrentHP = hp;
        StressLimit = stress;
        Stress = 0;
        MaxJump = jump;
        WeaponType = type;
        MoveSpeed = speed;
        Gold = gold;
        Inventory = new Dictionary<ItemType, Dictionary<int, Item>>
        {
            { ItemType.Weapon, new  Dictionary<int, Item>() },
            { ItemType.Equipment, new  Dictionary<int, Item>() },
            { ItemType.Item, new  Dictionary<int, Item>() }
        };
    }

    #region Player Builder : 플레이어의 스탯 다수를 동시에 조작할 경우 체이닝하여 사용
    public Player SetTenacity(int tenacity)
    {
        MaxTenacity = tenacity;
        CurrentTenacity = tenacity;
        return this;
    }
    public Player SetHP(int hp)
    {
        MaxHP = hp;
        CurrentHP = hp;
        return this;
    }
    public Player SetStress(int stress)
    {
        StressLimit = stress;
        Stress = 0;
        return this;
    }

    public Player SetJump(int jump)
    {
        MaxJump = jump;
        return this;
    }

    public Player SetGold(int gold)
    {
        Gold = gold;
        return this;
    }

    public Player SetSpeed(float speed)
    {
        MoveSpeed = speed;
        return this;
    }

    public Player SetAttack(float attack)
    {
        Attack = attack;
        return this;
    }

    public Player SetAttackSpeed(float attackSpeed)
    {
        AttackSpeed = attackSpeed;
        return this;
    }

    public Player SetInventory()
    {
        Inventory.Clear();
        Inventory.Add(ItemType.Weapon, new Dictionary<int, Item>());
        Inventory.Add(ItemType.Equipment, new Dictionary<int, Item>());
        Inventory.Add(ItemType.Item, new Dictionary<int, Item>());
        return this;
    }
    #endregion

    public Player DecreaseTenacity(int damage)
    {
        CurrentTenacity -= damage;
        if(CurrentTenacity < 0)
        {
            CurrentTenacity = 0;
        }
        return this;
    }

    public Player DecreaseHP(int damage)
    {
        CurrentHP -= damage;
        if(CurrentHP < 0)
        {
            CurrentHP = 0;
        }
        return this;
    }

    public Player IncreaseStress(int stress)
    {
        Stress += stress;
        if(Stress > StressLimit)
        {
            Stress = StressLimit;
        }
        return this;
    }

    public Player ChangeWeapon(WeaponType type)
    {
        WeaponType = type;
        return this;
    }

    public Player ChangeGold(int gold)
    {
        Gold += gold;
        return this;
    }
    
    public void AddToInventory(Item item, int amount)
    {
        ItemType itemType = item.ItemSO.ItemType;
        int itemID = item.ItemSO.ItemID;


        if (!Inventory[itemType].ContainsKey(itemID))
        {
            Inventory[itemType].Add(itemID, item);
            Inventory[itemType][itemID].ItemCount += amount;
            if (itemType == ItemType.Weapon)
            {
                InventoryUI inventoryUI = UIManager.Instance.OpenUI<InventoryUI>();
                inventoryUI.SelectedItem = item.ItemSO;
                inventoryUI.EquipItem();
                DataManager.Instance.SavePlayerData(this);
                UIManager.Instance.CloseUI<InventoryUI>();
            }
        }

        else Inventory[itemType][itemID].ItemCount++;
    }

    public bool ThrowItem(ItemSO item)
    {
        Inventory[item.ItemType][item.ItemID].ItemCount--;
        if(Inventory[item.ItemType][item.ItemID].ItemCount < 0)
        {
            Inventory[item.ItemType].Remove(item.ItemID);
            ThrowableItem = null;
            return false;
        }
        return true;
    }

    public void SetThrowableItem(ItemSO item)
    {
        ThrowableItem = item;
    }
}
