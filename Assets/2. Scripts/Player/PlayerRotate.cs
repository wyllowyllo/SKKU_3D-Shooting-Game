using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerRotate : MonoBehaviour
{
    [Header("회전 설정")]
    private float _rotatationSpeed = 200f;
    private float _accumulationX = 0f;
    
    //참조
    private PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (!_input.Rotate) return;
        
      
        float mouseX = _input.MouseX;

        
        _accumulationX += mouseX * _rotatationSpeed * Time.deltaTime;
       
       
        transform.eulerAngles = new Vector3(0, _accumulationX, 0);
    }
}
