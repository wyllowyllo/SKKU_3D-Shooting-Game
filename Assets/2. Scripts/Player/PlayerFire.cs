using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerFire : MonoBehaviour
{
   [SerializeField] private Transform _fireTransform;
   [SerializeField] private Bomb _bombPrefab;
   [SerializeField] private float _throwPower = 15f;

   // 참조
   private PlayerInput _input;
   private Camera _cam;
   private void Awake()
   {
      Init();
   }

   private void Update()
   {
      if (_input.Fire)
      {
         Bomb bomb = Instantiate(_bombPrefab, _fireTransform.position, Quaternion.identity);
         Rigidbody bombRigid = bomb.GetComponent<Rigidbody>();
         bombRigid.AddForce(_cam.transform.forward * _throwPower, ForceMode.Impulse);
      }
   }

   private void Init()
   {
      _input = GetComponent<PlayerInput>();
      _cam = Camera.main;
   }
}
