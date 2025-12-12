using System;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateSpeed = 5f;
    
    [Header("넉백")]
    [SerializeField] private float _knockBackForce = 4f;
    [SerializeField] private float _knockbackDuration = 0.15f;
    
    // 참조
    private CharacterController _characterController;


    //상수
    private const float HitRotateFactor = 0.05f;

    // 프로퍼티
    public float KnockbackDuration => _knockbackDuration;

    private void Awake()
    {
        Init();
    }

    
    public void MoveToTarget(Vector3 targetPosition)
    {
        // TODO : Run anim
        
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        _characterController.Move(direction * _moveSpeed * Time.deltaTime);
        
        RotateToDirection(direction);
    }

    public void Knockback(Vector3 knockbackDir)
    {
        Vector3 movement = knockbackDir * (_knockBackForce * Time.deltaTime);
        _characterController.Move(movement);
        
        RotateToDirection(knockbackDir * HitRotateFactor);
    }

    private void Init()
    {
        _characterController = GetComponent<CharacterController>();
    }
    private void RotateToDirection(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
    }
}