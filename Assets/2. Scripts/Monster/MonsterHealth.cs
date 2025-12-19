using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private ComsumableStat _health;

    [SerializeField] private GameObject _bloodEffectPrefab;
    // 참조
    private MonsterStateController _stateController;

    // 생성된 피 이펙트 추적
    private List<GameObject> _bloodEffects = new List<GameObject>();

    public bool IsLive => CurHealth > 0f;

    public float CurHealth => _health.Value;
    public float MaxHealth => _health.MaxValue;

    private void Awake()
    {
        _stateController = GetComponent<MonsterStateController>();

        _health.Initialize();
    }

    public void TryTakeDamage(AttackInfo attackInfo)
    {
        if (!IsLive) return;
        _health.Decrease(attackInfo.Damage);
        _stateController?.OnDamaged(attackInfo);

        GameObject bloodEffect =  Instantiate(_bloodEffectPrefab, attackInfo.HitPoint, Quaternion.identity, transform);
        bloodEffect.transform.forward = attackInfo.HitPointNormal;
        _bloodEffects.Add(bloodEffect);
    }

    // 모든 피 이펙트 제거
    public void ClearAllBloodEffects()
    {
        foreach (GameObject bloodEffect in _bloodEffects)
        {
            if (bloodEffect != null)
            {
                Destroy(bloodEffect);
            }
        }
        _bloodEffects.Clear();
    }
}