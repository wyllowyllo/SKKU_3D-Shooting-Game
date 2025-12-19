using UnityEngine;

public class AttackInfo
{
    private float _damage;
    private Vector3 _hitPoint;
    private Vector3 _attackDirection;
    private Vector3 _hitPointNormal;

    public AttackInfo(float damage)
    {
        _damage = damage;
    }
    public AttackInfo(float damage, Vector3 attackDirection) :  this(damage)
    {
        _attackDirection = attackDirection;
    }
    public AttackInfo(float damage, Vector3 attackDirection, Vector3 hitPoint) :  this(damage, attackDirection)
    {
        _attackDirection = attackDirection;
        _hitPoint = hitPoint;
    }
    
    public AttackInfo(float damage, Vector3 attackDirection, Vector3 hitPoint, Vector3 hitPointNormal) :  this(damage, attackDirection, hitPoint)
    {
        _attackDirection = attackDirection;
        _hitPoint = hitPoint;
        _hitPointNormal = hitPointNormal;
    }

    public float Damage => _damage;

    public Vector3 AttackDirection => _attackDirection;

    public Vector3 HitPoint => _hitPoint;

    public Vector3 HitPointNormal => _hitPointNormal;
}