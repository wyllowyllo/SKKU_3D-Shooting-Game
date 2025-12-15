using UnityEngine;

public class MonsterStats : MonoBehaviour, IStat
{
    [SerializeField] private ComsumableStat _health;

    // 참조
    private MonsterStateController _stateController;

    public bool IsLive => CurHealth > 0f;

    public float CurHealth => _health.Value;
    public float MaxHealth => _health.MaxValue;

    private void Awake()
    {
        _stateController = GetComponent<MonsterStateController>();
        
        _health.Initialize();
    }

    public void TryTakeDamage(AttackInfo attackInfo)
    {
        _health.Decrease(attackInfo.Damage);
        
        _stateController?.OnDamaged(attackInfo);
        
    }
}