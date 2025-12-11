using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] private GunStat _gunStat;

    [Header("Hit VFX")]
    [SerializeField] private ParticleSystem _hitEffect;
    
    // 이벤트
    private UnityEvent _onReload;
    private UnityEvent _onFire;
    
    // 참조
    private Camera _cam;
    private CameraRotate _camRotate;
    
    // 현재 총 상태
    private int _remainBullets;
    private int _bulletCntForAmmo;
    private int _totalBulletCnt;
    private float _reloadDuration;
    
    // 플래그 변수
    private bool _isReloading;
    
    // 타이머
    private float _shotTimer = 0f;

    // 프로퍼티
    public UnityEvent OnReload => _onReload;
    public UnityEvent OnFire => _onFire;
    public int RemainBullets => _remainBullets;
    public int TotalBulletCnt => _totalBulletCnt;
    public float ReloadDuration => _reloadDuration;

   

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        _shotTimer += Time.deltaTime;
    }
    
    public void Fire()
    {
        if (_shotTimer < _gunStat.ShotInterval) return;
        if (_isReloading) return;
        if (RemainBullets <= 0)
        {
            Reload();
            _shotTimer = 0f;
            return;
        }
        
        
       ShootRay();
       
       _camRotate?.AddRecoil(_gunStat.UpRecoilStrength, _gunStat.SideRecoilStrength, _gunStat.RecoilDuration);
       _onFire?.Invoke();
       
        _remainBullets--;
        _shotTimer = 0f;
    }
    public void Reload()
    {
        if (_isReloading) return;
        if (_remainBullets == _bulletCntForAmmo) return;
        
        int loadBulletCnt = _bulletCntForAmmo - _remainBullets;
        if (loadBulletCnt <= _totalBulletCnt)
        {
            StartCoroutine(ReloadCoroutine(loadBulletCnt));
        }
    }

    private void ShootRay()
    {
        Ray ray = new Ray(_cam.transform.position, Camera.main.transform.forward);
        
        RaycastHit hitInfo = new RaycastHit();
        
        bool isHit =  Physics.Raycast(ray, out hitInfo);
        if (isHit)
        {
            PlayHitEffect(hitInfo);  
        }
    }

    private void PlayHitEffect(RaycastHit hitInfo)
    {
        // 파티클 생성과 플레이 방식
        // 1. Instantiate 방식 ( + 풀링) -> 새로 생성(메모리, cpu) -> 한 화면에 여러가지 수정 후 여러 개 그릴 경우 주로 사용
            
        // 2. 하나를 캐싱해두고 Play ->단점 : 재실행이므로 기존 것이 삭제 -> 인스펙터 설정 그대로 그릴 경우
        Debug.Log("Hit " + hitInfo.transform.name);

        if (_hitEffect != null)
        {
            _hitEffect.transform.position = hitInfo.point;
            _hitEffect.transform.forward = hitInfo.normal;
            _hitEffect.Play(true);
        }
            
        // 3. 하나를 캐싱해두고 Emit -> 인스펙터 설정을 수정 후 그릴 경우
        // 파티클을 어떻게 분출할지 정보를 넘겨줌
        /*ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.position = hitInfo.point;
        emitParams.rotation3D = Quaternion.LookRotation(hitInfo.normal).eulerAngles;
        _hitEffect.Emit(emitParams, 1);*/
    }

    private IEnumerator ReloadCoroutine(int loadBulletCnt)
    {
        _isReloading = true;
        _onReload?.Invoke();
        
        yield return new WaitForSeconds(ReloadDuration);
        
        _totalBulletCnt -= loadBulletCnt;
        _remainBullets = _bulletCntForAmmo; // 재장전
        
        _isReloading = false;
    }

    private void Init()
    {
        _cam = Camera.main;
        _camRotate = _cam?.GetComponent<CameraRotate>();
        
        _bulletCntForAmmo = _gunStat.BulletCntForAmmo;
        _remainBullets = _bulletCntForAmmo;
        _totalBulletCnt = _gunStat.AmmoCnt *_bulletCntForAmmo;
        _reloadDuration = _gunStat.ReloadTime;
        
        _onReload = new UnityEvent();
        _onFire = new UnityEvent();
        
    }
}
