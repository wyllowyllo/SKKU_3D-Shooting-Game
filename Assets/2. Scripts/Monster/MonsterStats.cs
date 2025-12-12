using UnityEngine;

public class MonsterStats : MonoBehaviour, IStat
{
    [SerializeField] private float _health = 100f;
    
    
    public bool IsLive =>  _health > 0f;
    
    public void TryTakeDamage(AttackInfo attackInfo)
    {
        _health -= attackInfo.Damage;
    }
}