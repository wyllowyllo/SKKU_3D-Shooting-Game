using UnityEngine;

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
    
    public float AccumulationX { get => _accumulationX; set => _accumulationX = value; }
    public float AccumulationY { get => _accumulationY; set => _accumulationY = value; }

    private const float MaxVerticalRotationAngle = 80f;
    private void Awake()
    {
       Init();
    }
    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing) return;
        if (_input == null) return;

        RotateView();
    }

    private void Init()
    {
        Vector3 startAngle = transform.eulerAngles;
        _accumulationX = startAngle.y;
        _accumulationY = startAngle.x;
        
    }

    private void RotateView()
    {
        
        transform.eulerAngles = GetEulerAngles();
    }

    private Vector3 GetEulerAngles()
    {
        float mouseX = _input.MouseX;
        float mouseY = _input.MouseY;
        
        _accumulationX += mouseX * _rotateSpeed * Time.deltaTime;
        _accumulationY -= mouseY * _rotateSpeed * Time.deltaTime;
        
        _accumulationY = Mathf.Clamp(_accumulationY, -MaxVerticalRotationAngle, MaxVerticalRotationAngle);
        
        return new Vector3(_accumulationY, _accumulationX, 0);
    }
    
}