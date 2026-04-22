using System;
using System.Collections.Generic;
using UnityEngine;

namespace Work.JW.Code.Enemies.EnemySpawners
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "SO/Enemy/SpawnWave", order = 0)]
    public class EnemyWaveDataSO : ScriptableObject
    {
        [SerializeField] private List<Wave> waves;
        
        public int Count => waves.Count;
        public Wave this[int idx] => waves[idx];
    }

    [Serializable]
    public struct Wave
    {
        public List<WavePiece> pieces; //한 웨이브에 나오는 적의 순서
        public float time;             // 다음 웨이브 까지의 시간
        
    }
    
    [Serializable]
    public struct WavePiece
    {
        public EnemyType spawnEnemyType; //소환할 적 type
        public int count;                //소환할 수
        public float cooltime;           //다음 소환 까지의 시간
    }
}