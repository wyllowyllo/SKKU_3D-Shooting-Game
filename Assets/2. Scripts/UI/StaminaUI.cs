using UnityEngine;
using UnityEngine.UI;


public class StaminaUI : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private PlayerStats _player ;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }
    private void LateUpdate()
    {
        if (_player == null || _slider == null) return;

       _slider.value = (_player.Stamina.Value / _player.Stamina.MaxValue);
    }
}
