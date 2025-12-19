using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    [SerializeField] private Transform _healthBarTransform;
    [SerializeField] private Image _guageImg;
    
    private MonsterDamagables _monsterDamagables;
    private float _lastHealth;
    private void Awake()
    {
        _monsterDamagables = GetComponent<MonsterDamagables>();
    }
    
    private void LateUpdate()
    {
        if (_lastHealth != _monsterDamagables.CurHealth)
        {
            _guageImg.fillAmount = _monsterDamagables.CurHealth / _monsterDamagables.MaxHealth;
            _lastHealth = _monsterDamagables.CurHealth;
        }
        
        // 빌보드 기법 : 카메라의 위치와 회전에 상관없이 항상 정면을 바라보게 하는 기법
        _healthBarTransform.forward = Camera.main.transform.forward;
    }

}
