using System;
using Redcode.Pools;
using UnityEngine;


public class BombFactory : MonoBehaviour
{
    private static BombFactory _instance;
    private PoolManager _poolManager;
    public static BombFactory Instance
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

    public Bomb GetBomb()
    {
        Bomb bomb = _poolManager.GetFromPool<Bomb>();
        return bomb;
    } 

    public void ReturnBomb(Bomb bomb)
    {
        _poolManager.TakeToPool<Bomb>(bomb);
    }
}
