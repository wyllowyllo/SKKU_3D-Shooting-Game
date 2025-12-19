using TMPro;
using UnityEngine;
using DG.Tweening;

public class TopviewCamera : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Transform _target;
    [SerializeField] private PlayerInput _playerInput;
    
    [SerializeField] private float _offsetY = 10f;

    [SerializeField] private float _zoomInSizeMax = 3f;
    [SerializeField] private float _zoomOutSizeMax = 10f;
    [SerializeField] private float _zoomDuration = 0.3f;
    [SerializeField] private float _zoomUnit = 2f;
    
    private Camera _camera;
    private Tween _zoomTween;

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
        UpdateZoom();
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

    private void UpdateZoom()
    {
        if (_playerInput == null) return;

        if (_playerInput.MapZoomIn)
        {
            ZoomIn();
        }
        if (_playerInput.MapZoomOut)
        {
            ZoomOut();
        }
    }
    public void ZoomIn()
    {
        if (_camera == null) return;
        
        
        _zoomTween?.Kill();
        
        float targetSize = Mathf.Max(_zoomInSizeMax, _camera.orthographicSize - _zoomUnit);

       
        _zoomTween = _camera.DOOrthoSize(targetSize, _zoomDuration)
            .SetEase(Ease.OutQuad);
    }

    public void ZoomOut()
    {
        if (_camera == null) return;
        
        _zoomTween?.Kill();
        
        float targetSize = Mathf.Min(_zoomOutSizeMax, _camera.orthographicSize + _zoomUnit);

       
        _zoomTween = _camera.DOOrthoSize(targetSize, _zoomDuration)
            .SetEase(Ease.OutQuad);
    }
}
