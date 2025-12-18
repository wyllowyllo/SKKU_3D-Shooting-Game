using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 키보드 누르면 캐릭터 그 방향으로 이동
/// </summary>
[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
[RequireComponent(typeof(PlayerStats), typeof(NavMeshAgent))]
public class PlayerMove : MonoBehaviour
{
    
    [Header("스테미나 소모량")]
    [SerializeField] private float _runStaminaPerSec = 20f;
    [SerializeField] private float _secondJumpStamina = 50f;
    
    // 참조
    private CharacterController _characterController;
    private PlayerInput _input;
    private PlayerStats _playerStats;
    private Camera _cam;
    private NavMeshAgent _agent;
    private Animator _animator;
    
    // 이동 관련 
    private Vector3 direction;
    private float _yVelocity = 0f;
    
    
    // 플래그 변수
    private bool _isFirstJump;
    private bool _isSecondJump;
    
    
    // 프로퍼티;
    //public float CurStamina => _curStamina;
    public bool IsGrounded => _characterController.isGrounded;
    private bool IsMove => (_input.X!=0 || _input.Z!=0);
    
    // 상수
    private const float Gravity = -9.81f;
    
    private void Awake()
    {
        Init();
    }
    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing
            && GameManager.Instance.State != EGameState.Auto) return;
        
       SetMoveDirection();
       ApplyMovement();
    }

    private void Init()
    {
        _characterController = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInput>();
        _playerStats = GetComponent<PlayerStats>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        
        _cam = Camera.main;

        _agent.speed = _playerStats.MoveSpeed;
    }
    private void SetMoveDirection()
    {
        _yVelocity += Gravity * Time.deltaTime;

        TryJump();

        if (GameManager.Instance.IsTopMode)
        {
            if (!_input.Pointed)
            {
                _agent.isStopped = true;
                return;
            }

            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _agent.isStopped = false;
                _agent.SetDestination(hit.point);
            }
            else
            {
                _agent.isStopped = true;
            }

            return;
        }
        else
        {
            direction = _input.Direction;
           
            
            direction.Normalize();
            direction = _cam.transform.TransformDirection(direction);
        }


        direction.y = _yVelocity;
    }

    private void ApplyMovement()
    {
        if (GameManager.Instance.IsTopMode)
        {
            UpdateAnimationForTopMode();
            return;
        }

        float applySpeed = 1f;

        if (_input.Dash && IsMove && IsGrounded)
        {
            if (_playerStats.StaminaStat.Value > 0)
            {
                applySpeed = _playerStats.RunSpeed;
                _playerStats.StaminaStat.Decrease(_runStaminaPerSec * Time.deltaTime);
            }
        }
        else
        {
            applySpeed = _playerStats.MoveSpeed;

            if (IsGrounded)
            {
                _playerStats.StaminaStat.Regenerate(Time.deltaTime);
            }

        }

        _characterController.Move(direction * applySpeed * Time.deltaTime);
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Vector3 horizontalVelocity = new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z);
        float normalizedSpeed = horizontalVelocity.magnitude / _playerStats.RunSpeed;

        _animator.SetFloat("Speed", normalizedSpeed);
    }

    private void UpdateAnimationForTopMode()
    {
        Vector3 horizontalVelocity = new Vector3(_agent.velocity.x, 0, _agent.velocity.z);
        float normalizedSpeed = horizontalVelocity.magnitude / _playerStats.RunSpeed;

        _animator.SetFloat("Speed", normalizedSpeed);
    }

    private void Jump()
    {
        
        if (!_isFirstJump)
        {
            _yVelocity = _playerStats.JumpPower;
            _isFirstJump = true;
        }
        else if (!_isSecondJump)
        {
            if (!_playerStats.StaminaStat.TryConsume(_secondJumpStamina))
            {
                Debug.Log("스테미나가 부족합니다");
                return;
            }
            
            _yVelocity = _playerStats.JumpPower;
            _isSecondJump = true;
        }
    }
    
    private void TryJump()
    {
        // 점프 상태 업데이트
        if (IsGrounded)
        {
            _isFirstJump = false;
            _isSecondJump = false;
        }

        // 점프 적용
        if (!_input.Jump) return;
        
        Jump();
    }

   
}
