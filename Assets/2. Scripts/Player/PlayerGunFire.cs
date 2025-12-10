using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerGunFire : MonoBehaviour
{
    [Header("장착된 총")]
    [SerializeField] private Gun _curGun;
    
    // 참조
    private PlayerInput _input;
  
    
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
        
        if(_curGun == null)
            _curGun = GetComponentInChildren<Gun>();
    }
    private void Fire()
    {
        if (!_input.Fire) return;
        
        _curGun?.Shoot();
    }
}
