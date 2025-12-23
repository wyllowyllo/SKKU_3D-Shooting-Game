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

    [Header("Gold UI")]
    [SerializeField] private TextMeshProUGUI _goldText;

    private PlayerStat _playerStat ;
    private PlayerBombFire _playerBombFire ;
    private Gun _gunInfo;

    private Coroutine _prevHPRoutine;
    private void Awake()
    { 
         Init();
         
         _gunInfo?.OnReload.AddListener(UpdateReloadBar);
        
         //_playerStat?.HitEvent.AddListener(UpdateHealthBar);

         // 플레이어 스탯의 데이터의 변화가 있을 때마다 Refresh호출
         PlayerStat.OnDataChanged += RefreshBars;
    }

    private void Start()
    {
        
    }

    private void RefreshBars()
    {
        UpdateHealthBar();
        UpdateStaminaBar();
        UpdateGoldText();
    }
    
    private void LateUpdate()
    {
        // 유트브에 영상이 올라왔는지 매번 새로고침 하는 것과 똑같다.
        // 시각적인 변화가 없음에도 데이터를 참조하고 UI를 수정하므로 성능이 저하된다.
        
        //UpdateStatBars();
        UpdateBombText();
        UpdateBulletText();
    }

    private void Init()
    {
        if (_player == null) return;

        _playerStat = _player.GetComponent<PlayerStat>();
        _playerBombFire = _player.GetComponent<PlayerBombFire>();
        _gunInfo = _player.GetComponentInChildren<Gun>();

        if (_healthFillImage != null)
        {
            _originalHealthColor = _healthFillImage.color;
        }
    }

    private void UpdateStaminaBar()
    {
        if (_playerStat == null) return;


        if (_staminaSlider != null)
        {
            _staminaSlider.value = (_playerStat.CurStamina / _playerStat.MaxStamina);
        }

    }

    private void UpdateGoldText()
    {
        if (_playerStat == null || _goldText == null) return;
        _goldText.text = $"Gold: {_playerStat.Gold}";
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
        float targetValue = _playerStat.CurHealth / _playerStat.MaxHealth;
        
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
