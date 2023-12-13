using UnityEngine;

public class Smell : MonsterSensor
{
    [SerializeField] private LayerMask targetLayer;
    public override void Init(MonsterSO data)
    {
        SetRange(data.SmellRange);
        _collider.excludeLayers = targetLayer;
        if (data.SmellRange == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
