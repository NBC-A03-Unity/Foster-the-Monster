using System;
using UnityEngine;

public abstract class MonsterSensor : MonoBehaviour
{
    [SerializeField] public Collider2D _collider;
    public event Action<Stimulus> OnSense;
    public event Action<Stimulus> UnSense;

    public virtual void Init(MonsterSO data)
    {
    }

    protected virtual void SetRange(int range)
    {
        gameObject.layer = LayerMask.NameToLayer("Sensor");
        Vector3 scale = transform.localScale;
        scale.x = range;
        scale.y = range;
        transform.localScale = scale;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Stimulus temp = collision.gameObject.GetComponent<Stimulus>();
        CallEventOnSense(temp);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        Stimulus temp = collision.gameObject.GetComponent<Stimulus>();
        CallEventUnSense(temp);
    }
    protected virtual void CallEventOnSense(Stimulus stimulus)
    {
        OnSense?.Invoke(stimulus);
    }

    protected virtual void CallEventUnSense(Stimulus stimulus)
    {
        UnSense?.Invoke(stimulus);
    }
}
