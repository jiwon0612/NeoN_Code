using UnityEngine;

namespace Work.JW.Code.Enemies
{
    [CreateAssetMenu(fileName = "Enemy/Data", menuName = "EnemyData", order = 0)]
    public class EnemyDataSO : ScriptableObject
    {
        [Header("Data")]
        public EnemyType type;
        public PoolTypeSO enemyPoolType;
        
        [Header("Stats")]
        public int health;
        public float speed;
        public int damage;
        public float coolTime;
        
        public int score;
        public int exp;
    }
}