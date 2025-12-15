using UnityEngine;

public class PlayerStats : MonoBehaviour, IStat
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
   
   // 플래그 변수
   private bool _isDead;

   
   private void Awake()
   {
      HealthStat.Initialize();
      StaminaStat.Initialize();
   }
   private void Update()
   {
      if (_isDead) return;
      
      float deltaTime = Time.deltaTime;
      
      RegenStamina(deltaTime);
      //RegenHealth(deltaTime);
   }
   
   public void TryTakeDamage(AttackInfo attackInfo)
   {
      if(_isDead || attackInfo.Damage <= 0) return;
      
      HealthStat.Decrease(attackInfo.Damage);
      
      if (healthStat.Value > 0)
      {
        // TODO : Hit 이펙트, 애니메이션
         
        Debug.Log("몬스터에게 피격됨!");
      }
      else
      {
         // TODO : Die 이펙트, 애니메이션
         _isDead = true;
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
  
}