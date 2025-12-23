using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(MonsterMove))]
[RequireComponent(typeof(TraceController), typeof(BossCombat), typeof(MonsterHealth))]
public class BossStateController : MonoBehaviour
{
    [Header("보스 State")]
    [SerializeField] private EBossState _state = EBossState.Idle;

    [Header("점프 공격 설정")]
    [SerializeField] private float _jumpAttackCheckInterval = 5f;

    // 참조
    private TraceController _traceController;
    private MonsterMove _moveController;
    private BossCombat _combatController;
    private MonsterHealth _health;
    private Animator _animator;

    // 상수
    private const float DistanceEpsilon = 0.1f;
    private const float JumpDelay = 0.5f; // 웅크리는 모션 대기 시간

    // 타이머
    private float _meleeAttackTimer;
    private float _jumpAttackCheckTimer;
    private float _jumpTimer;
    private float _knockBackTimer;

    // 점프 공격 관련
    private Vector3 _jumpStartPos;
    private Vector3 _jumpTargetPos;
    private bool _isJumping;
    private float _jumpAnimationDuration; // 실제 애니메이션 길이

    // 넉백 관련
    private Vector3 _knockBackDir;

    // 플래그 변수
    private bool _isDie;
    private bool _hasEnteredCombat; // Idle에서 Trace로 전환되었는지 추적

    public EBossState State { get => _state; set => _state = value; }

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing) return;
        if (_traceController.Target == null) return;
        if (_isDie) return;

        // 보스의 상태에 따라 다른 메서드를 호출
        switch (State)
        {
            case EBossState.Idle:
                Idle();
                break;
            case EBossState.Trace:
                Trace();
                break;
            case EBossState.MeleeAttack:
                MeleeAttack();
                break;
            case EBossState.JumpAttack:
                JumpAttack();
                break;
            case EBossState.Hit:
                Hit();
                break;
            case EBossState.Death:
                Die();
                break;
        }
    }

    public bool OnDamaged(AttackInfo info)
    {
        if (State == EBossState.Death) return false;
        if (info.Damage <= 0f) return false;

        if (_health.IsLive)
        {
            ChangeState(EBossState.Hit);

            _knockBackDir = info.AttackDirection;
            _knockBackTimer = 0f;

            //_animator?.SetBool("Hit", true);
        }
        else
        {
            //_animator?.SetBool("Hit", false);
            ChangeState(EBossState.Death);
        }

        return true;
    }

    private void Idle()
    {
        // Idle 상태에서 플레이어 감지 시 Trace로 전환
        if (_traceController.Detected)
        {
            _hasEnteredCombat = true;
            ChangeState(EBossState.Trace);
            _animator?.SetTrigger("Detect");
            return;
        }
    }

    private void Trace()
    {
        float distance = _traceController.DistanceFromTarget;

        
        _jumpAttackCheckTimer += Time.deltaTime;
        if (_jumpAttackCheckTimer >= _jumpAttackCheckInterval)
        {
            _jumpAttackCheckTimer = 0f;

            if (_combatController.CanUseJumpAttack(distance))
            {
                _jumpStartPos = transform.position;
                _jumpTargetPos = _traceController.TargetPosition;
                _jumpTimer = -JumpDelay; // 음수로 시작하여 딜레이 표현
                _isJumping = false; 
                _jumpAnimationDuration = 0f;

                ChangeState(EBossState.JumpAttack);
                _animator?.SetTrigger("JumpAttack");
                _moveController.Pause();

                // 애니메이션 길이 가져오기
                StartCoroutine(GetJumpAnimationLength_Coroutine());
                return;
            }
        }

       
        if (distance <= _combatController.MeleeAttackDistance)
        {
            ChangeState(EBossState.MeleeAttack);
            _animator?.SetBool("MeleeAttackIdle", true);
            return;
        }

      
        _moveController.MoveToTarget(_traceController.TargetPosition);
        //_animator?.SetBool("Trace", true);
    }

    private void MeleeAttack()
    {
        float distance = _traceController.DistanceFromTarget;

        // 플레이어가 너무 멀어지면 다시 추적
        if (distance > _combatController.MeleeAttackDistance)
        {
            ChangeState(EBossState.Trace);
            _animator?.SetBool("MeleeAttackIdle", false);
            return;
        }

        // 공격 타이머
        _meleeAttackTimer += Time.deltaTime;
        if (_meleeAttackTimer >= _combatController.MeleeAttackSpeed)
        {
            _combatController.MeleeAttack();
            _animator?.SetTrigger("MeleeAttack");
            _meleeAttackTimer = 0f;
        }
    }

    private void JumpAttack()
    {
        // 웅크리는 딜레이 처리 (음수 타이머)
        if (_jumpTimer < 0f)
        {
            _jumpTimer += Time.deltaTime;
            if (_jumpTimer >= 0f)
            {
                // 딜레이 끝, 실제 점프 시작
                _jumpTimer = 0f;
                _isJumping = true;
            }
            return;
        }

        if (!_isJumping) return;

        // 애니메이션 길이 가져오기 전에는 기본값 사용
        float duration = _jumpAnimationDuration > 0f ? _jumpAnimationDuration : _combatController.JumpDuration;
        _jumpTimer += Time.deltaTime / duration;

        if (_jumpTimer >= 1f)
        {
            transform.position = _jumpTargetPos;
            _isJumping = false;

            _combatController.JumpAttack(_jumpTargetPos);

            ChangeState(EBossState.Trace);
            return;
        }

        Vector3 currentPos = Vector3.Lerp(_jumpStartPos, _jumpTargetPos, _jumpTimer);
        float arc = Mathf.Sin(_jumpTimer * Mathf.PI);
        currentPos.y += arc * _combatController.JumpHeight;
        transform.position = currentPos;

        Vector3 direction = (_jumpTargetPos - currentPos).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void Hit()
    {
        _moveController.Knockback(_knockBackDir);

        _knockBackTimer += Time.deltaTime;
        if (_knockBackTimer >= _moveController.KnockbackDuration)
        {
            ChangeState(EBossState.Trace);
        }
    }

    private void Die()
    {
        _isDie = true;
        _moveController.Pause();

        
        AnimReset();

        _animator?.SetTrigger("Death");
        StartCoroutine(Die_Coroutine());
    }

    private IEnumerator Die_Coroutine()
    {
        if (_animator == null) yield break;

        yield return null; // Play 적용 대기 1프레임

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float length = stateInfo.length;

        yield return new WaitForSeconds(length);

        Destroy(gameObject);
    }

    private IEnumerator GetJumpAnimationLength_Coroutine()
    {
        if (_animator == null) yield break;

        yield return null; // 애니메이션 상태 전환 대기 1프레임

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _jumpAnimationDuration = stateInfo.length;

        //Debug.Log($"점프 애니메이션 길이: {_jumpAnimationDuration}초");
    }

    private void Init()
    {
        _traceController = GetComponent<TraceController>();
        _moveController = GetComponent<MonsterMove>();
        _combatController = GetComponent<BossCombat>();
        _health = GetComponent<MonsterHealth>();
        _animator = GetComponentInChildren<Animator>();

        _hasEnteredCombat = false;
    }

    private void ChangeState(EBossState nextState)
    {
        if (State == nextState) return;

        State = nextState;
        Debug.Log($"보스 상태 전환: {State}");

        if (State == EBossState.MeleeAttack)
        {
            _meleeAttackTimer = _combatController.MeleeAttackSpeed;
        }
    }

    private void AnimReset()
    {
        _animator?.SetBool("Hit", false);
        _animator?.SetBool("MeleeAttackIdle", false);
        _animator?.SetBool("Trace", false);
    }

    private void OnDrawGizmos()
    {
        if (_isJumping)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(_jumpStartPos, _jumpTargetPos);
            Gizmos.DrawWireSphere(_jumpTargetPos, _combatController.JumpAttackRadius);
        }
    }
}
