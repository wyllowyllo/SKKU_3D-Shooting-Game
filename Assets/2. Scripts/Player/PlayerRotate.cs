using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerRotate : MonoBehaviour
{
    [Header("회전 설정")]
    [SerializeField] private float _rotatationSpeed = 200f;
    private float _accumulationX = 0f;
    
    //참조
    private PlayerInput _input;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing) return;
        float mouseX = _input.MouseX;
        
        _accumulationX += mouseX * _rotatationSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, _accumulationX, 0);
    }

    private void Init()
    {
        _input = GetComponent<PlayerInput>();
        
    }
}
