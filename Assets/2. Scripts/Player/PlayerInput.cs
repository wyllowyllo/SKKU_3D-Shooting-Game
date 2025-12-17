using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float X => Input.GetAxisRaw("Horizontal");
    public float Z => Input.GetAxisRaw("Vertical");

    public bool Dash => Input.GetKey(KeyCode.LeftShift);
    public bool Jump => Input.GetButtonDown("Jump");
    public float MouseX => Input.GetAxis("Mouse X");
    public float MouseY => Input.GetAxis("Mouse Y");
    
    public bool BombThrow => Input.GetKeyDown(KeyCode.G);
    public bool Fire => Input.GetMouseButton(0) && GameManager.Instance.State == EGameState.Playing;
    public bool Pointed => Input.GetMouseButton(0) &&  GameManager.Instance.State == EGameState.Auto;
    public bool Reload => Input.GetKeyDown(KeyCode.R);
    public bool ViewToggle => Input.GetKeyDown(KeyCode.T);
    public bool TopMode => Input.GetKeyDown(KeyCode.K);
    public bool ZoomIn => Input.GetKey(KeyCode.O);
    public bool ZoomOut => Input.GetKey(KeyCode.P);
    public Vector3 Direction
    {
        get
        {
            return new Vector3(X, 0, Z).normalized;
        }
    }
    
    
    
}