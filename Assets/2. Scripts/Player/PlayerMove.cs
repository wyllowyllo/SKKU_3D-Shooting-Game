using System;
using UnityEngine;

/// <summary>
/// 키보드 누르면 캐릭터 그 방향으로 이동
/// </summary>
[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerMove : MonoBehaviour
{
    [Header("기본 이동")]
    [SerializeField] private float _moveSpeed;
   
    
    [Header("빠른 이동")]
    [SerializeField] private float _staminaMax = 100f;
    [SerializeField] private float _speedFactor = 1.5f;
    [SerializeField] private float _staminaUnitForTime = 20f;
    
    [Header("점프")]
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _staminaForSecondJump = 50f;
    
    // 참조
    private CharacterController _characterController;
    private PlayerInput _input;

    // 이동 관련 
    private Vector3 direction;
    private float _yVelocity = 0f;
    private float _curStamina = 0f;
   
    
    // 플래그 변수
    private bool _isFirstJump;
    private bool _isSecondJump;
    
    
    // 프로퍼티;
    public float CurStamina => _curStamina;
    public float StaminaMax => _staminaMax;
    public bool IsGrounded => _characterController.isGrounded;
    private bool IsMove => (_input.X!=0 || _input.Z!=0);
    
    // 상수
    private const float Gravity = -9.81f;
    
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInput>();
        
        _curStamina = StaminaMax;
    }
    private void Update()
    {
       GetDirection();
       Jump();
       Move();
    }

    private void GetDirection()
    {
        _yVelocity += Gravity * Time.deltaTime;

        direction = _input.Direction;
       
        
       
        
        // 따라서
        // 1. 글로벌 좌표 방향을 구한다
        // 2. 카메라가 쳐다보는 방향으로 변환한다 (즉 월드 -> 로컬)
        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = _yVelocity;
    }

    private void Move()
    {
        float boost = 1f;
        if (_input.Dash && IsMove && IsGrounded)
        {
            if (CurStamina > 0)
            {
                boost = _speedFactor;
                _curStamina = CurStamina - _staminaUnitForTime * Time.deltaTime;
                _curStamina = Mathf.Max(CurStamina, 0);
            }
        }
        else
        {
            boost = 1f;

            if (IsGrounded)
            {
                _curStamina = CurStamina + _staminaUnitForTime * Time.deltaTime;
                _curStamina = Mathf.Min(CurStamina, StaminaMax);
            }
            
        }
        
        // 3. 방향으로 이동시키기
        //transform.position += direction * _moveSpeed * Time.deltaTime;
        _characterController.Move(direction * _moveSpeed * boost * Time.deltaTime);
    }

    private void Jump()
    {
        if (!_input.Jump) return;

        if (IsGrounded)
        {
            _isFirstJump = false;
            _isSecondJump = false;
        }
        
        
        if (!_isFirstJump)
        {
            _yVelocity = _jumpPower;
            _isFirstJump = true;
        }
        else if (!_isSecondJump && _curStamina >= _staminaForSecondJump)
        {
            _yVelocity = _jumpPower;
            _curStamina =  Mathf.Max(0,  _curStamina - _staminaForSecondJump);
            _isSecondJump = true;
        }
    }

   
}
