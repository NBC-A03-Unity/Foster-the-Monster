using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UITutorialCage : MonoBehaviour
{
    public GameObject TutorialCage;
    [SerializeField] private Button NextBtn;
    [SerializeField] private List<GameObject> Tutorial;
    public Canvas[] Canvass;
    int index = 0;
    int tempOrder;
    private void Awake()
    {
        NextBtn.onClick.AddListener(OnNextBtn);
        Canvass = new Canvas[Tutorial.Count];
    }

    public void OnNextBtn()
    {
        AudioManager.Instance.PlaySFX(SFXCategory.Button, SFXClips.Open);

        if (Canvass[index] != null)
        {
            Canvass[index].sortingOrder = tempOrder;
        }
        Tutorial[index].SetActive(false);
        index++;


        if (index == Tutorial.Count)
        {
            DataManager.Instance.MainSceneTutorial = true;
            Destroy(TutorialCage);
            UIManager.Instance.CloseUI<UITutorialCage>();
            UIManager.Instance.OpenUI<UIMainRoom>();
            
            return;
        }


        Tutorial[index].SetActive(true);

        if (Canvass[index] != null)
        {
            tempOrder = Canvass[index].sortingOrder;
            Canvass[index].sortingOrder = UIManager.Instance.CurrentSortingOrder + 1;
            Canvass[index].enabled = false;
            Canvass[index].enabled = true;
        }
    }
}
