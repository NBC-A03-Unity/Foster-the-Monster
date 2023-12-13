using UnityEngine;

public class AttackCircle : MonoBehaviour
{
    private Monster _monster;
    [SerializeField] private int _damage;
    public bool isRight;
    private void Awake()
    {
        _monster = GetComponentInParent<Monster>();
        _damage = _monster.monsterSO.attack;

        gameObject.layer = LayerMask.NameToLayer("MonsterAttack");
    }

    private void OnEnable()
    {
        isRight = (_monster.transform.localScale.x >0) ? false : true ;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public bool GetDirection()
    {
        return isRight ? true: false;
    }
    public int GetDamage()
    {
        return _damage;
    }

    public void TurnOffAttackCircle()
    {
        gameObject.SetActive(false);
    }
}
