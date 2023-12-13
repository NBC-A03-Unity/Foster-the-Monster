using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image _image;
    private int _imageIndex;

    private void OnEnable()
    {
        
        StartCoroutine(AnimateImage(new WaitForSeconds(0.2f)))
;    }

    private IEnumerator AnimateImage(WaitForSeconds interval)
    {
        while(gameObject.activeSelf)
        {
            _imageIndex = (_imageIndex < _sprites.Length)? _imageIndex :0;
            _image.sprite = _sprites[_imageIndex];
            _imageIndex++;

            yield return interval;
        }
    }

}
