using System;
using UnityEngine;
using UnityEngine.AI;
    
[RequireComponent(typeof(NavMeshAgent))]
public class MonsterMove : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateSpeed = 5f;
    
    [Header("넉백")]
    [SerializeField] private float _knockBackForce = 4f;
    [SerializeField] private float _knockbackDuration = 0.15f;
    
    // 점프 설정
    private Vector3 _jumpStartPos;
    private Vector3 _jumpEndPos;
    private float _jumpDuration;
    
    
    // 참조
    private CharacterController _characterController;
    private NavMeshAgent _agent;

    // 상수
    private const float HitRotateFactor = 0.05f;

    // 프로퍼티
    public float KnockbackDuration => _knockbackDuration;
    public float MoveSpeed => _moveSpeed;

    public float JumpDuration => _jumpDuration;


    private void Awake()
    {
        Init();
    }

    
    public void MoveToTarget(Vector3 targetPosition)
    {
        // TODO : Run anim

        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        //_characterController.Move(direction * MoveSpeed * Time.deltaTime);*/

        _agent.SetDestination(targetPosition);

        RotateToDirection(direction);
    }

    public void Knockback(Vector3 knockbackDir)
    {
        Pause();

        Vector3 movement = knockbackDir * (_knockBackForce * Time.deltaTime);
        _characterController.Move(movement);

        //RotateToDirection(knockbackDir * HitRotateFactor);
    }

    public void Jump(float t)
    {
        // 2. 점프 시간동안 포물선으로 이동한다
        Vector3 curPos = Vector3.Lerp(_jumpStartPos, _jumpEndPos, t);
       
        float arc = Mathf.Sin(t * Mathf.PI); 
        curPos.y += arc * 5f;

        transform.position = curPos;
        
        Vector3 direction = (_jumpEndPos - curPos).normalized;
        direction.y = 0;
        RotateToDirection(direction);
    }

    public void JumpEnd()
    {
        transform.position = _jumpEndPos;
        
        _agent.CompleteOffMeshLink();
        _agent.isStopped = false;
    }
    public bool IsOnJumpTrigger()
    {
        if (!_agent.isOnOffMeshLink) return false;

        OffMeshLinkData linkData = _agent.currentOffMeshLinkData;
        _jumpStartPos = linkData.startPos;
        _jumpEndPos = linkData.endPos;
        
        if (_jumpEndPos.y <= _jumpStartPos.y) return false;
        
        Pause();
        
        _jumpDuration = Vector3.Distance(_jumpStartPos, _jumpEndPos) / MoveSpeed;
        
        
        return true;
    }
    private void Init()
    {
        _characterController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Pause()
    {
        // 둘이 웬만하면 같이 쓰자
       _agent.isStopped = true; //이동 일시정지
       _agent.ResetPath(); // 경로(목적지) 삭제
    }
    private void RotateToDirection(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
    }
}