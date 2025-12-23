using Redcode.Pools;
using UnityEngine;

public class GoldFactory : MonoBehaviour
{
    private static GoldFactory _instance;
    private PoolManager _poolManager;

    public static GoldFactory Instance
    {
        get => _instance;
        private set => _instance = value;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _poolManager = GetComponent<PoolManager>();
    }

    public GoldCoin GetGoldCoin()
    {
        if (_poolManager == null)
        {
            Debug.LogError("GoldFactory: PoolManager is not assigned!");
            return null;
        }

        GoldCoin coin = _poolManager.GetFromPool<GoldCoin>();
        return coin;
    }

    public void ReturnGoldCoin(GoldCoin coin)
    {
        if (_poolManager == null || coin == null) return;

        _poolManager.TakeToPool<GoldCoin>(coin);
    }
}
