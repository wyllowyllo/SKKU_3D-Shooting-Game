using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStat : MonoBehaviour, IDamagable
{
   [Header("체력")] 
   [SerializeField] private ComsumableStat healthStat;
   
   [Header("스테미나")]
   [SerializeField] private ComsumableStat staminaStat;
  
   
   [Header("스텟 (값스텟)")]
   [SerializeField] private ValueStat _damage;
   [SerializeField] private ValueStat _moveSpeed;
   [SerializeField] private ValueStat _runSpeed;
   [SerializeField] private ValueStat _jumpPower;

   [Header("코인")]
   [SerializeField] private ValueStat _gold;

   [Header("UI 이펙트")]
   [SerializeField] private BloodScreenEffect _bloodScreenEffect;

   private Animator _animator;
   
   //이벤트
   //public static event Action OnDataChanged;
   private UnityEvent _hitEvent = new UnityEvent();
  
   
   // 프로퍼티
   public ComsumableStat HealthStat => healthStat;
   public ComsumableStat StaminaStat => staminaStat;
   public float CurHealth => healthStat.Value;
   public float MaxHealth => healthStat.MaxValue;
   public float CurStamina => staminaStat.Value;
   public float MaxStamina => staminaStat.MaxValue;
   public float Damage => _damage.Value;
   public float MoveSpeed => _moveSpeed.Value;
   public float RunSpeed => _runSpeed.Value;
   public float JumpPower => _jumpPower.Value;
   public int Gold => Mathf.RoundToInt(_gold.Value);
   

   public UnityEvent HitEvent => _hitEvent;

   // 플래그 변수
   private bool _isDead;

   
   private void Start()
   {
      HealthStat.Initialize();
      StaminaStat.Initialize();
   }
   private void Update()
   {
      if (GameManager.Instance.State != EGameState.Playing) return;
      if (_isDead) return;
      
      float deltaTime = Time.deltaTime;
      
      RegenStamina(deltaTime);
   }
   
   public void TryTakeDamage(AttackInfo attackInfo)
   {
      if(_isDead || attackInfo.Damage <= 0) return;
      
      HealthStat.Decrease(attackInfo.Damage);
      
      if (healthStat.Value > 0)
      {
        // Hit 이펙트
        _bloodScreenEffect?.ShowHitEffect();
        HitEvent?.Invoke();

        // 레이어 가중치
        _animator?.SetLayerWeight(2, healthStat.Value / HealthStat.MaxValue);
        Debug.Log("몬스터에게 피격됨!");
      }
      else
      {
         // TODO : Die 이펙트, 애니메이션
         _isDead = true;
         
         GameManager.Instance.GameOver();
      }
   }
   
  

   private void RegenStamina(float time)
   {
      StaminaStat.Regenerate(time);
   }
   private void RegenHealth(float time)
   {
      HealthStat.Regenerate(time);
   }

   public void AddGold(int amount)
   {
      if (amount <= 0) return;
      _gold.Increase(amount);
      //OnDataChanged?.Invoke();
   }

   public bool TrySpendGold(int amount)
   {
      if (Gold < amount) return false;
      _gold.Decrease(amount);
      //OnDataChanged?.Invoke();
      return true;
   }

}