using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ErrorTest : MonoBehaviour
{
    // 오류 : 프로그램이 비정상적으로 동작하게 하는 문제
    // 예외 : 프로그램이 실행 중 발생하고, 개발자가 예상하고 처리할 수 있는 문제
    
    // 오류는 크게 3가지
    // 1. 문법 오류
    // 2. 런타임 오류 : 실행 시 발생하는 오류
    // 3. 알고리즘 or 휴면 오류 or AI 오류 : 주어진 문제에 대해 잘못된 해석이나 구현으로 내가 원하지 않는 결과물이 나오는 오류
    
    // 유니티에서 런타임에(플레이) 주로 나타나는 오류(예외)
    
    // NullReferenceException  
    // -> 사용하고자 하는 객체가 null 값일 때 그 객체의 필드나 메서드에 접근하려고 하면 
    private void Start()
    {
        // MissingComponentException
        // -> 사용하고자 하는 객체가 null 일 때
        Rigidbody rb = GetComponent<Rigidbody>(); 
        Debug.Log(rb.linearVelocity);
        
        // NullReferenceException  
        // -> 사용하고자 하는 객체가 null 값일 때 그 객체의 필드나 메서드에 접근하려고 하면 
        Rigidbody rb2 = null; 
        Debug.Log(rb2.linearVelocity);
    }
    
    // 쓸 수 있는 해결책들
    // 1. 항상 null 체크하는 방어코드 작성한다
    // 2. Unity 스크립트라면 [RequireComponent(typeof())] 어트리뷰트 활용
    // 3. TryGetComponent등의 메서드를 적절히 사용한다
    // 등등...
   
}
