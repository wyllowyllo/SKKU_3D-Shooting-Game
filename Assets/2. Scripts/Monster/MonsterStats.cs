using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    
    
    public bool IsLive =>  _health > 0f;
    
    public void TryTakeDamage(float damage)
    {
        _health -= damage;
    }
}