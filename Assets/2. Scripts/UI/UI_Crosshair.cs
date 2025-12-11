using System;
using UnityEngine;
using DG.Tweening;

public class UI_Crosshair : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Gun _gun;
    
    [Header("Fire Effect 설정")]
    [Tooltip("격발 시 crosshair 스케일링")]
    [SerializeField] private float _fireScaling = 1.3f;
    
    [Tooltip("crosshair 스케일링 기간")]
    [SerializeField] private float _upScalingDuration = 0.05f;
    [SerializeField] private float _downScalingDuration = 0.1f;

    
    private RectTransform _rectTransform;

    private Sequence _scailSequence;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        SetTween();
    }

    private void OnFireEffect()
    {
        _scailSequence.Restart();
    }

    private void Init()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void SetTween()
    {
        _gun?.OnFire.AddListener(OnFireEffect);

        _scailSequence = DOTween.Sequence()
            .Append(_rectTransform.DOScale(1f * _fireScaling, _upScalingDuration))
            .Append(_rectTransform.DOScale(1f, _downScalingDuration))
            .SetAutoKill(false)
            .Pause();
    }
}

   
