using System.Collections;
using DG.Tweening;
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

    [Header("스위칭 시간")]
    [SerializeField] private float _switchDuration = 0.5f;
    
    private bool _isTpsMode = false;
    private bool _isSwitching = false;
    
    private Transform _target;

    private void Awake()
    {
        Init();
    }
    private void LateUpdate()
    {
        if (_fpsTarget == null || _tpsTarget == null) return;
        
        UpdateViewMode();
        FollowTarget();
    }

    private void Init()
    {
        _target = _fpsTarget;
    }
    
    private void FollowTarget()
    {
        if (_isSwitching) return;
        
        transform.position = _target.position;
        
    }

    IEnumerator SwitchView(Transform followTarget)
    {
        _isSwitching = true;
        
        _target = followTarget;
        
        transform.DOKill();
        
        Sequence seq = DOTween.Sequence();

        seq.Join(
            transform.DOMove(followTarget.position, _switchDuration)
        );

        seq.Join(
            transform.DORotateQuaternion(followTarget.rotation, _switchDuration)
        );
        
        yield return seq.WaitForCompletion();

       
        
        _isSwitching = false;
       
    }

    private void UpdateViewMode()
    {
        if (_input == null) return;
        if (!_input.ViewToggle) return;
      
        _isTpsMode = !_isTpsMode;
        
        Transform followTarget = _isTpsMode ? _tpsTarget : _fpsTarget;
        StartCoroutine(SwitchView(followTarget));
    }

   
    
    
}
