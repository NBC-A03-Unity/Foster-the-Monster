using UnityEngine;

public class Hearing : MonsterSensor
{
    [SerializeField] private LayerMask targetLayer;

    public override void Init(MonsterSO data)
    {
        SetRange(data.HearingRange);
        _collider.excludeLayers = targetLayer;
        if (data.HearingRange == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
