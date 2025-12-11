using UnityEngine;
using DG.Tweening;

/// <summary>
/// 카메라를 마우스 방향으로 회전
/// </summary>
public class CameraRotate : MonoBehaviour
{

    [Header("참조")]
    [SerializeField] private PlayerInput _input;
    
    [Header("카메라 회전 설정")]
    [SerializeField] private float _rotateSpeed = 200f;
    
    // 게임 시작하면 y축이 0도에서 시작. 살짝 아래 쳐다보면 -> -1도가 되어야 함
    // 그러나 실제로는 359도로 변환이 되고(유니티 내부적으로 [0,360] degree 체계를 가지고 있음), 코드에서 clamp를 걸었기 때문에 90도가 됨

    // 유니티는 0 ~ 360 각도 체계이므로, 아래와 같이 우리가 따로 저장할 -360 ~ 360 체계로 누적할 변수를 둔다.
    private float _accumulationX = 0;
    private float _accumulationY = 0;
    
    private void Update()
    {
        if (_input == null) return;
        if (!_input.Rotate) return;
        
        // 1. 마우스 입력 받기
        float mouseX = _input.MouseX;
        float mouseY = _input.MouseY;
        
       // 2. 마우스 입력을 누적한다 (누적된 회전 방향)
       _accumulationX += mouseX * _rotateSpeed * Time.deltaTime;
       _accumulationY -= mouseY * _rotateSpeed * Time.deltaTime;
       
       // 3. 사람처럼 -90 ~ 90 도 사이로 제한한다.
       _accumulationY = Mathf.Clamp(_accumulationY, -90f, 90f);
       
       // 4. 회전 방향으로 카메라 회전하기
       // 새로운 위치 = 이전 위치 + (속력 * 방향 * 시간)
       // 새로운 회전 = 이전 회전 + (속력 * 방향 * 시간)
       transform.eulerAngles = new Vector3(_accumulationY, _accumulationX, 0);
       //transform.localRotation = Quaternion.Euler(_accumulationY, _accumulationX, 0f);
       
       /*// 2. 입력에 따른 회전 방향 만들기
       Vector3 direction = new Vector3(-mouseY, mouseX, 0f);
       
       
       // 3. 회전 방향으로 카메라 회전하기
       // 새로운 위치 = 이전 위치 + (속력 * 방향 * 시간)
       // 새로운 회전 = 이전 회전 + (속력 * 방향 * 시간)
       Vector3 eulerAngle =  transform.eulerAngles + _rotateSpeed * direction * Time.deltaTime;
       eulerAngle.y = Mathf.Clamp(eulerAngle.y, -90f, 90f);
       transform.eulerAngles = eulerAngle;*/
       
       // 쿼너티언(사원수) : 쓰는 이유는 짐벌락 현상 방지
       
    }
    
    public void AddRecoil(float upRecoil, float sideRecoil, float duration)
    {
        // 상하 반동
        DOTween.To(
            () => _accumulationY,
            x => _accumulationY = x,
            _accumulationY - upRecoil,
            duration * 0.2f
        );

        // 좌우 반동
        DOTween.To(
            () => _accumulationX,
            x => _accumulationX = x,
            _accumulationX + Random.Range(-sideRecoil, sideRecoil),
            duration * 0.2f
        );
    }

    
}


