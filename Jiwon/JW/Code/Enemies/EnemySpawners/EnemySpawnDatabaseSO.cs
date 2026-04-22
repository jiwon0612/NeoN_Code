using System.Collections.Generic;
using UnityEngine;

namespace Work.JW.Code.Enemies.EnemySpawners
{
    [CreateAssetMenu(fileName = "EnemySpawnDatabase", menuName = "SO/Enemy/Database", order = 0)]
    public class EnemySpawnDatabaseSO : ScriptableObject
    {
        public List<EnemyDataSO> enemyList;

        public int Count => enemyList.Count;
        public EnemyDataSO this[int idx] => enemyList[idx];
    }
}