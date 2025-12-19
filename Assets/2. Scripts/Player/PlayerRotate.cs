using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerRotate : MonoBehaviour
{
    [Header("회전 설정")]
    [SerializeField] private float _rotatationSpeed = 200f;

    [Header("카메라 참조")]
    [SerializeField] private CameraRotate _cameraRotate;

    //참조
    private PlayerInput _input;

    private void Awake()
    {
        Init();
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.State != EGameState.Playing) return;
        
        if (_cameraRotate != null)
        {
            transform.eulerAngles = new Vector3(0, _cameraRotate.AccumulationX, 0);
        }
    }

    private void Init()
    {
        _input = GetComponent<PlayerInput>();

        if (_cameraRotate == null)
        {
            _cameraRotate = Camera.main.GetComponent<CameraRotate>();
        }
    }
}
