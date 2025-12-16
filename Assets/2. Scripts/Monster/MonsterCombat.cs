using UnityEngine;


public class MonsterCombat : MonoBehaviour
{
    
    [Header("공격 설정")]
    [SerializeField] private float _attackDistance = 1f;
    [SerializeField] private float _attackSpeed = 1.2f;
    [SerializeField] private float _attackDamage = 5f;

    public float AttackDistance => _attackDistance;

    public float AttackSpeed => _attackSpeed;

    public float AttackDamage => _attackDamage;

   
    //참조
    private TraceController _traceController;
    private IStat _playerStats;

    private void Start()
    {
        _traceController = GetComponent<TraceController>();
        _playerStats =  _traceController?.Target?.GetComponent<IStat>();
    }
    
    public void Attack()
    {
        _playerStats?.TryTakeDamage(new AttackInfo(_attackDamage));
    }
}