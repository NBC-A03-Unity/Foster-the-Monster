using System;
using UnityEngine;
using UnityEngine.UI;

public class CageBtn : MonoBehaviour
{
    [SerializeField] private GameObject waringIcon;
    [SerializeField] private RawImage image;
    [SerializeField] private Button button;

    public void InitBtn(RenderTexture renderTexture, Action action)
    {
        image.texture = renderTexture;
        button.onClick.AddListener(() => action?.Invoke());
    }

    public void OnWaringIcon()
    {
        waringIcon.SetActive(true);
    }

    public void OffWaringIcon() 
    {
        waringIcon.SetActive(false);
    }

    
}
