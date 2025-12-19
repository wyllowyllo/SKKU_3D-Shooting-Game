using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    [SerializeField] private Transform _healthBarTransform;
    [SerializeField] private Image _guageImg;
    
    private MonsterHealth _monsterHealth;
    private float _lastHealth;
    private void Awake()
    {
        _monsterHealth = GetComponent<MonsterHealth>();
    }
    
    private void LateUpdate()
    {
        if (_lastHealth != _monsterHealth.CurHealth)
        {
            _guageImg.fillAmount = _monsterHealth.CurHealth / _monsterHealth.MaxHealth;
            _lastHealth = _monsterHealth.CurHealth;
        }
        
        // 빌보드 기법 : 카메라의 위치와 회전에 상관없이 항상 정면을 바라보게 하는 기법
        _healthBarTransform.forward = Camera.main.transform.forward;
    }

}
