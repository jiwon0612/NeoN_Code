using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Work.JW.Code.Core.EventSystems;
using Random = UnityEngine.Random;

namespace Work.JW.Code.Enemies.EnemySpawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO gameChannel;
        [SerializeField] private GameEventChannelSO enemyChannel;
        
        [SerializeField] private PoolManagerSO poolM;
        [SerializeField] private EnemySpawnDatabaseSO enemyDatas;
        [SerializeField] private EnemyWaveDataSO waveData;

        [SerializeField] private Vector2 spawnerSize;
        [SerializeField] private Vector2 spawnerOffset;

        private Dictionary<EnemyType, PoolTypeSO> _enemies;
        private Dictionary<EnemyType, EnemyDataSO> _enemyDatas;

        private Coroutine _waveSpawnCoroutine;
        private Coroutine _infiniteSpawnCoroutine;

        private bool _isStop;

        private int _waveCount;
        private int _currentWaveIdx = 0;
        private int _curWaveEnemyCount;

        [SerializeField] private float infiniteEnemySpawnCoolTime = 8f;


        private void Awake()
        {
            _enemies = new Dictionary<EnemyType, PoolTypeSO>();
            _enemyDatas = new Dictionary<EnemyType, EnemyDataSO>();
            _waveCount = waveData.Count;

            enemyDatas.enemyList.ForEach(data => _enemies.Add(data.type, data.enemyPoolType));
            enemyDatas.enemyList.ForEach(data => _enemyDatas.Add(data.type, data));

            gameChannel.AddListener<BossSpawnEvent>(HandleBossSpawnEvent);
            gameChannel.AddListener<GameStopEvent>(HandleGameStopEvent);
            enemyChannel.AddListener<EnemyDeadEvent>(HandleEnemiesDead);
        }

        private void Start()
        {
            _waveSpawnCoroutine = StartCoroutine(SpawnWave());
        }

        private IEnumerator SpawnWave()
        {
            while (_currentWaveIdx < _waveCount)
            {
                var evt = GameEvents.WaveChangeEvent.Initializer(_currentWaveIdx);
                gameChannel.RaiseEvent(evt);

                Wave wave = waveData[_currentWaveIdx];

                wave.pieces.ForEach(p => _curWaveEnemyCount += p.count);

                foreach (WavePiece piece in wave.pieces)
                {
                    yield return new WaitWhile(() => _isStop);
                    SpawnEnemies(piece);
                
                    yield return new WaitWhile(() => _isStop);
                    yield return new WaitForSeconds(piece.cooltime);
                }
            
                yield return new WaitForSeconds(wave.time);
                _currentWaveIdx++;
            }
        }

        private void SpawnEnemies(WavePiece piece)
        {
            PoolTypeSO enemyPool = _enemies[piece.spawnEnemyType];
            EnemyDataSO data = _enemyDatas[piece.spawnEnemyType];

            for (int i = 0; i < piece.count; i++)
            {
                float sizeMaxX = spawnerOffset.x + (spawnerSize.x / 2f);
                float sizeMinX = spawnerOffset.x - (spawnerSize.x / 2f);
                float sizeMaxY = spawnerOffset.y + (spawnerSize.y / 2f);
                float sizeMinY = spawnerOffset.y - (spawnerSize.y / 2f);

                Vector3 pos = new Vector3(Random.Range(sizeMinX, sizeMaxX), 4, Random.Range(sizeMinY, sizeMaxY));

                Enemy item = poolM.Pop(enemyPool) as Enemy;

                item.GetCompo<EntityMover>().ForceWarpTo(pos);
                item.GetCompo<EnemyDataCompo>().InitData(data);
            }
        }

        private IEnumerator SpawnInfiniteEnemies()
        {
            while (true)
            {
                int wantSpawnCount = Random.Range(1, 5);
                Wave wave = MakeWavePiece(wantSpawnCount);
                
                wave.pieces.ForEach(p => _curWaveEnemyCount += p.count);
                
                foreach (WavePiece piece in wave.pieces)
                {
                    yield return new WaitWhile(() => _isStop);
                    
                    SpawnEnemies(piece);
                    
                    yield return new WaitWhile(() => _isStop);
                    yield return new WaitForSeconds(piece.cooltime);
                }

                yield return new WaitWhile(() => _isStop);
                
                float randSize = Random.Range(0.1f, 0.5f);
                infiniteEnemySpawnCoolTime = Mathf.Clamp(infiniteEnemySpawnCoolTime - randSize, 2f, 8f);
                yield return new WaitForSeconds(infiniteEnemySpawnCoolTime);
            }
        }

        private Wave MakeWavePiece(int wantSpawnTypeCount)
        {
            Wave wave = new Wave();

            wave.pieces = new List<WavePiece>();

            for (int i = 0; i < wantSpawnTypeCount; i++)
            {
                WavePiece piece = new WavePiece();

                int lastEnemyType = (int)EnemyType.Boss;
                piece.spawnEnemyType = (EnemyType)Random.Range(0, lastEnemyType);
                piece.count = Random.Range(1, 5);
                piece.cooltime = Random.Range(0.1f, 4f);

                wave.pieces.Add(piece);
            }

            return wave;
        }

        private void HandleEnemiesDead(EnemyDeadEvent evt)
        {
            _curWaveEnemyCount--;

            if (_curWaveEnemyCount <= 0)
            {
                if(_currentWaveIdx < _waveCount)
                {
                    _currentWaveIdx++;
                    NextWaveSpawnCoroutine(ref _waveSpawnCoroutine, SpawnWave());
                }
                else
                {
                    if (_waveSpawnCoroutine != null)
                        StopCoroutine(_waveSpawnCoroutine);
                    NextWaveSpawnCoroutine(ref _infiniteSpawnCoroutine, SpawnInfiniteEnemies());
                }
            }
        }

        private void NextWaveSpawnCoroutine(ref Coroutine curSpawnCoroutine, IEnumerator spawnMethod)
        {
            if (curSpawnCoroutine != null)
                StopCoroutine(curSpawnCoroutine);
            
            curSpawnCoroutine = StartCoroutine(spawnMethod);
        }

        private void HandleBossSpawnEvent(BossSpawnEvent evt)
        {
            evt.boss.OnDeadEvent.AddListener(HandleBossDeathEvent);
        }

        private void HandleBossDeathEvent()
        {
            var evt = GameEvents.WaveChangeEvent.Initializer(-1);
            gameChannel.RaiseEvent(evt);
            
            if (_waveSpawnCoroutine != null)
                StopCoroutine(_waveSpawnCoroutine);
            _infiniteSpawnCoroutine = StartCoroutine(SpawnInfiniteEnemies());
        }

        private void HandleGameStopEvent(GameStopEvent evt)
        {
            _isStop = evt.isStop;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            float x = spawnerSize.x;
            float y = spawnerSize.y;
            Vector3 center = new Vector3(spawnerOffset.x, 0, spawnerOffset.y);

            Gizmos.DrawWireCube(center, new Vector3(x, 1, y));
        }
    }
}