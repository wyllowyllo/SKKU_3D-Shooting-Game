using UnityEngine;

public class PlayerStats : MonoBehaviour
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

   


   private void Start()
   {
      HealthStat.Initialize();
      StaminaStat.Initialize();
   }

   private void Update()
   {
      float deltaTime = Time.deltaTime;
      
      HealthStat.Regenerate(deltaTime);
      StaminaStat.Regenerate(deltaTime);
   }
}