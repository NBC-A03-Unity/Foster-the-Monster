using System.Collections;
using UnityEngine;

public class Stimulus : MonoBehaviour
{
    public StimulusData _data;
    public Collider2D _collider;
    public PlayerController _controller;

    private void Start()
    {
        Init(_data);
        if (_data.type == Enums.StimulusType.Sound && _data.objectType == Enums.StimulusObjectType.Player) 
        {
            StartCoroutine(PlayerSound());
        }
    }
    public void Init(StimulusData data)
    {
        _data = data;
        _data.transform = transform;
        FixTagAndLayer();
        if (data.hasDestroyTime)
        {
            SetTimer(data.stimulusDuration);
        }
        _collider.enabled = true;
        _collider.transform.localScale = new Vector2(data.range, data.range);
       
    }
    public void SetTimer(float time)
    {
        Destroy(gameObject, time);
    }
    private void FixTagAndLayer()
    {
        gameObject.tag = _data.objectType.ToString();
        gameObject.layer = LayerMask.NameToLayer(_data.type.ToString());
    }

    public IEnumerator PlayerSound()
    {
        _controller = GetComponentInParent<PlayerController>();
        Rigidbody2D rigid = _controller.Rigidbody;
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            float soundRange = Mathf.Abs(rigid.velocity.x) + Mathf.Abs(rigid.velocity.y);
            _collider.transform.localScale = new Vector3(soundRange, soundRange, 1);
            yield return wait;
        }
    }
}
