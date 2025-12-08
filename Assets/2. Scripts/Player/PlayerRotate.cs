using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    private float _rotatationSpeed = 200f;
    private float _accumulationX = 0f;

    private void Update()
    {
        if (!Input.GetMouseButton(1)) return;
        
      
        float mouseX = Input.GetAxis("Mouse X");

        
        _accumulationX += mouseX * _rotatationSpeed * Time.deltaTime;
       
       
        transform.eulerAngles = new Vector3(0, _accumulationX, 0);
    }
}
