using UnityEngine;

public class RankingUI : MonoBehaviour
{
    [SerializeField] private PoolTypeSO poolingItem;
    [SerializeField] private PoolManagerSO pool;
    [SerializeField] private Transform contents;

    private void SetRankUI()
    {
        RankingBoardColum boardColum = pool.Pop(poolingItem) as RankingBoardColum;
    }
}
