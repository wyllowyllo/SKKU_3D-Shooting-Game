using System;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateSpeed = 5f;
    
    
    // 참조
    private CharacterController _characterController;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void MoveToTarget(Vector3 targetPosition)
    {
        // TODO : Run anim
        
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        _characterController.Move(direction * _moveSpeed * Time.deltaTime);
        
        RotateToDirection(direction);
    }

    public void Knockback(Vector3 knockbackDir, float knockbackForce)
    {
        Vector3 movement = knockbackDir * (knockbackForce * Time.deltaTime);
        _characterController.Move(movement);
        
        RotateToDirection(knockbackDir);
    }
    private void RotateToDirection(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
    }
}