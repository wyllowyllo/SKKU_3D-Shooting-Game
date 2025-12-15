using TMPro;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _offsetY = 10f;
    
    [SerializeField] private float _zoomInSizeMax = 5f;
    [SerializeField] private float _zoomOutSizeMax = 10f;
    
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        if (_camera == null)
        {
            _camera.orthographicSize =  _zoomOutSizeMax;
        }
    }
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

    public void ZoomIn()
    {
        if (_camera == null) return;
        
        Debug.Log("줌인");
        float targetSize = _camera.orthographicSize - 1f;
        _camera.orthographicSize = Mathf.Max(_zoomInSizeMax,  targetSize);
    }

    public void ZoomOut()
    {
        if (_camera == null) return;
        
        Debug.Log("줌아웃");
        float targetSize = _camera.orthographicSize + 1f;
        _camera.orthographicSize = Mathf.Min(_zoomOutSizeMax,  targetSize);
    }
}
