using System;
using Redcode.Pools;
using UnityEngine;


public class BulletFactory : MonoBehaviour
{
    private static BulletFactory _instance;
    private PoolManager _poolManager;
    public static BulletFactory Instance
    {
        get => _instance;
        private set => _instance = value;
    }


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
        
        _poolManager = GetComponent<PoolManager>();
    }

    public Bomb GetBullet()
    {
        Bomb bomb = _poolManager.GetFromPool<Bomb>();
        return bomb;
    } 

    public void ReturnBullet(Bomb bomb)
    {
        _poolManager.TakeToPool<Bomb>(bomb);
    }
}
