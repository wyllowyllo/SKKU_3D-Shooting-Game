using System;
using UnityEngine;

/// <summary>
/// 키보드 누르면 캐릭터 그 방향으로 이동
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("기본 이동")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpPower;
    
    [Header("빠른 이동")]
    [SerializeField] private float _staminaMax = 100f;
    [SerializeField] private float _speedFactor = 1.5f;
    [SerializeField] private float _staminaUnitForTime = 10f;
    
    
    private CharacterController _characterController;
    private float _yVelocity = 0f;
    private float _curStamina = 0f;
    private const float Gravity = -9.81f;

    
    private Vector3 direction;

    private bool _isMove;
    private bool _isFirstJump;
    private bool _isSecondJump;
    public float CurStamina => _curStamina;

    public float StaminaMax => _staminaMax;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

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
        // 0 . 중력을 누적한다
        _yVelocity += Gravity * Time.deltaTime;
        
        // 1. 키보드 입력 받기
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");
        _isMove = (inputX != 0 || inputZ != 0);
        
        // 2. 입력에 따른 방향 구하기
        // 현재는 유니티 세상의 절대적인 방향이 기준(글로벌 좌표계)
        // 내가 원하는 것은 카메라가 쳐다보는 방향 기준
        direction = new Vector3(inputX, 0, inputZ).normalized;
       
        
       
        
        // 따라서
        // 1. 글로벌 좌표 방향을 구한다
        // 2. 카메라가 쳐다보는 방향으로 변환한다 (즉 월드 -> 로컬)
        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = _yVelocity;
    }

    private void Move()
    {
        float boost = 1f;
        if (Input.GetKey(KeyCode.LeftShift) && _isMove)
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

           
            _curStamina = CurStamina + _staminaUnitForTime * Time.deltaTime;
            _curStamina = Mathf.Min(CurStamina, StaminaMax);
            
        }
        
        // 3. 방향으로 이동시키기
        //transform.position += direction * _moveSpeed * Time.deltaTime;
        _characterController.Move(direction * _moveSpeed * boost * Time.deltaTime);
    }

    private void Jump()
    {
        if (!Input.GetButtonDown("Jump")) return;

        if (_characterController.isGrounded)
        {
            _isFirstJump = false;
            _isSecondJump = false;
        }
        
        
        if (!_isFirstJump)
        {
            _yVelocity = _jumpPower;
            _isFirstJump = true;
        }
        else if (!_isSecondJump)
        {
            _yVelocity = _jumpPower;
            _isSecondJump = true;
        }
    }
    
}
