using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 2f;
    [SerializeField] private float _damage = 1000f;
    
    [SerializeField] private GameObject _explosionVFX;

    private bool _isExploded = false;
    
    private void OnEnable()
    {
        _isExploded = false;
    }

    
    private void OnCollisionEnter(Collision other)
    {

        if (_isExploded) return;
        
        _isExploded = true;
        
        GameObject effectObject = Instantiate(_explosionVFX, transform.position, Quaternion.identity);

        // 가상의 구를 만들어서 그 구 영역 안에 있는 모든 콜라이더를 찾아서 배열로 반환한다
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius, LayerMask.NameToLayer("Monster"));
        for (int i = 0; i < colliders.Length; i++)
        {
            MonsterStateController monster = colliders[i].GetComponent<MonsterStateController>();
            monster.TryTakeDamage(new AttackInfo(_damage));
        }
        
        BombFactory.Instance.ReturnBomb(this);
    }
}
