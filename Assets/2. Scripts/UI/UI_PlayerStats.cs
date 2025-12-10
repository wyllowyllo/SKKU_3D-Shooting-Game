using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_PlayerStats : MonoBehaviour
{
    [Header("플레이어 참조")]
    [SerializeField] private Transform _player;

    [Header("HP bar")]
    [SerializeField] private Slider _healthSlider;
    
    [Header("Stamina bar")]
    [SerializeField] private Slider _staminaSlider;

    [Header("Bomb Text")]
    [SerializeField] private TextMeshProUGUI _bombText;
    
    [Header("Bullet UI")]
    [SerializeField] private TextMeshProUGUI _bulletText;
    [SerializeField] private Slider _reloadGuage;
    
    private PlayerStats _playerStats ;
    private PlayerBombFire _playerBombFire ;
    private Gun _gunInfo;

    private void Awake()
    { 
         Init();
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
        
        _playerStats = _player.GetComponent<PlayerStats>();
        _playerBombFire = _player.GetComponent<PlayerBombFire>();
        _gunInfo = _player.GetComponentInChildren<Gun>();
    }

    private void UpdateStatBars()
    {
        if (_playerStats == null) return;

        
        if (_staminaSlider != null)
        {
            _staminaSlider.value = (_playerStats.CurStamina / _playerStats.MaxStamina);
        }
        if (_healthSlider != null)
        {
            _healthSlider.value = (_playerStats.CurHealth / _playerStats.MaxHealth);
        }
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
}
