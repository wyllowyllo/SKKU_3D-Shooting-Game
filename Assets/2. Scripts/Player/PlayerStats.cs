using UnityEngine;

public class PlayerStats : MonoBehaviour
{
   [Header("체력")] 
   [SerializeField] private ComsumableStat _health;
   
   [Header("스테미나")]
   [SerializeField] private ComsumableStat _stamina;
  
   
   [Header("스텟 (값스텟)")]
   [SerializeField] private ValueStat _damage;
   [SerializeField] private ValueStat _moveSpeed;
   [SerializeField] private ValueStat _runSpeed;
   [SerializeField] private ValueStat _jumpPower;

   // 프로퍼티
   public ComsumableStat Health => _health;

   public ComsumableStat Stamina => _stamina;
   
   public float CurHealth => _health.Value;
   public float MaxHealth => _health.MaxValue;
   public float CurStamina => _stamina.Value;
   public float MaxStamina => _stamina.MaxValue;
   
   public float Damage => _damage.Value;

   public float MoveSpeed => _moveSpeed.Value;

   public float RunSpeed => _runSpeed.Value;

   public float JumpPower => _jumpPower.Value;

   


   private void Start()
   {
      Health.Initialize();
      Stamina.Initialize();
   }

   private void Update()
   {
      float deltaTime = Time.deltaTime;
      
      Health.Regenerate(deltaTime);
      Stamina.Regenerate(deltaTime);
   }
}