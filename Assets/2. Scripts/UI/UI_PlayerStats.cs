using UnityEngine;
using UnityEngine.UI;


public class UI_PlayerStats : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private PlayerStats _player ;

    [Header("HP bar")]
    [SerializeField] private Slider _healthSlider;
    
    [Header("Stamina bar")]
    [SerializeField] private Slider _staminaSlider;
    
   
    
    private void LateUpdate()
    {
        if (_player == null) return;

        if(_staminaSlider != null)
            _staminaSlider.value = (_player.CurStamina / _player.MaxStamina);
        
        if(_healthSlider != null)
            _healthSlider.value = (_player.CurHealth / _player.MaxHealth);
    }
}
