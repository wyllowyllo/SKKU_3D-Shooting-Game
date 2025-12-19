using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class UI_PlayerStats : MonoBehaviour
{
    [Header("플레이어 참조")]
    [SerializeField] private Transform _player;

    [Header("HP bar")]
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Image _healthFillImage;
    [SerializeField] private Slider _delayHealthSlider;
    [SerializeField] private Image _delayFillImage;
    

    [Header("피격 효과")]
    [SerializeField] private float _delayTime = 0.5f;
    [SerializeField] private float _healthTweenDuration = 0.2f;
    [SerializeField] private float _delayHealthTweenDuration = 0.5f;
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashDuration = 0.15f;
    [SerializeField] private float _shakeDuration = 0.3f;
    [SerializeField] private float _shakeStrength = 10f;

    private Color _originalHealthColor;
    
    [Header("Stamina bar")]
    [SerializeField] private Slider _staminaSlider;

    [Header("Bomb Text")]
    [SerializeField] private TextMeshProUGUI _bombText;
    
    [Header("Bullet UI")]
    [SerializeField] private TextMeshProUGUI _bulletText;
    [SerializeField] private Slider _reloadGuage;
    
    private PlayerDamagables _playerDamagables ;
    private PlayerBombFire _playerBombFire ;
    private Gun _gunInfo;

    private Coroutine _prevHPRoutine;
    private void Awake()
    { 
         Init();
    }

    private void Start()
    {
        // 재장전 이벤트 리스너 등록
        _gunInfo?.OnReload.AddListener(UpdateReloadBar);
        
        _playerDamagables?.HitEvent.AddListener(UpdateHealthBar);
    }
    
    private void LateUpdate()
    {
        UpdateStatBars();
        UpdateBombText();
        UpdateBulletText();
    }

    private void Init()
    {
        if (_player == null) return;

        _playerDamagables = _player.GetComponent<PlayerDamagables>();
        _playerBombFire = _player.GetComponent<PlayerBombFire>();
        _gunInfo = _player.GetComponentInChildren<Gun>();

        if (_healthFillImage != null)
        {
            _originalHealthColor = _healthFillImage.color;
        }
    }

    private void UpdateStatBars()
    {
        if (_playerDamagables == null) return;

        
        if (_staminaSlider != null)
        {
            _staminaSlider.value = (_playerDamagables.CurStamina / _playerDamagables.MaxStamina);
        }
        if (_healthSlider != null)
        {
            //_healthSlider.value = (_playerStats.CurHealth / _playerStats.MaxHealth);
            
         
        }
    }

    private void UpdateHealthBar()
    {
        if(_prevHPRoutine != null) StopCoroutine(_prevHPRoutine);
        _prevHPRoutine = StartCoroutine(UpdateHealthBarRoutine());
    }
    private void UpdateBombText()
    {
        if (_playerBombFire == null || _bombText == null) return;


        _bombText.text = $"{_playerBombFire.CurBombCnt} / {_playerBombFire.MaxBombCnt}";
    }

    private void UpdateBulletText()
    {
        if (_gunInfo == null || _bulletText == null ) return;
        
        _bulletText.text = $"{_gunInfo.RemainBullets} / {_gunInfo.TotalBulletCnt}";
    }

    private void UpdateReloadBar()
    {
        
        StartCoroutine(UpdateReloadBarRoutine());
    }

    private IEnumerator UpdateHealthBarRoutine()
    {
        float targetValue = _playerDamagables.CurHealth / _playerDamagables.MaxHealth;
        
        _healthSlider.DOValue(targetValue, _healthTweenDuration).SetEase(Ease.OutQuad);

       
        _healthFillImage.DOColor(_flashColor, _flashDuration * 0.5f)
                        .OnComplete(() =>
                        {
                            _healthFillImage.DOColor(_originalHealthColor, _flashDuration * 0.5f);
                        });
        
        _healthSlider.transform.parent.DOShakePosition(_shakeDuration, _shakeStrength, 20);
        
        yield return new WaitForSeconds(_delayTime);

        _delayHealthSlider.DOValue(targetValue, _delayHealthTweenDuration).SetEase(Ease.OutQuad);
    }
    private IEnumerator UpdateReloadBarRoutine()
    {
        CanvasGroup canvasGroup = _reloadGuage.GetComponent<CanvasGroup>();
        _reloadGuage.value = 0f;
        
        DOTween.To(()=> canvasGroup.alpha, x=> canvasGroup.alpha = x, 1f, 0.1f);
       
        
        float t = 0f;

        while (t < _gunInfo.ReloadDuration)
        {
            t += Time.deltaTime;
            _reloadGuage.value = (t/_gunInfo.ReloadDuration);
            yield return null;
        }
        
       
        DOTween.To(()=> canvasGroup.alpha, x=> canvasGroup.alpha = x, 0f, 0.1f);
    }
}
