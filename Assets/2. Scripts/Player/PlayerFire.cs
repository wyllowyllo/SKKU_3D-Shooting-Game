using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerFire : MonoBehaviour
{
   [Header("사격 설정")]
   [SerializeField] private Transform _fireTransform;
   [SerializeField] private float _throwPower = 15f;
   
   [Header("투사체 설정")]
   [SerializeField] private Bomb _bombPrefab;
   [SerializeField] private int _maxBombCnt = 5;
   private int _curBombCnt;
   
   // 참조
   private PlayerInput _input;
   private Camera _cam;

   public int MaxBombCnt => _maxBombCnt;

   public int CurBombCnt => _curBombCnt;

   private void Awake()
   {
      Init();
   }

   private void Update()
   {
      Fire();
   }

   private void Init()
   {
      _input = GetComponent<PlayerInput>();
      _cam = Camera.main;

      _curBombCnt = MaxBombCnt;
   }

   private void Fire()
   {
      if (!_input.Fire || CurBombCnt <= 0) return;
      
      Bomb bomb = Instantiate(_bombPrefab, _fireTransform.position, Quaternion.identity);
      Rigidbody bombRigid = bomb.GetComponent<Rigidbody>();
      bombRigid.AddForce(_cam.transform.forward * _throwPower, ForceMode.Impulse);
      _curBombCnt = CurBombCnt - 1;
   }
}
