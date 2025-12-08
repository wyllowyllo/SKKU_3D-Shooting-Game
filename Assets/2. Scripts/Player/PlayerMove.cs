using System;
using UnityEngine;

/// <summary>
/// 키보드 누르면 캐릭터 그 방향으로 이동
/// </summary>
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private void Update()
    {
        // 1. 키보드 입력 받기
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");
        
        // 2. 입력에 따른 방향 구하기
        // 현재는 유니티 세상의 절대적인 방향이 기준(글로벌 좌표계)
        // 내가 원하는 것은 카메라가 쳐다보는 방향 기준
        Vector3 direction = new Vector3(inputX, 0, inputZ).normalized;
        
        // 따라서
        // 1. 글로벌 좌표 방향을 구한다
        // 2. 카메라가 쳐다보는 방향으로 변환한다 (즉 월드 -> 로컬)
        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = 0;
        
        // 3. 방향으로 이동시키기
        transform.position += direction * _moveSpeed * Time.deltaTime;
    }
}
