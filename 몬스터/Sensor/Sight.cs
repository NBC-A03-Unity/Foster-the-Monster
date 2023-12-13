using UnityEngine;

public class Sight :MonsterSensor
{
    [SerializeField] private LayerMask dayTimeTargetLayer;
    [SerializeField] private LayerMask nightTimeTargetLayer;

    private bool isNightTime;

    public override void Init(MonsterSO data)
    {
        SetRange(data.SightRange);
        _collider.excludeLayers = isNightTime ? nightTimeTargetLayer : dayTimeTargetLayer;
        if (data.SightRange == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
