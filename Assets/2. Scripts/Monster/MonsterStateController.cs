using System;
using System.Collections;
using UnityEngine;



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
public class MonsterStateController : MonoBehaviour
{
    [Header("타겟")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _detectDistance = 5f;
    
    [Header("몬스터 State")]
    [SerializeField] private EMonsterState _state = EMonsterState.Idle;
    
    
    [SerializeField] private float _health = 100f;
  
    [Header("공격 설정")]
    [SerializeField] private float _attackDistance = 1f;
    [SerializeField] private float _attackSpeed = 1.2f;
    [SerializeField] private float _attackDamage = 5f;
    
    [Header("넉백")]
    [SerializeField] private float _knockBackForce = 4f;
    [SerializeField] private float _knockbackDuration = 0.15f;
    
    // 참조
    private MonsterMove _move;
    private PlayerStats _playerStats;
    
    // 상수
    private const float DistanceEpsilon = 0.1f;
    
    // 타이머
    private float _attackTimer;
    
    private Vector3 _originalPosition;
    private Vector3 _knockBackDir;
    private float _knockBackTimer;
    
    public EMonsterState State { get => _state; set => _state = value; }

    private void Awake()
    {
       Init();
    }

    private void Update()
    {
        if (_target == null) return;
        
        // 몬스터의 상태에 따라 다른 메서드를 호출한다.
        switch (State)
        {
            case EMonsterState.Idle:
                Idle();
                break;
            case EMonsterState.Trace:
                Trace();
                break;
            case EMonsterState.Comeback:
                Comeback();
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

    

    /// <summary>
    /// 외부에서 데미지 적용시 호출됨.
    /// </summary>
    public bool TryTakeDamage(AttackInfo info)
    {
        if (State == EMonsterState.Death) return false;
        if (info.Damage <= 0f) return false;

        _health -= info.Damage;
        
        if (_health > 0)
        {
            ChangeState(EMonsterState.Hit);
            
            _knockBackDir = (-info.HitDirection).normalized;
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
        float distance = Vector3.Distance(transform.position, _target.position);
        if (distance <= _detectDistance)
        {
            ChangeState(EMonsterState.Trace);
            return;
        }
        
        // 가만히 있는다.
        // TODO : Idle anim
    }

    private void Trace()
    {
        // TODO : Run anim
        
        float distance = Vector3.Distance(transform.position, _target.position);
        if (distance <= _attackDistance)
        {
            ChangeState(EMonsterState.Attack);
            return;
        }
        else if (distance > _detectDistance)
        {
            ChangeState(EMonsterState.Comeback);
            return;
        }
        
        _move.MoveToTarget(_target.position);
    }
    private void Comeback()
    {
        float distance = Vector3.Distance(transform.position, _originalPosition);
        if (distance <= DistanceEpsilon)
        {
            ChangeState(EMonsterState.Idle);
            return;
        }
        
      
        _move.MoveToTarget(_originalPosition);
    }
    
    private void Attack()
    {
        float distance = Vector3.Distance(transform.position, _target.position);
        if (distance > _attackDistance)
        {
            ChangeState(EMonsterState.Trace);
            return;
        }
        
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackSpeed)
        {
            _playerStats?.TryTakeDamage(_attackDamage);
            _attackTimer = 0f;
        }
        
    }
    private void Hit()
    {
       _move.Knockback(_knockBackDir, _knockBackForce);
       
       _knockBackTimer += Time.deltaTime;
       if (_knockBackTimer > _knockbackDuration)
       {
           State = EMonsterState.Trace;
       }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    private void Init()
    {
        _move = GetComponent<MonsterMove>();
        _originalPosition = transform.position;
    }

    private void ChangeState(EMonsterState nextState)
    {
        if (State == nextState) return;
        
        State = nextState;
        Debug.Log($"상태 전환  to  {State} ");
        
        if (State == EMonsterState.Attack)
        {
            _attackTimer = 0f;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectDistance);
    }
    
    
  
}

public class MonsterHealth
{
    
}

public class MonsterCombat
{
    
}


