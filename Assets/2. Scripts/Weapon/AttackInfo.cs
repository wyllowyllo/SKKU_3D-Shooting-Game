using UnityEngine;

public class AttackInfo
{
    private float _damage;
    private Vector3 _hitDirection;


    public AttackInfo(float damage)
    {
        _damage = damage;
    }
    public AttackInfo(float damage, Vector3 hitDirection) :  this(damage)
    {
        _hitDirection = hitDirection;
    }

    public float Damage => _damage;

    public Vector3 HitDirection => _hitDirection;
}