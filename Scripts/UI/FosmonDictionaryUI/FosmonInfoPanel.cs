using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Enums;

public class FosmonInfoPanel : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text fosmonNumberText;
    [Header("Name")]
    [SerializeField] private TMP_Text fosmonNameText;

    [Header("Description")]
    [SerializeField] private TMP_Text fosmonDescriptionText;

    [Header("Prefer")]
    [SerializeField] private TMP_Text fosmonTempratureText;
    [SerializeField] private TMP_Text fosmonCleanlinessText;
    [SerializeField] private TMP_Text fosmonBrighnessText;
    [SerializeField] private TMP_Text fosmonLikeFoodTypeText;
    [SerializeField] private TMP_Text fosmonHateFoodTypeText;

    [Header("Behaviour")]
    [SerializeField] private TMP_Text fosmonHostileText;
    [SerializeField] private TMP_Text fosmonCuriousText;
    [SerializeField] private TMP_Text fosmonFoodLovingText;
    [SerializeField] private TMP_Text fosmonFearfulValueText;

    [Header("Image")]
    [SerializeField] private Image monsterImage;

    [Header("Button")]
    [SerializeField] private Button returnBtn;

    [Header("Localizaiotn")]
    [SerializeField] private FosmonInfoSO fosmonInfoSO;

    private string _locale;
    private MonsterSO currentMonsterData;
    private int key;
    private string locale
    {
        get => _locale;
        set
        {
            if(_locale != value){
                _locale = value;
                UpdateLocalizedText();
            }
        }
    }

    private void Start()
    {
        InitializeButtonListeners();
        locale = GlobalSettings.CurrentLocale;
        GlobalSettings.OnLocaleChanged += HandleLocaleChanged;

        if (currentMonsterData != null)
        {
            SetMonsterInfo(currentMonsterData);
        }
    }


    private void OnDestroy()
    {
        GlobalSettings.OnLocaleChanged -= HandleLocaleChanged;
    }

    private void HandleLocaleChanged(string newLocale)
    {
        locale = newLocale;
        UpdateLocalizedText();
        SetMonsterInfo(currentMonsterData);
    }

    private void InitializeButtonListeners()
    {
        returnBtn.onClick.AddListener(onReturnBtn);
        locale = GlobalSettings.CurrentLocale;
    }

    public void SetMonsterInfo(MonsterSO monsterData)
    {
        if (monsterData == null)
        {
            return;
        }
        fosmonNumberText.text = $"{monsterData.monsterID}";
        fosmonNameText.text = GetLocalizedMonsterName(monsterData);
        fosmonDescriptionText.text = GetLocalizedMonsterDescription(monsterData);


        string temperatureString = ConvertTemperatureToString(monsterData.preferTemperature);
        string cleanlinessString = ConvertCleanlinessToString(monsterData.preferCleanliness);
        string brightnessStirng = ConvertBrightnessToString(monsterData.preferBrightness);
        string likeFoodTypeString = ConvertLikeFoodTypeString(monsterData.preferFoodType);
        string hateFoodTypeString = ConvertHateFoodTypeString(monsterData.hateFoodType);

        string hostileString = ConvertHostileToString(monsterData.Hosile);
        string curiousString = ConvertCuriousToString(monsterData.Curious);
        string foodLovingString = ConvertFoodLovingToString(monsterData.FoodLoving);
        string fearfulstring = ConvertFearfulToString(monsterData.Fearful);

        fosmonTempratureText.text = temperatureString;
        fosmonCleanlinessText.text = cleanlinessString;
        fosmonBrighnessText.text = brightnessStirng;
        fosmonLikeFoodTypeText.text = likeFoodTypeString;
        fosmonHateFoodTypeText.text = hateFoodTypeString;
        fosmonHostileText.text = hostileString;
        fosmonCuriousText.text = curiousString;
        fosmonFoodLovingText.text = foodLovingString;
        fosmonFearfulValueText.text = fearfulstring;

        monsterImage.sprite = monsterData.mosterSprite;
    }

    private void UpdateLocalizedText()
    {
        if(currentMonsterData != null)
        {
            SetMonsterInfo(currentMonsterData);
        }
    }

    private string ConvertTemperatureToString(int preferTemperature)
    {
        key = 5000 + preferTemperature - 1;
        return GetLocalizedString(key);
    }

    private string ConvertCleanlinessToString(int preferCleanliness)
    {
        key = 5004 + preferCleanliness;
        return GetLocalizedString(key);
    }

    private string ConvertBrightnessToString(int preferBrightness)
    {
        key = 5009 + preferBrightness;
        return GetLocalizedString(key);
    }

    private string ConvertHostileToString(int hosile)
    {
        key = 5015 + hosile;
        return GetLocalizedString(key);
    }

    private string ConvertCuriousToString(int curious)
    {
        key = 5018 + curious;
        return GetLocalizedString(key);
    }

    private string ConvertFoodLovingToString(int foodLoving)
    {
        key = 5021 + foodLoving;
        return GetLocalizedString(key);
    }

    private string ConvertFearfulToString(int fearful)
    {
        key = 5024 + fearful;
        return GetLocalizedString(key);
    }

    private string ConvertLikeFoodTypeString(Enums.FillersFoodType foodType)
    {
        key = ConvertFoodTypeToKey(foodType);
        return GetLocalizedString(key);
    }

    private string ConvertHateFoodTypeString(Enums.FillersFoodType foodType)
    {
        key = ConvertFoodTypeToKey(foodType);
        return GetLocalizedString(key);
    }

    private int ConvertFoodTypeToKey(Enums.FillersFoodType foodType)
    {
        switch (foodType)
        {
            case Enums.FillersFoodType.Meat: return 6000;
            case Enums.FillersFoodType.Grains: return 6001;
            case Enums.FillersFoodType.SeaFood: return 6002;
            case Enums.FillersFoodType.None: return 6003;
            default: return -1;
        }
    }

    private string GetLocalizedString(int key)
    {
        foreach (var item in fosmonInfoSO.localizedStirngs)
        {
            if (item.localeKey == key)
            {
                return locale == "en-US" ? item.English : item.Korean;
            }
        }
        return "Unknown";
    }

    private string GetLocalizedMonsterName(MonsterSO monsterData)
    {
        return locale == "en-US" ? monsterData.monsterName : monsterData.monsterKoreanName;
    }

    private string GetLocalizedMonsterDescription(MonsterSO monsterData)
    {
        return locale == "en-US" ? monsterData.description : monsterData.koreanDescription;
    }

    private void onReturnBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Close);
        UIManager.Instance.CloseUI<FosmonInfoPanel>();
    }
}