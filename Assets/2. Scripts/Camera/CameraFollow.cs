using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    [Header("Following Target")]
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _fpsTargetOffset;
    [SerializeField] private Vector3 _tpsTargetOffset;
    private Vector3 _camOffset;

    [Header("스위칭 시간")]
    [SerializeField] private float _switchDuration = 0.5f;
    
    
    private bool _isTpsMode = true;
    
    private PlayerInput _input;

    private void Awake()
    {
        Init();
    }
    private void LateUpdate()
    {
        if(_target == null) return;
        
        UpdateViewMode();
        FollowTarget();
    }

    private void Init()
    {
        _input = _target?.GetComponentInParent<PlayerInput>();
        
        _camOffset = _tpsTargetOffset;
    }
    
    private void FollowTarget()
    {
        transform.position = _target.position + transform.TransformDirection(_camOffset);
    }

    
    private void UpdateViewMode()
    {
        if (_input == null) return;
        if (!_input.ViewToggle) return;
      
        _isTpsMode = !_isTpsMode;
        
        Vector3 targetOffset = _isTpsMode ? _tpsTargetOffset : _fpsTargetOffset;

        DOTween.To(
                () => _camOffset,
                x => _camOffset = x,
                targetOffset,
                _switchDuration
            )
            .SetEase(Ease.OutCubic)
            .SetUpdate(UpdateType.Late);
    }

   
    
    
}
