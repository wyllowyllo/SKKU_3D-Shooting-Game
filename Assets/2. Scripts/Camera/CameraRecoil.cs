using UnityEngine;
using DG.Tweening;

/// <summary>
/// 카메라 반동 처리
/// </summary>
[RequireComponent(typeof(CameraRotate))]
public class CameraRecoil : MonoBehaviour
{
   
    private CameraRotate _cameraRotate;

    private void Awake()
    {
        _cameraRotate = GetComponent<CameraRotate>();
    }
    
    /// <summary>
    /// 반동 추가
    /// </summary>
    public void AddRecoil(float upRecoil, float sideRecoil, float duration)
    {
        if (_cameraRotate == null) return;

        // 상하 반동 (위로 올라가도록)
        float targetY = _cameraRotate.AccumulationY - upRecoil;
        DOTween.To(
            () => _cameraRotate.AccumulationY,
            x => _cameraRotate.AccumulationY = x,
            targetY,
            duration
        );

        // 좌우 반동 (랜덤하게)
        float randomSide = Random.Range(-sideRecoil, sideRecoil);
        float targetX = _cameraRotate.AccumulationX + randomSide;
        DOTween.To(
            () => _cameraRotate.AccumulationX,
            x => _cameraRotate.AccumulationX = x,
            targetX,
            duration
        );
    }

   
}