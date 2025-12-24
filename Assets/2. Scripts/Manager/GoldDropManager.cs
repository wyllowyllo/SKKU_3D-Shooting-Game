using UnityEngine;

public static class GoldDropManager
{
    private static float _scatterForce = 4f;

    public static void DropGold(Vector3 dropPosition, int goldAmount)
    {
        if (goldAmount <= 0) return;

        if (GoldFactory.Instance == null)
        {
            Debug.LogError("GoldDropManager: GoldFactory instance not found!");
            return;
        }

        // 골드 양에 따라 코인 개수 계산 (3-5개)
        int coinCount = CalculateCoinCount(goldAmount);
        int goldPerCoin = Mathf.CeilToInt((float)goldAmount / coinCount);

      
        for (int i = 0; i < coinCount; i++)
        {
            GoldCoin coin = GoldFactory.Instance.GetGoldCoin();
            if (coin == null) continue;
            
            Vector3 scatterDirection = CalculateScatterDirection(i, coinCount);
            coin.transform.position = dropPosition + Vector3.up * 0.5f;
            coin.Initialize(goldPerCoin, scatterDirection, _scatterForce);
        }
    }

    private static int CalculateCoinCount(int goldAmount)
    {
        
        int count = Mathf.Clamp(goldAmount / 2, 3, 5);
        return count;
    }

    private static Vector3 CalculateScatterDirection(int index, int total)
    {
        // 원형 패턴으로 균등하게 분배
        float baseAngle = (360f / total) * index;

       
        float randomOffset = Random.Range(-20f, 20f);
        float angle = baseAngle + randomOffset;
        float angleRad = angle * Mathf.Deg2Rad;

        // 수평 방향 벡터
        Vector3 horizontal = new Vector3(Mathf.Cos(angleRad), 0f, Mathf.Sin(angleRad));

        // 위쪽으로도 힘을 줘서 포물선 궤적 생성
        Vector3 upward = Vector3.up * Random.Range(0.5f, 1.2f);

        return (horizontal + upward).normalized;
    }
}
