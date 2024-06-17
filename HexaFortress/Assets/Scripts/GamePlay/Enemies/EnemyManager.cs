using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HexaFortress.Game;
using MyUtilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace HexaFortress.GamePlay
{
    public class EnemyManager : SingletonMono<EnemyManager>
    {
        [FormerlySerializedAs("tier1")]
        [Header("Enemies")]
        [SerializeField] private EnemyConfig[] tier1Enemies;
        [FormerlySerializedAs("tier2")] 
        [SerializeField] private EnemyConfig[] tier2Enemies;
        [Header("Bosses")]
        [SerializeField] private EnemyConfig boss1;
        [SerializeField] private EnemyConfig boss2;
        [FormerlySerializedAs("tier2Day")]
        [Space(5)]
        [SerializeField] private int tier2ActivationDay;
        [FormerlySerializedAs("tier3ActivationDay")]
        [FormerlySerializedAs("tier3Day")] 
        [SerializeField] private int tier2BossDay;
        [Space(10)]
        [SerializeField] private float spawnPointY;

        public float EnemyPosY => spawnPointY;

        private bool _debug = false;
        private bool _isSpawning;
        private Vector3 _currentSpawnPosition;
        private int _currentPathIndex;
        private Dictionary<int, List<Vector3>> _paths = new Dictionary<int, List<Vector3>>();
        private List<EnemyController> _enemyList = new List<EnemyController>();
        private EnemyFactory _enemyFactory = new();


        private void StartSpawn()
        {
            if (_isSpawning) return;
            _isSpawning = true;
            StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            if (TileManager.Instance.EnemySpawnPoints.Count == 0) yield break;

            var waitTime = new WaitForSeconds(1f);
            int enemyCount = GameModel.Instance.PlayerData.DayCount * GameModel.Instance.PlayerData.DayCount;
            
            InitializePaths();

            while (enemyCount > 0)
            {
                foreach (var spawnPoint in TileManager.Instance.EnemySpawnPoints)
                {
                    yield return waitTime;

                    _currentSpawnPosition = spawnPoint;
                    _currentSpawnPosition.y = spawnPointY;
                    _currentPathIndex = TileManager.Instance.EnemySpawnPoints.IndexOf(spawnPoint);

                    enemyCount -= SpawnEnemiesBasedOnTiers();
                    if (enemyCount <= 0)
                        break;
                }
            }

            SpawnBoss();
            _isSpawning = false;
            StartCoroutine(CheckAllEnemiesDead());
        }

        private void InitializePaths()
        {
            _paths.Clear();
            foreach (var (index, spawnPoint) in TileManager.Instance.EnemySpawnPoints.WithIndex())
            {
                var gridNode = GridManager.Instance.GetGridNode(spawnPoint);
                _paths[index] = HexPathFinding.Instance.FindPath(gridNode.Position, GameModel.Instance.CastleTile.MyHexNode.Position);
            }
        }

        private int SpawnEnemiesBasedOnTiers()
        {
            if (GameModel.Instance.PlayerData.DayCount >= tier2ActivationDay)
            {
                int numSpawned = SpawnEnemiesFromArray(tier2Enemies, 2f);
                if (numSpawned > 0) return numSpawned;
            }
            return SpawnEnemiesFromArray(tier1Enemies, 1f);
        }

        private int SpawnEnemiesFromArray(EnemyConfig[] enemies, float spawnChanceMultiplier)
        {
            foreach (var enemy in enemies.Reverse())
            {
                bool canSpawn = enemy.Level <= GameModel.Instance.PlayerData.DayCount &&
                                Random.Range(0.0f, 1.0f) < (spawnChanceMultiplier / enemy.Level);

                if (canSpawn)
                {
                    SpawnEnemy(enemy);
                    return enemy.Level;
                }
            }
            return 0;
        }

        private void SpawnEnemy(EnemyConfig config)
        {
            EnemyController newEnemy = _enemyFactory.Create(config);
            newEnemy.transform.position = _currentSpawnPosition;
            newEnemy.SetMovePosition(_paths[_currentPathIndex]);
            
            // Spawn animation
            Vector3 originalScale = newEnemy.transform.localScale;
            newEnemy.transform.localScale = Vector3.zero;
            newEnemy.transform.DOScale(originalScale, 0.2f);
            
            _enemyList.Add(newEnemy);
        }

        private void SpawnBoss()
        {
            if (GameModel.Instance.PlayerData.DayCount == tier2ActivationDay - 1)
            {
                SpawnEnemy(boss1);
            }
            else if (GameModel.Instance.PlayerData.DayCount == tier2BossDay - 1)
            {
                SpawnEnemy(boss2);
            }
        }

        private IEnumerator CheckAllEnemiesDead()
        {
            var waitTime = new WaitForSeconds(0.5f);

            while (_enemyList.Count > 0)
            {
                yield return waitTime;
                _enemyList.RemoveAll(enemy => enemy == null);
            }

            OnAllEnemiesDead();
        }

        private void OnAllEnemiesDead()
        {
            GameManager.Instance.SetTurnState(TurnStates.TurnEnd);
        }

        private void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            if (evt.TurnState == TurnStates.EnemySpawnStart)
            {
                StartSpawn();
            }
        }

        private void OnGameStateChange(GameStateChangeEvent evt)
        {
            if (evt.GameState == GameStates.GAMEOVER)
            {
                StopAllCoroutines();
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener<TurnStateChangeEvent>(OnTurnStateChange);
            EventManager.AddListener<GameStateChangeEvent>(OnGameStateChange);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<TurnStateChangeEvent>(OnTurnStateChange);
            EventManager.RemoveListener<GameStateChangeEvent>(OnGameStateChange);
        }

        private void OnDrawGizmosSelected()
        {
            if (!_debug) return;

            foreach (var spawnPoint in TileManager.Instance.EnemySpawnPoints)
            {
                Gizmos.DrawSphere(spawnPoint, 0.3f);
            }
        }
    }

    public static class ExtensionMethods
    {
        public static IEnumerable<(int, T)> WithIndex<T>(this IEnumerable<T> self)
        {
            return self.Select((item, index) => (index, item));
        }
    }
    
}

