using System;
using UnityEngine;

public class Drum : MonoBehaviour, IStat
{
   [SerializeField] private GameObject _explosionVFX;

   [SerializeField] private float _health;
   [SerializeField] private float _maxHealth;
   
   [SerializeField] private float _explosionRadius = 2f;
   [SerializeField] private float _explosionDamage = 1000f;
   [SerializeField] private float _explosionDelay = 0.5f;
   [SerializeField] private float _rocketStrength = 10f;
   private Rigidbody _rigid;
   
   private bool _isExplored;

   private void Awake()
   {
      _rigid = GetComponent<Rigidbody>();
      
      _health = _maxHealth;
   }
   
   public void TryTakeDamage(AttackInfo attackInfo)
   {
      if (_isExplored) return;
      if (attackInfo.Damage <= 0f) return;
     
      _health -= attackInfo.Damage;

      if (_health > 0f)
      {
         
      }
      else
      {
        Invoke(nameof(Explode), _explosionDelay);
         
      }
      
   }

   private void Explode()
   {
      _isExplored = true;
         
      Instantiate(_explosionVFX,  transform.position, transform.rotation);
         
      Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
      for (int i = 0; i < colliders.Length; i++)
      {  
         if (colliders[i].TryGetComponent<IStat>(out var stat))
         {
            stat.TryTakeDamage(new AttackInfo(_explosionDamage));
         }
      }

      _rigid.AddForce(Vector3.up * _rocketStrength, ForceMode.Impulse);
   }
   
   private void OnDrawGizmos()
   {
      /*Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, _explosionRadius);*/
   }
   
}
