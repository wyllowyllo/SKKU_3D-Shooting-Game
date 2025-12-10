using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStat", menuName = "WeaponSO/CreateWeaponData")]
public class WeaponStat : ScriptableObject
{
    [Header("기본 총 스텟")]
    [SerializeField] private float _shotInterval = 0.1f;
    [SerializeField] private int _remainBulletCnt;
    [SerializeField] private int _maxBulletCnt;
    [SerializeField] private int _magazineCnt;
    
    [Header("재장전 속도")]
    [SerializeField] private float _reloadTime;

    public float ShotInterval => _shotInterval;

    public int RemainBulletCnt => _remainBulletCnt;

    public int MaxBulletCnt => _maxBulletCnt;

    public int MagazineCnt => _magazineCnt;

    public float ReloadTime => _reloadTime;
}
