using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class FosmonButton : MonoBehaviour
{
    [SerializeField] private Image fosmonImage;
    [SerializeField] private TMP_Text fosmonNameText;

    private MonsterSO monsterData;
    private UIFosmonDictionary dictionaryUI;

    public void Initialize(MonsterSO data, UIFosmonDictionary ui)
    {
        monsterData = data;
        dictionaryUI = ui;

        fosmonImage.sprite = data.mosterSprite;
        SetMonsterNameText(data);

        var button = GetComponent<Button>();
        button.onClick.RemoveListener(OnButtonClick);
        button.onClick.AddListener(OnButtonClick);
    }

    private void SetMonsterNameText(MonsterSO monster)
    {
        string currentLocale = GlobalSettings.CurrentLocale;
        fosmonNameText.text = currentLocale == "en-US" ? monster.monsterName : monster.monsterKoreanName;
    }


    private void OnButtonClick()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        dictionaryUI.ShowMonsterInfo(monsterData);
    }

    public void ResetState()
    {
        fosmonImage.sprite = null;
        fosmonNameText.text = "";
    }
}
