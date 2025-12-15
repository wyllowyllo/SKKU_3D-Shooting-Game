using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class PlayerGunFire : MonoBehaviour
{
    [Header("장착된 총")]
    [SerializeField, DisallowNull] private Gun _curGun;
    
    [Header("카메라 참조")]

    [SerializeField] private CameraRotate _camRotate;
    // 참조
    private PlayerInput _input;
  
    
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing) return;
        if (_curGun == null) return;
            
        TryFire();
        TryReload();
    }

    private void Init()
    {
        if (_curGun == null)
        {
            Debug.LogError($"{nameof(_curGun)} is not assigned on {name}", this);
            enabled = false;             
            return;
        }
        
        _input = GetComponent<PlayerInput>();

        if (_camRotate == null)
        {
            _camRotate = Camera.main.GetComponent<CameraRotate>();
        }

        if (_camRotate != null)
        {
            _curGun?.CamInit(_camRotate.transform);
        }
        
    }

  

    private void TryFire()
    {
        if (!_input.Fire) return;
        //if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;  // UI 클릭 무시

        _curGun?.Fire();
    }

    private void TryReload()
    {
        if (!_input.Reload) return;
        
        _curGun?.Reload();
    }
}
