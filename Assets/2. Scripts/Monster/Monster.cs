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

[RequireComponent(typeof(CharacterController))]
public class Monster : MonoBehaviour
{
    [Header("추적 설정")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _detectDistance = 3f;
    
    [Header("몬스터 설정")]
    [SerializeField] private EMonsterState _state = EMonsterState.Idle;
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _attackDistance = 200f;
    [SerializeField] private float _attackSpeed = 1.2f;
    
    
    private CharacterController _controller;

    private float _attackTimer;
    
    private void Awake()
    {
        _controller.GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 몬스터의 상태에 따라 다른 메서드를 호출한다.

        switch (_state)
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
        }
    }

    /// <summary>
    /// 외부에서 데미지 적용시 호출됨.
    /// </summary>
    public bool TryTakeDamage(float damage)
    {
        if (_state == EMonsterState.Hit || _state == EMonsterState.Death) return false;


        _health -= damage;
        
        if (_health > 0)
        {
            _state = EMonsterState.Hit;
            
        }
        else
        {
            _state = EMonsterState.Death;
        }
        
        Debug.Log($"상태 전환  to  {_state} ");
        return true;
    }
    
    private void Idle()
    {
        

        if (Vector3.Distance(transform.position, _target.position) <= _detectDistance)
        {
            _state = EMonsterState.Trace;
            Debug.Log($"상태 전환  to  {_state} ");
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
            _state = EMonsterState.Attack;
            Debug.Log($"상태 전환  to  {_state} ");
            return;
        }
        
        
        Vector3 direction = (_target.position - transform.position).normalized;
        _controller.Move(direction * _moveSpeed * Time.deltaTime);
    }
    private void Comeback()
    {
        
    }
    
    private void Attack()
    {
        Debug.Log("플레이어 공격!");

        float distance = Vector3.Distance(transform.position, _target.position);
        if (distance > _attackDistance)
        {
            _state = EMonsterState.Trace;
            Debug.Log($"상태 전환  to  {_state} ");
            return;
        }
        
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackSpeed)
        {
            // TODO : 플레이어 공격하기
            
            _attackTimer = 0f;
        }
    }
    
    private IEnumerator Hit_Coroutine()
    {
        // TODO : Hit 애니메이션 실행
        
        yield return new WaitForSeconds(0.5f);
        _state = EMonsterState.Idle;
    }
    
    private IEnumerator Death_Coroutine()
    {
        // TODO : Death 애니메이션 실행
        
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    
    

   
    
  
}
