using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    [Header("입력 참조")]
    [SerializeField] private PlayerInput _input;
    
    [Header("Target in FPS")]
    [SerializeField] private Transform _fpsTarget;
    
    [Header("Target in TPS")]
    [SerializeField] private Transform _tpsTarget;

    private bool _isTpsMode = false;
    private void LateUpdate()
    {
        if (_fpsTarget == null || _tpsTarget == null) return;
        
        UpdateViewMode();
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (_isTpsMode)
            transform.position = _tpsTarget.position;
        else
            transform.position = _fpsTarget.position;
    }

    private void UpdateViewMode()
    {
        if (_input == null) return;
        if (!_input.ViewToggle) return;
      
        _isTpsMode = !_isTpsMode;
    }
    
    
}
