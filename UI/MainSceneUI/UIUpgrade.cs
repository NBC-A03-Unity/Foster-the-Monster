using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgrade : MonoBehaviour
{
    #region Componentes
    [SerializeField] private TextMeshProUGUI _currentEnergy;
    [SerializeField] private TextMeshProUGUI _newEnergy;
    [SerializeField] private TextMeshProUGUI _currentJump;
    [SerializeField] private TextMeshProUGUI _newJump;
    [SerializeField] private TextMeshProUGUI _currentSpeed;
    [SerializeField] private TextMeshProUGUI _newSpeed;
    [SerializeField] private TextMeshProUGUI _currentAttack;
    [SerializeField] private TextMeshProUGUI _newAttack;
    [SerializeField] private TextMeshProUGUI _currentAttackSpeed;
    [SerializeField] private TextMeshProUGUI _newAttackSpeed;

    [SerializeField] private TextMeshProUGUI _energyPriceText;
    [SerializeField] private TextMeshProUGUI _jumpPriceText;
    [SerializeField] private TextMeshProUGUI _speedPriceText;
    [SerializeField] private TextMeshProUGUI _attackPriceText;
    [SerializeField] private TextMeshProUGUI _attackSpeedPriceText;

    [SerializeField] private Button _upgradeEnergyButton;
    [SerializeField] private Button _upgradeJumpButton;
    [SerializeField] private Button _upgradeSpeedButton;
    [SerializeField] private Button _upgradeAttackButton;
    [SerializeField] private Button _upgradeAttackSpeedButton;

    [SerializeField] private Button _closeUIButton;
    #endregion

    #region Stat variables
    private int _newEnergyStat;
    private int _newJumpStat;
    private float _newSpeedStat;
    private float _newAttackStat;
    private float _newAttackSpeedStat;
    #endregion

    private Player _player;

    int noMoneyPopupKey = GlobalSettings.CurrentLocale == "en-US" ? 2005 : 1005;

    private void Start()
    {
        _closeUIButton.onClick.AddListener(() => UIManager.Instance.CloseUI<UIUpgrade>());

        _upgradeEnergyButton.onClick.AddListener(() =>
        {
            if(_player.Gold >= DataManager.Instance.energyPrice)
            {
                _player.ChangeGold(-DataManager.Instance.energyPrice);
                DataManager.Instance.energyPrice *= 2;
                _player.SetTenacity(_newEnergyStat);
                DataManager.Instance.energyStat = _newEnergyStat;
                SetNewStats();
                ShowStats();
            }
            else
            {
                UIManager.Instance.OpenConfirmationPopup(
                noMoneyPopupKey,
                () => {
                    UIManager.Instance.CloseUI<ConfirmationPopup>();
                },
                () => {
                    UIManager.Instance.CloseUI<ConfirmationPopup>();
                });
            }
        });
        _upgradeJumpButton.onClick.AddListener(() =>
        {
            if(_player.Gold >= DataManager.Instance.jumpPrice)
            {
                _player.ChangeGold(-DataManager.Instance.jumpPrice);
                DataManager.Instance.jumpPrice *= 2;
                _player.SetJump(_newJumpStat);
                DataManager.Instance.jumpStat = _newJumpStat;
                SetNewStats();
                ShowStats();
            }
            else
            {
                UIManager.Instance.OpenConfirmationPopup(
                noMoneyPopupKey,
                () => {
                    UIManager.Instance.CloseUI<ConfirmationPopup>();
                },
                () => {
                    UIManager.Instance.CloseUI<ConfirmationPopup>();
                });
            }
        });
        _upgradeSpeedButton.onClick.AddListener(() =>
        {
            if(_player.Gold >= DataManager.Instance.speedPrice)
            {
                _player.ChangeGold(-DataManager.Instance.speedPrice);
                DataManager.Instance.speedPrice *= 2;
                _player.MoveSpeed = _newSpeedStat;
                DataManager.Instance.speedStat = _newSpeedStat;
                SetNewStats();
                ShowStats();
            }
            else
            {
                UIManager.Instance.OpenConfirmationPopup(
                noMoneyPopupKey,
                () => {
                    UIManager.Instance.CloseUI<ConfirmationPopup>();
                },
                () => {
                    UIManager.Instance.CloseUI<ConfirmationPopup>();
                });
            }

        });
        _upgradeAttackButton.onClick.AddListener(() =>
        {
            if(_player.Gold >= DataManager.Instance.attackPrice)
            {
                _player.ChangeGold(-DataManager.Instance.attackPrice);
                DataManager.Instance.attackPrice *= 2;
                _player.Attack = _newAttackStat;
                DataManager.Instance.attackStat = _newAttackStat;
                SetNewStats();
                ShowStats();
            }
            else
            {
                UIManager.Instance.OpenConfirmationPopup(
                noMoneyPopupKey,
                () => {
                    UIManager.Instance.CloseUI<ConfirmationPopup>();
                },
                () => {
                    UIManager.Instance.CloseUI<ConfirmationPopup>();
                });
            }
        });
        _upgradeAttackSpeedButton.onClick.AddListener(() =>
        {
            if(_player.Gold >= DataManager.Instance.attackSpeedPrice)
            {
                _player.ChangeGold(-DataManager.Instance.attackSpeedPrice);
                DataManager.Instance.attackSpeedPrice *= 2;
                _player.AttackSpeed = _newAttackSpeedStat;
                DataManager.Instance.attackSpeedStat = _newAttackSpeedStat;
                SetNewStats();
                ShowStats();
            }
            else
            {
                UIManager.Instance.OpenConfirmationPopup(
                noMoneyPopupKey,
                () => {
                    UIManager.Instance.CloseUI<ConfirmationPopup>();
                },
                () => {
                    UIManager.Instance.CloseUI<ConfirmationPopup>();
                });
            }

        });
    }

    private void OnEnable()
    {
        DataManager.Instance.UpdatePlayerStat();

        _player = DataManager.Instance.Player;
        SetNewStats();
        ShowStats();
    }

    private void ShowStats()
    {
        _currentEnergy.text = _player.MaxTenacity.ToString();
        _currentJump.text = _player.MaxJump.ToString();
        _currentSpeed.text = _player.MoveSpeed.ToString();
        _currentAttack.text = _player.Attack.ToString();
        _currentAttackSpeed.text = _player.AttackSpeed.ToString() + " /s";

        _newEnergy.text = _newEnergyStat.ToString();
        _newJump.text = _newJumpStat.ToString();
        _newSpeed.text = _newSpeedStat.ToString();
        _newAttack.text = _newAttackStat.ToString();
        _newAttackSpeed.text = _newAttackSpeedStat.ToString() + " /s";

        _energyPriceText.text = DataManager.Instance.energyPrice.ToString() + "G";
        _jumpPriceText.text = DataManager.Instance.jumpPrice.ToString() + "G";
        _speedPriceText.text = DataManager.Instance.speedPrice.ToString() + "G";
        _attackPriceText.text = DataManager.Instance.attackPrice.ToString() + "G";
        _attackSpeedPriceText.text = DataManager.Instance.attackSpeedPrice.ToString() + "G";
    }

    private void SetNewStats()
    {
        _newEnergyStat = _player.MaxTenacity + 25;
        _newJumpStat = _player.MaxJump + 1;
        _newSpeedStat = _player.MoveSpeed + 2.5f;
        _newAttackStat = _player.Attack + 2.5f;
        _newAttackSpeedStat = _player.AttackSpeed + 1.5f;
    }
}
