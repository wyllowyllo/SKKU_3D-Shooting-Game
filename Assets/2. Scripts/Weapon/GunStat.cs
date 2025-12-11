using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStat", menuName = "WeaponSO/CreateWeaponData")]
public class GunStat : ScriptableObject
{
    [Header("기본 총 스텟")]
    [SerializeField] private float _shotInterval = 0.1f;
    [SerializeField] private int _bulletCntForAmmo;
    [SerializeField] private int _ammoCnt = 5;
    
    [Header("재장전 속도")]
    [SerializeField] private float _reloadTime;
    
    [Header("반동")]
    [SerializeField] private float _upRecoilStrength = 3f;
    [SerializeField] private float _sideRecoilStrength = 1.2f;
    [SerializeField] private float _recoilDuration = 0.1f;
    
    public float ShotInterval => _shotInterval;

    public int BulletCntForAmmo => _bulletCntForAmmo;

    public int AmmoCnt => _ammoCnt;

    public float ReloadTime => _reloadTime;

    public float UpRecoilStrength => _upRecoilStrength;

    public float SideRecoilStrength => _sideRecoilStrength;

    public float RecoilDuration => _recoilDuration;
}
