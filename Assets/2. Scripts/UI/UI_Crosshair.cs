using System;
using UnityEngine;
using DG.Tweening;

public class UI_Crosshair : MonoBehaviour
{
    [Header("참조")] [SerializeField] private Gun _gun;

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
            .Append(_rectTransform.DOScale(1.3f, 0.05f))
            .Append(_rectTransform.DOScale(1f, 0.1f))
            .SetAutoKill(false)
            .Pause();
    }
}

   
