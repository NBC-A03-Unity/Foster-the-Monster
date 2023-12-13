using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIPlanetDictionary : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button chlorophyllisBtn;
    [SerializeField] private Button cryogeniaBtn;
    [SerializeField] private Button pyroclastiaBtn;
    [SerializeField] private Button returnBtn;

    private void Start()
    {
        InitializeButtonListeners();
    }

    private void InitializeButtonListeners()
    {
        chlorophyllisBtn.onClick.AddListener(OnChlorophyllis);
        cryogeniaBtn.onClick.AddListener(OnCryogenia);
        pyroclastiaBtn.onClick.AddListener(OnPyroclastia);
        returnBtn.onClick.AddListener(OnReturn);
    }

    private void OnChlorophyllis()
    {
        OpenFosmonDictionaryWithPlanet(MonsterPlanet.Planet1);
    }

    private void OnCryogenia()
    {
        OpenFosmonDictionaryWithPlanet(MonsterPlanet.Planet5);
    }

    private void OnPyroclastia()
    {
        OpenFosmonDictionaryWithPlanet(MonsterPlanet.Planet2);
    }

    private void OnReturn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Close);
        UIManager.Instance.CloseUI<UIPlanetDictionary>();
    }

    private void OpenFosmonDictionaryWithPlanet(MonsterPlanet planet)
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        var fosmonDictionary = UIManager.Instance.OpenUI<UIFosmonDictionary>();
        fosmonDictionary.LoadMonsterForPlanet(planet);
    }
}
