using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HexaFortress.Game;
using MyUtilities;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class EnemySpawner : SingletonMono<EnemySpawner>
    {
        [Header("Enemies")]
        [SerializeField] private Enemy[] tier1;
        [SerializeField] private Enemy[] tier2;
        //[SerializeField] private Enemy[] tier3;
        [Header("Bosses")]
        [SerializeField] private Enemy boss1;
        [SerializeField] private Enemy boss2;
        //[SerializeField] private Enemy boss3;
        [Space(5)]
        [SerializeField] private int tier2Day;
        [SerializeField] private int tier3Day;
        [SerializeField] private int gameEndDay;
        [Space(10)]
        [SerializeField] private float spawnPointY;

        public float EnemyPosY => spawnPointY;
        public int GameEndDay => gameEndDay;

        private bool isDebug = false;
        private bool _isSpawning;
        private Vector3 _lastSpawnPos;
        private int _pathIndex;
        private Dictionary<int, List<Vector3>> _paths = new Dictionary<int, List<Vector3>>();
        private List<Enemy> _enemyList = new List<Enemy>();


        #region Spawn
        private void StartSpawn()
        {
            if (_isSpawning) return;
            _isSpawning = true;
            StartCoroutine(SpawnLoop());
        }
        private IEnumerator SpawnLoop()
        {
            if (TileManager.Instance.EnemySpawnPoints.Count == 0) yield break;
            // total count(not)
            var time = new WaitForSeconds(1f);
            int count = GameModel.Instance.PlayerData.DayCount * GameModel.Instance.PlayerData.DayCount;
            // set path for each spawn point
            _paths.Clear();
            for (int i = 0; i < TileManager.Instance.EnemySpawnPoints.Count; i++)
            {
                var grid = GridManager.Instance.GetGridNode(TileManager.Instance.EnemySpawnPoints[i]);
                _paths[i] = GridManager.Instance.PathFinding.FindPath(grid.Position,
                    GameModel.Instance.CastleTile.MyHexNode.Position);
            }
            // spawn enemies
            while (count > 0)
            {
                for (int i = 0; i < TileManager.Instance.EnemySpawnPoints.Count; i++)
                {
                    yield return time;
                    _pathIndex = i;
                    _lastSpawnPos = TileManager.Instance.EnemySpawnPoints[i];

                    count -= SpawnFromTiers();
                    if (count <= 0)
                        break;
                }

            }
            SpawnBoss();
            _isSpawning = false;
            StartCoroutine(CheckAllEnemiesDead());
        }

        private int SpawnFromTiers()
        {
            //if (GameManager.Instance.DayCount >= tier3Day)
            //{
            //    int num = SpawnFromArray(tier3, 3f);
            //    if (num > 0)
            //        return num;
            //}
            if (GameModel.Instance.PlayerData.DayCount >= tier2Day)
            {
                int num = SpawnFromArray(tier2, 2f);
                if (num > 0)
                    return num;
            }
            int num1 = SpawnFromArray(tier1, 1f);
            return num1;
        }
        private int SpawnFromArray(Enemy[] enemies, float multiplier)
        {
            int num = 0;
            float rndPlus = 0f;
            for (int i = enemies.Length - 1; i >= 0; --i)
            {
                bool canSpawnThisEnemy = enemies[i].Level <= GameModel.Instance.PlayerData.DayCount
                                         && UnityEngine.Random.Range(0.0f, 1.0f) < (multiplier / enemies[i].Level) + rndPlus;
                if (canSpawnThisEnemy)
                {
                    SpawnEnemy(enemies[i]);
                    num = enemies[i].Level;
                    break;
                }
                else
                    rndPlus += 0.02f;
            }
            return num;
        }
        private void SpawnEnemy(Enemy enemy)
        {
            _lastSpawnPos.y = spawnPointY;
            Enemy newEnemy = Instantiate(enemy, _lastSpawnPos, Quaternion.identity);
            newEnemy.SetMovePosition(_paths[_pathIndex]);
            Vector3 scale = newEnemy.transform.localScale;
            newEnemy.transform.localScale = Vector3.zero;
            newEnemy.transform.DOScale(scale, 0.2f);
            _enemyList.Add(newEnemy);
        }
        private void SpawnBoss()
        {
            if (GameModel.Instance.PlayerData.DayCount == tier2Day - 1)
                SpawnEnemy(boss1);
            else if (GameModel.Instance.PlayerData.DayCount == tier3Day - 1)
                SpawnEnemy(boss2);

        }
        #endregion

        private IEnumerator CheckAllEnemiesDead()
        {
            var time = new WaitForSeconds(0.5f);
            while (_enemyList.Count > 0)
            {
                yield return time;
                for (int i = 0; i < _enemyList.Count; i++)
                {
                    if (_enemyList[i] == null)
                    {
                        _enemyList.RemoveAt(i);
                        i--;
                    }
                }
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
            // when game over stop spawning
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

        private void OnDrawGizmos()
        {
            if (!isDebug) return;
            foreach (var point in TileManager.Instance.EnemySpawnPoints)
            {
                Gizmos.DrawSphere(point, 0.3f);
            }
        }
    }
}

