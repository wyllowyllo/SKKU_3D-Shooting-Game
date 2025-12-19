using UnityEngine;

public class AttackInfo
{
    private float _damage;
    private Vector3 _hitPoint;
    private Vector3 _attackDirection;


    public AttackInfo(float damage)
    {
        _damage = damage;
    }
    public AttackInfo(float damage, Vector3 attackDirection, Vector3 hitPoint) :  this(damage)
    {
        _attackDirection = attackDirection;
        _hitPoint = hitPoint;
    }

    public float Damage => _damage;

    public Vector3 AttackDirection => _attackDirection;
}