using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIPlanet : MonoBehaviour
{
    [SerializeField] private Button earth;
    [SerializeField] private Button frozenPlanet;
    [SerializeField] private Button lavaPlanet;
    [SerializeField] private Button normalPlanet;

    private ColorChanger colorChanger;

    private void Start()
    {
        colorChanger = GetComponent<ColorChanger>() ?? gameObject.AddComponent<ColorChanger>();

        InitializeButtonListeners();
        InitializeButtonHoverEffects();
    }

    private void InitializeButtonListeners()
    {
        earth.onClick.AddListener(OnEarth);
        frozenPlanet.onClick.AddListener(OnFrozenPlanet);
        lavaPlanet.onClick.AddListener(OnLavaPlanet);
        normalPlanet.onClick.AddListener(OnNormalPlanet);
    }

    private void InitializeButtonHoverEffects()
    {
        foreach (Button button in new Button[] { frozenPlanet, lavaPlanet, normalPlanet, earth })
        {
            colorChanger.AddHoverEffect(button.GetComponent<Image>());
        }
    }

    private void OnEarth()
    {
        DataManager.Instance.todayVisitPlanet = false;
        UIManager.Instance.CloseUI<UIPlanet>();
    }

    private void OnFrozenPlanet()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        UIManager.Instance.OpenUI<UIPlanetCryogenia>();
    }

    private void OnLavaPlanet()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        UIManager.Instance.OpenUI<UIPlanetPyroclastia>();
    }

    private void OnNormalPlanet()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);
        UIManager.Instance.OpenUI<UIPlanetChlorophyllis>();
    }
}
