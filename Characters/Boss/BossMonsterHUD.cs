using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossMonsterHUD : MonoBehaviour
{
    [SerializeField] private Slider sliderHP;
    [SerializeField] private BossMonster bossMonster;

    private float percent;


    private void Update()
    {
        
        percent = bossMonster.CurHP / bossMonster.MaxHP;
        if (sliderHP.value != percent)
        {
            StartCoroutine(SetHP());
        }
    }

    private IEnumerator SetHP()
    {
        float startValue = sliderHP.value;
        float timeElapsed = 0f;
        float duration = 0.1f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            sliderHP.value = Mathf.Lerp(startValue, percent, timeElapsed / duration);
            yield return null;
        }

        sliderHP.value = percent;
    }
}