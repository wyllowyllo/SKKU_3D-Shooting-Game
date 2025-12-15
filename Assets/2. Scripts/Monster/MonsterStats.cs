using UnityEngine;

public class MonsterStats : MonoBehaviour, IStat
{
    [SerializeField] private float _health = 100f;

    // 참조
    private MonsterStateController _stateController;

    public bool IsLive =>  _health > 0f;

    private void Awake()
    {
        _stateController = GetComponent<MonsterStateController>();
    }

    public void TryTakeDamage(AttackInfo attackInfo)
    {
        _health -= attackInfo.Damage;
        
        _stateController?.TryTakeDamage(attackInfo);
        
    }
}