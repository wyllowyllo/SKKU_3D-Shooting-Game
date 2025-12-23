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

        // 코인 생성 및 흩뿌리기
        for (int i = 0; i < coinCount; i++)
        {
            GoldCoin coin = GoldFactory.Instance.GetGoldCoin();
            if (coin == null) continue;

            // 흩어질 방향 계산
            Vector3 scatterDirection = CalculateScatterDirection(i, coinCount);

            // 위치 설정 (약간 위에서 시작)
            coin.transform.position = dropPosition + Vector3.up * 0.5f;

            // 코인 초기화
            coin.Initialize(goldPerCoin, scatterDirection, _scatterForce);
        }
    }

    private static int CalculateCoinCount(int goldAmount)
    {
        // 골드 양에 따라 3-5개 코인 생성
        // 5골드 이하 -> 3개
        // 10골드 -> 3-4개
        // 50골드 이상 -> 5개
        int count = Mathf.Clamp(goldAmount / 2, 3, 5);
        return count;
    }

    private static Vector3 CalculateScatterDirection(int index, int total)
    {
        // 원형 패턴으로 균등하게 분배
        float baseAngle = (360f / total) * index;

        // 약간의 랜덤성 추가
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
