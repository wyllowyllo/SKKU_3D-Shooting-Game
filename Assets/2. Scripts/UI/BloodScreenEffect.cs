using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BloodScreenEffect : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private PlayerStats _playerStats;

    [Header("피격 flash(번쩍임) 설정")]
    [SerializeField] private float _hitFlashIntensity = 0.6f;
    [SerializeField] private float _hitFlashDuration = 0.3f;
    [SerializeField] private float _hitFadeOutDuration = 0.5f;

    [Header("체력 설정")]
    [SerializeField] private float _lowHealthThreshold = 0.3f; 
    [SerializeField] private float _lowHealthMaxIntensity = 0.4f;
    [SerializeField] private float _lowHealthPulseSpeed = 2f;

    private Image _bloodScreenImage;
    
    private float _currentIntensity = 0f;
    private Tweener _currentTween;
    private bool _isFlashing = false;


    private void Awake()
    {
       Init();
    }
   

    private void Update()
    {
        if (_playerStats == null || _isFlashing) return;
        
       UpdateBloodScreen();
    }

    /// <summary>
    /// 피격 시 호출됨. blood screen image 알파 값 조정
    /// </summary>
    public void ShowHitEffect()
    {
        if (_bloodScreenImage == null) return;
        
        _currentTween?.Kill();
        _isFlashing = true;
        
        _currentIntensity = _hitFlashIntensity;
        SetBloodScreenAlpha(_currentIntensity);

        // Fade out after delay
        _currentTween = DOVirtual.Float(_hitFlashIntensity, 0f, _hitFlashDuration + _hitFadeOutDuration, value =>
        {
            if (!_isFlashing) return;

            if (value > 0f)
            {
                SetBloodScreenAlpha(value);
            }
        })
        .SetEase(Ease.OutQuad)
        .SetDelay(_hitFlashDuration)
        .OnComplete(() =>
        {
            _isFlashing = false;
            _currentIntensity = 0f;
        });
    }

    private void Init()
    {
        _bloodScreenImage = GetComponent<Image>();
        
        if (_bloodScreenImage == null)
        {
            Debug.LogError("BloodScreenEffect: Blood screen image is not assigned!");
            enabled = false;
            return;
        }

        if (_playerStats == null)
        {
           
            Debug.LogError("BloodScreenEffect: PlayerStats not found!");
            enabled = false;
            return;
            
        }
        
        SetBloodScreenAlpha(0f);
    }

    private void UpdateBloodScreen()
    {
        float healthPercent = _playerStats.CurHealth / _playerStats.MaxHealth;

        if (healthPercent <= _lowHealthThreshold)
        {
            float healthRatio = 1f - (healthPercent / _lowHealthThreshold);
            float targetIntensity = _lowHealthMaxIntensity * healthRatio;
            
            float pulse = (Mathf.Sin(Time.time * _lowHealthPulseSpeed) + 1f) * 0.5f;
            float finalIntensity = targetIntensity * (0.5f + pulse * 0.5f);

            SetBloodScreenAlpha(finalIntensity);
        }
        else if (_currentIntensity > 0f)
        {
            _currentIntensity = Mathf.Lerp(_currentIntensity, 0f, Time.deltaTime * 2f);
            SetBloodScreenAlpha(_currentIntensity);
        }
    }
    private void SetBloodScreenAlpha(float alpha)
    {
        if (_bloodScreenImage == null) return;

        _currentIntensity = alpha;
        Color color = _bloodScreenImage.color;
        color.a = Mathf.Clamp01(alpha);
        _bloodScreenImage.color = color;
    }
    
}
