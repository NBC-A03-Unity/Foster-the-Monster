using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class StimulusViewer : MonoBehaviour
{
    [SerializeField] private Monster monster;
    [SerializeField] private SpriteRenderer objectTypeIconRenderer;
    [SerializeField] private SpriteRenderer SensorTypeIconRenderer;

    [SerializeField] private List<Sprite> objectTypeSprites;
    [SerializeField] private List<Sprite> foodTypeSprites;

    [SerializeField] private List<Sprite> SensorTypeSprites;
    
    [SerializeField] private Sprite rageModeSprite;
    [SerializeField] private Sprite happyModeSprite;

    private void Awake()
    {
        monster = GetComponentInParent<Monster>();
    }
    private void Start()
    {
        monster.OnCurStimulusChange += SetCurStimulusView;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        monster.OnCurStimulusChange -= SetCurStimulusView;
    }
    private void SetCurStimulusView(Stimulus curStimulus, MonsterMode mode)
    {
        if(mode == MonsterMode.Rage)
        {
            gameObject.SetActive(true);
            objectTypeIconRenderer.sprite = objectTypeSprites[2];
            SensorTypeIconRenderer.sprite = rageModeSprite;
            return;
        }
        if(mode == MonsterMode.Happy)
        {
            gameObject.SetActive(true);
            objectTypeIconRenderer.sprite = foodTypeSprites[(int)monster.monsterSO.preferFoodType];
            SensorTypeIconRenderer.sprite = happyModeSprite;
            return;
        }

        if (curStimulus == null)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        objectTypeIconRenderer.sprite = curStimulus._data.icon;
        SensorTypeIconRenderer.sprite = SensorTypeSprites[(int)curStimulus._data.type];
    }
}
