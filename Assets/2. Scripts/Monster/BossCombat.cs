using UnityEngine;

public class BossCombat : MonoBehaviour
{
    [Header("근접 공격 설정")]
    [SerializeField] private float _meleeAttackDistance = 3f;
    [SerializeField] private float _meleeAttackSpeed = 1.5f;
    [SerializeField] private float _meleeAttackDamage = 10f;

    [Header("점프 공격 설정")]
    [SerializeField] private float _jumpAttackMinDistance = 5f;
    [SerializeField] private float _jumpAttackMaxDistance = 15f;
    [SerializeField] private float _jumpAttackCooldown = 5f;
    [SerializeField] private float _jumpAttackDamage = 20f;
    [SerializeField] private float _jumpAttackRadius = 5f;
    [SerializeField] private float _jumpHeight = 10f;
    [SerializeField] private float _jumpDuration = 1.5f;

    // 프로퍼티
    public float MeleeAttackDistance => _meleeAttackDistance;
    public float MeleeAttackSpeed => _meleeAttackSpeed;
    public float MeleeAttackDamage => _meleeAttackDamage;

    public float JumpAttackMinDistance => _jumpAttackMinDistance;
    public float JumpAttackMaxDistance => _jumpAttackMaxDistance;
    public float JumpAttackCooldown => _jumpAttackCooldown;
    public float JumpAttackDamage => _jumpAttackDamage;
    public float JumpAttackRadius => _jumpAttackRadius;
    public float JumpHeight => _jumpHeight;
    public float JumpDuration => _jumpDuration;

    // 참조
    private TraceController _traceController;
    private IDamagable _playerDamagable;

    // 쿨타임 타이머
    private float _jumpAttackCooldownTimer;

    private void Start()
    {
        _traceController = GetComponent<TraceController>();
        _playerDamagable = _traceController?.Target?.GetComponent<IDamagable>();

        // 초기 쿨타임을 0으로 설정하여 바로 사용 가능하게 함
        _jumpAttackCooldownTimer = 0f;
    }

    private void Update()
    {
        if (_jumpAttackCooldownTimer > 0f)
        {
            _jumpAttackCooldownTimer -= Time.deltaTime;
        }
    }

    public void MeleeAttack()
    {
        _playerDamagable?.TryTakeDamage(new AttackInfo(_meleeAttackDamage));
    }

    public void JumpAttack(Vector3 landingPosition)
    {
        // 점프 공격 실행 시 쿨타임 시작
        _jumpAttackCooldownTimer = _jumpAttackCooldown;

        // 범위 내 적에게 데미지
        Collider[] hits = Physics.OverlapSphere(landingPosition, _jumpAttackRadius);
        foreach (Collider hit in hits)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if (damagable != null)
            {
                Vector3 attackDirection = (hit.transform.position - landingPosition).normalized;
                damagable.TryTakeDamage(new AttackInfo(_jumpAttackDamage, attackDirection));
            }
        }
    }

    public bool CanUseJumpAttack(float distanceToTarget)
    {
        // 거리 조건과 쿨타임 조건을 모두 만족해야 함
        bool distanceCondition = distanceToTarget >= _jumpAttackMinDistance &&
                                 distanceToTarget <= _jumpAttackMaxDistance;
        bool cooldownReady = _jumpAttackCooldownTimer <= 0f;

        return distanceCondition && cooldownReady;
    }

    private void OnDrawGizmosSelected()
    {
        // 근접 공격 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _meleeAttackDistance);

        // 점프 공격 최소/최대 거리 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _jumpAttackMinDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _jumpAttackMaxDistance);
    }
}
