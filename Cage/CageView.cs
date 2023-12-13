using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Utility;

public class CageView : MonoBehaviour
{
    private const float DURATION = 0.3f;

    [Header("Monster")]
    [SerializeField] private Transform monsterPosition;
    [SerializeField] private TextMeshProUGUI nameTxt;

    [SerializeField] public Camera mCamera;
    [SerializeField] private SpriteRenderer waringStress;
    [SerializeField] private Button monsterButton;

    private bool isWaring = false;
    public CageBtn cageBtn;    
    public Transform MonsterPosition { get { return monsterPosition; } }
    private ObjectManager og;

    private Sequence sequence;
    private void Awake()
    {
        og = ObjectManager.Instance;

    }

    public void AddMonsterButton(Action action)
    {
        monsterButton.onClick.RemoveAllListeners();
        monsterButton.onClick.AddListener(() => action?.Invoke());
    }
   
 
    public void MoveCameraToCage(CageMainController cc)
    {
        if (!DataManager.Instance.MainSceneTutorial) return;

        if (DataManager.Instance.selectCount > 0)
        {
            CheckPopUp(4010, 3010);
        }
        else
        {
            UIManager.Instance.CloseUI<UIMainRoom>();
            CageManager.Instance.ChangeCurrentCage(cc);
        }
       
    }
    public void OnWaringUI()
    {
        if (!isWaring)
        {
            isWaring = true;
            sequence = DOTween.Sequence()
            .Append(waringStress.DOFade(0.5f, DURATION))
            .Append(waringStress.DOFade(0f, DURATION))
            .SetLoops(-1, LoopType.Yoyo)
            .Play();
        }
    }
    public void OffWaringUI()
    {
        sequence.Kill();
        waringStress.DOFade(0f, DURATION);
        isWaring = false;
    }
}