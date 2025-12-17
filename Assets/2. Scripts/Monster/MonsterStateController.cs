using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
// 목표 : 처음에는 가만히 있지만 플레이어가 다가가면 쫓아오는 좀비 몬스터
// -> 쫓아 오다가 너무 멀어지면 제자리로 돌아간다.
   
  
// 몬스터 인공지능 (AI) : 사람처럼 행동하는 똑똑한 시스템 / 알고리즘
//  - 규칙 기반 인공지능 : 정해진 규칙에 따라 조건문/반복문 등을 이용해서 코딩하는 것
//  -> FSM(유한 상태 머신), BT(행동 트리)
   
// - 학습 기반 인공지능 : 머신러닝(딥러닝, 강화학습..)

// 다음 사항 항상 준수
// 1. 함수는 한 가지 일만 잘해야 한다.
// 2. 상태별 행동을 함수로 만든다.

[RequireComponent(typeof(CharacterController), typeof(MonsterMove))]
[RequireComponent(typeof(TraceController), typeof(MonsterCombat), typeof(MonsterStats))]
public class MonsterStateController : MonoBehaviour
{
    [Header("몬스터 State")]
    [SerializeField] private EMonsterState _state = EMonsterState.Patrol;

    [Header("순찰 설정")]
    [SerializeField] private float _patrolRadius = 10f;
    [SerializeField] private float _patrolWaitTime = 2f;
    
    

    // 참조
    private TraceController _traceController;
    private MonsterMove _moveController;
    private MonsterCombat _combatController;
    private MonsterStats _stats;
    private Animator _animator;


    // 상수
    private const float DistanceEpsilon = 0.1f;

    // 타이머
    private float _attackTimer;
    private float _patrolWaitTimer;
    private float _jumpTimer;
    
    private Vector3 _originalPosition;
    private Vector3 _knockBackDir;
    private float _knockBackTimer;
    private Vector3 _patrolTarget;
    

    
    
    public EMonsterState State { get => _state; set => _state = value; }

    private void Awake()
    {
       Init();
    }

    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing
            && GameManager.Instance.State != EGameState.Auto) return;
        if (_traceController.Target == null) return;
        
        // 몬스터의 상태에 따라 다른 메서드를 호출한다.
        switch (State)
        {
            case EMonsterState.Idle:
                Idle();
                break;
            case EMonsterState.Patrol:
                Patrol();
                break;
            case EMonsterState.Trace:
                Trace();
                break;
            case EMonsterState.Comeback:
                Comeback();
                break;
            
            case EMonsterState.Jump:
                Jump();
                break;
            
            case EMonsterState.Attack:
                Attack();
                break;

            case EMonsterState.Hit:
                Hit();
                break;

            case EMonsterState.Death:
                Die();
                break;
        }
    }

    
    
    public bool OnDamaged(AttackInfo info)
    {
        if (State == EMonsterState.Death) return false;
        if (info.Damage <= 0f) return false;

        
        if (_stats.IsLive)
        {
            ChangeState(EMonsterState.Hit);

            _knockBackDir = info.HitDirection;
            _knockBackTimer = 0f;
        }
        else
        {
            ChangeState(EMonsterState.Death);
        }

        return true;
    }
    
    
    private void Idle()
    {
        if (_traceController.Detected)
        {
            ChangeState(EMonsterState.Trace);
            _animator?.SetTrigger("IdleToTrace");
            return;
        }
        
        
    }

    private void Patrol()
    {
        if (_traceController.Detected)
        {
            ChangeState(EMonsterState.Trace);
            _animator?.SetTrigger("IdleToTrace");
            return;
        }
        
        float distanceToTarget = Vector3.Distance(transform.position, _patrolTarget);
        if (distanceToTarget <= DistanceEpsilon)
        {
            _patrolWaitTimer += Time.deltaTime;

            if (_patrolWaitTimer >= _patrolWaitTime)
            {
                _patrolTarget = GetRandomPatrolPosition();
                _patrolWaitTimer = 0f;
            }
        }
        else
        {
            _moveController.MoveToTarget(_patrolTarget);
           
        }
    }

    private void Trace()
    {
        // TODO : Run anim
        
        float distance = _traceController.DistanceFromTarget;
        if (distance <= _combatController.AttackDistance)
        {
            ChangeState(EMonsterState.Attack);
            _animator?.SetTrigger("TraceToAttackIdle");
            return;
        }
        else if (!_traceController.Detected)
        {
            ChangeState(EMonsterState.Comeback);
            return;
        }
        
        

        if (_moveController.IsOnJumpTrigger())
        {
            _jumpTimer = 0f;
            ChangeState(EMonsterState.Jump);
                
            return;
        }
        
        _moveController.MoveToTarget(_traceController.TargetPosition);
    }
    private void Comeback()
    {
        float distance = Vector3.Distance(transform.position, _originalPosition);
        if (distance <= DistanceEpsilon)
        {
            ChangeState(EMonsterState.Patrol);
            return;
        }


        _moveController.MoveToTarget(_originalPosition);
    }

    private void Jump()
    {
        _jumpTimer += Time.deltaTime / _moveController.JumpDuration;
        
        if (_jumpTimer >= 1f)
        {
            _moveController.JumpEnd();
           
            ChangeState(EMonsterState.Trace);
            return;
        }
        
       _moveController.Jump(_jumpTimer);
    }
    
    private void Attack()
    {
        float distance = _traceController.DistanceFromTarget;
        if (distance > _combatController.AttackDistance)
        {
            ChangeState(EMonsterState.Trace);
            _animator?.SetTrigger("Attack");
            return;
        }
        
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _combatController.AttackSpeed)
        {
            _combatController.Attack();
            _attackTimer = 0f;
        }
        
    }
    private void Hit()
    {
       _moveController.Knockback(_knockBackDir);
       
       _knockBackTimer += Time.deltaTime;
       if (_knockBackTimer > _moveController.KnockbackDuration)
       {
           State = EMonsterState.Trace;
       }
       
       _animator?.SetTrigger("Hit");
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    private void Init()
    {
        _traceController = GetComponent<TraceController>();
        _moveController = GetComponent<MonsterMove>();
        _combatController = GetComponent<MonsterCombat>();
        _stats = GetComponent<MonsterStats>();
        _animator = GetComponentInChildren<Animator>();
        
        _originalPosition = transform.position;
        _patrolTarget = GetRandomPatrolPosition();
    }

    private Vector3 GetRandomPatrolPosition()
    {
        // 원점 기준으로 반경 내 랜덤 위치 생성
        Vector2 randomCircle = Random.insideUnitCircle * _patrolRadius;
        Vector3 randomPosition = _originalPosition + new Vector3(randomCircle.x, 0f, randomCircle.y);

        return randomPosition;
    }

    private void ChangeState(EMonsterState nextState)
    {
        if (State == nextState) return;

        State = nextState;
        Debug.Log($"상태 전환  to  {State} ");

        if (State == EMonsterState.Attack)
        {
            _attackTimer = _combatController.AttackSpeed;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, _traceController.DetectDistance);
    }
    
    
  
}