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
   
   public ValueStat Damage => _damage;

   public ValueStat MoveSpeed => _moveSpeed;

   public ValueStat RunSpeed => _runSpeed;

   public ValueStat JumpPower => _jumpPower;

   


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