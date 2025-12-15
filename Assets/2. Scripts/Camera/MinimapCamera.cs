using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _offsetY = 10f;
    private void LateUpdate()
    {
        if (_target == null) return;
        
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        Vector3 targetPosition = _target.position;
        Vector3 finalPosition = _target.position + new Vector3(0f, _offsetY, 0f);
        
        transform.position = finalPosition;
    }

    private void UpdateRotation()
    {
        Vector3 targetAngle = _target.eulerAngles;
        targetAngle.x = 90f;
        
        transform.eulerAngles = targetAngle;
    }
}
