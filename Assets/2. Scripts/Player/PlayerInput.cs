using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float X => Input.GetAxisRaw("Horizontal");
    public float Z => Input.GetAxisRaw("Vertical");

    public bool Dash => Input.GetKey(KeyCode.LeftShift);
    public bool Jump => Input.GetButtonDown("Jump");
    public bool Rotate => Input.GetMouseButton(1);
    public float MouseX => Input.GetAxis("Mouse X");
    public float MouseY => Input.GetAxis("Mouse Y");
    
    public bool ViewToggle => Input.GetKeyDown(KeyCode.T);
    public Vector3 Direction
    {
        get
        {
            return new Vector3(X, 0, Z).normalized;
        }
    }
    
    
}