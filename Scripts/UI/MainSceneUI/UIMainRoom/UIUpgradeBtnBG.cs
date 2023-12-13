using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeBtnBG : MonoBehaviour
{
    private const string mainColor = "98F7C6";
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private TMP_Text upgradeBtnHoverText;
    private void Awake()
    {
        upgradeBtn.onClick.AddListener(() =>
        {
            if (!DataManager.Instance.MainSceneTutorial) return;
            UIManager.Instance.OpenUI<UIUpgrade>();
        });
        UIManager.Instance.InitializeButtonHoverEffect(upgradeBtn, upgradeBtnHoverText, mainColor);
    }

}
