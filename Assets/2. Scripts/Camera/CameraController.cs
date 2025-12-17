using System;
using UnityEngine;

[RequireComponent(typeof(CameraFollow), typeof(CameraRotate), typeof(CameraRecoil))]
[RequireComponent(typeof(TopviewCamera))]
public class CameraController : MonoBehaviour
{
   [Header("입력 참조")]
   [SerializeField] private PlayerInput _input;
   
   private CameraFollow _followController;
   private CameraRotate _rotateController;
   private CameraRecoil _recoilController;
   private TopviewCamera _topViewController;
   
   private bool _topMode = false;

   private void Awake()
   {
     Init();
   }

   private void Update()
   {
      if (_input == null || !_input.TopMode) return;

      SwitchViewMode();
   }

   private void SwitchViewMode()
   {
      _topMode = !_topMode;

      
      _followController.enabled = !_topMode;
      _rotateController.enabled = !_topMode;
      _recoilController.enabled = !_topMode;

      _topViewController.enabled = _topMode;

      GameManager.Instance.IsTopMode = _topMode;
   }

   private void Init()
   {
      _followController = GetComponent<CameraFollow>();
      _rotateController = GetComponent<CameraRotate>();
      _recoilController = GetComponent<CameraRecoil>();
      _topViewController = GetComponent<TopviewCamera>();
      
      _followController.enabled = true;
      _rotateController.enabled = true;
      _recoilController.enabled = true;
      _topViewController.enabled = false;
   }
   
   
   
   
}
