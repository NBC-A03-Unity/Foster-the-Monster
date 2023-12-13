using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Enums;

public class InventoryUI : MonoBehaviour
{
    #region Keys
    private const string _playerTag = "Player";
    #endregion

    #region Components
    [SerializeField] private UnityEngine.UI.Image[] _itemSlots;

    [SerializeField] private TextMeshProUGUI _weaponText;
    [SerializeField] private TextMeshProUGUI _equipmentText;
    [SerializeField] private TextMeshProUGUI _otherText;

    [SerializeField] private UnityEngine.UI.Button _weaponTab;
    [SerializeField] private UnityEngine.UI.Button _equipmentTab;
    [SerializeField] private UnityEngine.UI.Button _itemTab;
    [SerializeField] private UnityEngine.UI.Button _equipBtn;

    [SerializeField] private UnityEngine.UI.Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemDescription;
    [SerializeField] private TextMeshProUGUI _itemType;
    [SerializeField] private TextMeshProUGUI _itemRarity;
    [SerializeField] private TextMeshProUGUI _itemCount;
    [SerializeField] private TextMeshProUGUI _equipBtnText;

    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _tenacityText;
    [SerializeField] private TextMeshProUGUI _stressText;
    [SerializeField] private TextMeshProUGUI _jumpText;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _attackSpeedText;

    [SerializeField] private UnityEngine.UI.Image _weaponCoreImage;
    [SerializeField] private UnityEngine.UI.Image _equipmentCoreImage;
    [SerializeField] private UnityEngine.UI.Image _otherCoreImage;

    [SerializeField] private Sprite _emptySlotImage;

    private UnityEngine.UI.Image _currentWeaponImage;
    private UnityEngine.UI.Image _currentItemImage;


    private Player _player;
    private PlayerController _playerController;
    #endregion

    #region Variables
    private ItemType _currentTab;
    public ItemSO SelectedItem;
    public ItemSO CurrentWeapon { get; private set; }
    private ItemSO _currentEquipment;
    private ItemSO _currentOther;
    #endregion

    private void Start()
    {
        _weaponTab.onClick.AddListener(ShowWeapon);
        _equipmentTab.onClick.AddListener(ShowEquipment);
        _itemTab.onClick.AddListener(ShowOther);
        _equipBtn.onClick.AddListener(EquipItem);

        _currentWeaponImage = GameObject.FindGameObjectWithTag("WeaponImage").GetComponent<UnityEngine.UI.Image>();
        _currentItemImage = GameObject.FindGameObjectWithTag("ItemImage").GetComponent<UnityEngine.UI.Image>();

        ShowWeapon();
        ShowPlayerStats();
    }

    private void OnEnable()
    {
        if (_player == null) _player = DataManager.Instance.Player;
        if(_playerController == null) _playerController = GameObject.FindGameObjectWithTag(_playerTag).GetComponent<PlayerController>();
        ShowItems(_currentTab);
        ShowPlayerStats();
    }


    private void ShowItems(ItemType itemType)
    {
        Dictionary<int, Item> currentInventory = _player.Inventory[itemType];
        int i = 0;

        foreach(var slot in _itemSlots)
        {
            slot.transform.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        }

        foreach(Item item in currentInventory.Values)
        {
            _itemSlots[i].sprite = item.ItemSO.ItemSprite;
            _itemSlots[i].transform.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ShowItemSpecs(item.ItemSO));
            i++;
        }
    }

    public void ShowWeapon()
    {
        ClearItemSlots();
        _currentTab = ItemType.Weapon;
        ShowItems(ItemType.Weapon);
        _weaponText.color = Color.white;
        _equipmentText.color = new Color(1, 1, 1, 50f / 255);
        _otherText.color = new Color(1, 1, 1, 50f / 255);
    }

    public void ShowEquipment()
    {
        ClearItemSlots();
        _currentTab = ItemType.Equipment;
        ShowItems(ItemType.Equipment);
        _weaponText.color = new Color(1, 1, 1, 50f / 255);
        _equipmentText.color = Color.white;
        _otherText.color = new Color(1, 1, 1, 50f / 255);
    }

    public void ShowOther()
    {
        ClearItemSlots();
        _currentTab = ItemType.Item;
        ShowItems(ItemType.Item);
        _weaponText.color = new Color(1, 1, 1, 50f / 255);
        _equipmentText.color = new Color(1, 1, 1, 50f / 255);
        _otherText.color = Color.white;
    }

    public void ClearItemSlots()
    {
        foreach(var slot in _itemSlots)
        {
            slot.sprite = _emptySlotImage;
        }
    }

    public void EquipItem()
    {
        switch(SelectedItem.ItemType)
        {
            case ItemType.Weapon:
                if(CurrentWeapon != null)
                {
                    _player.Attack -= ((WeaponSO)CurrentWeapon).Attack;
                    _player.AttackSpeed -= ((WeaponSO)CurrentWeapon).AttackSpeed;
                }
                    
                CurrentWeapon = SelectedItem;
                _player.ChangeWeapon(SelectedItem.WeaponType);
                _player.Attack += ((WeaponSO)SelectedItem).Attack;
                _player.AttackSpeed += ((WeaponSO)SelectedItem).AttackSpeed;
                _playerController.ChangeWeapon();
                if(_currentWeaponImage == null) _currentWeaponImage = GameObject.FindGameObjectWithTag("WeaponImage").GetComponent<UnityEngine.UI.Image>();
                _currentWeaponImage.sprite = SelectedItem.ItemSprite;
                _weaponCoreImage.sprite = SelectedItem.ItemSprite;
               
                break;
            case ItemType.Equipment:
                _equipmentCoreImage.sprite = SelectedItem.ItemSprite;
                break;
            case ItemType.Item:
                _currentOther = SelectedItem;
                _player.SetThrowableItem(ResourceManager.Instance.LoadPrefab<Item>($"Items/{SelectedItem.ItemID}", SelectedItem.ItemID.ToString()).ItemSO);
                _currentItemImage.sprite = SelectedItem.ItemSprite;
                _otherCoreImage.sprite = SelectedItem.ItemSprite;              
                break;
        }
        _equipBtnText.text = "장착중";
        _equipBtnText.color = Color.green;
        ShowPlayerStats();
        ShowItemSpecs(SelectedItem);
    }

    private void ShowItemSpecs(ItemSO item)
    {
        {
            SelectedItem = item;
            _itemName.text = SelectedItem.ItemName;
            _itemImage.sprite = SelectedItem.ItemSprite;
            _itemDescription.text = SelectedItem.ItemDescription;
            _itemCount.text = _player.Inventory[SelectedItem.ItemType][SelectedItem.ItemID].ItemCount.ToString();

            switch (SelectedItem.ItemType)
            {
                case ItemType.Weapon:
                    _itemType.text = "무기";
                    if (SelectedItem.ItemID == CurrentWeapon?.ItemID)
                    {
                        _equipBtnText.text = "장착중";
                        _equipBtnText.color = Color.green;
                    }
                    else
                    {
                        _equipBtnText.text = "장착";
                        _equipBtnText.color = Color.white;
                    }
                    break;
                case ItemType.Equipment:
                    _itemType.text = "장비";
                    if (SelectedItem.ItemID == _currentEquipment?.ItemID)
                    {
                        _equipBtnText.text = "장착중";
                        _equipBtnText.color = Color.green;
                    }
                    else
                    {
                        _equipBtnText.text = "장착";
                        _equipBtnText.color = Color.white;
                    }
                    break;
                case ItemType.Item:
                    _itemType.text = "기타";
                    if (SelectedItem.ItemID == _currentOther?.ItemID)
                    {
                        _equipBtnText.text = "장착중";
                        _equipBtnText.color = Color.green;
                    }
                    else
                    {
                        _equipBtnText.text = "장착";
                        _equipBtnText.color = Color.white;
                    }
                    break;
            }

            switch (SelectedItem.Rarity)
            {
                case CardRarity.Normal:
                    _itemRarity.text = "노멀";
                    _itemRarity.color = Color.white;
                    break;
                case CardRarity.Rare:
                    _itemRarity.text = "레어";
                    _itemRarity.color = Color.blue;
                    break;
                case CardRarity.Epic:
                    _itemRarity.text = "에픽";
                    _itemRarity.color = Color.magenta;
                    break;
                case CardRarity.Legend:
                    _itemRarity.text = "레전더리";
                    _itemRarity.color = Color.red;
                    break;
                case CardRarity.EndPoint:
                    _itemRarity.text = "신화";
                    _itemRarity.color = Color.yellow;
                    break;
            }

        }
    }

    private void ShowPlayerStats()
    {
        _healthText.text = $"{_player.CurrentHP} / {_player.MaxHP}";
        _tenacityText.text = $"{_player.CurrentTenacity} / {_player.MaxTenacity}";
        _stressText.text = $"{_player.Stress} / {_player.StressLimit}";
        _jumpText.text = $"{_player.MaxJump} 회 점프 가능";
        _speedText.text = $"{_player.MoveSpeed}";
        _attackText.text = $"{_player.Attack}";
        _attackSpeedText.text = _player.AttackSpeed == 0 ? "제한없음" : $"{_player.AttackSpeed}";
    }

    private void OnDisable()
    {
        _playerController.ApplyPlayerStats();
    }
}
