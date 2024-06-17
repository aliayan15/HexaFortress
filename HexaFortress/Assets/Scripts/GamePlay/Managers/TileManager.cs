using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HexaFortress.Game;
using MyUtilities;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class TileManager : SingletonMono<TileManager>
    {
        public Dictionary<TileType, int> TileCount = new Dictionary<TileType, int>();
        [HideInInspector]
        public List<Vector3> EnemySpawnPoints = new List<Vector3>();

        [SerializeField] private GameObject goldIconPref;

        private HashSet<TileBase> _economyTiles = new HashSet<TileBase>();
        private WaitForSeconds _animDelay = new WaitForSeconds(1.3f);

        #region Enemy Spawn Points
        public void AddEnemySpawnPoint(Vector3 spawnPoint)
        {
            if (!EnemySpawnPoints.Contains(spawnPoint))
                EnemySpawnPoints.Add(spawnPoint);
        }
        public void RemoveEnemySpawnPoint(Vector3 spawnPoint)
        {
            if (EnemySpawnPoints.Contains(spawnPoint))
                EnemySpawnPoints.Remove(spawnPoint);
        }

        #endregion

        #region Economy
        public void AddEconomyTile(TileBase tile)
        {
            _economyTiles.Add(tile);
        }

        private IEnumerator EconomyTileProdection()
        {
            yield return new WaitForSeconds(1f);
            var time = new WaitForSeconds(0.2f);
            var tiles = _economyTiles.ToList();
            foreach (var tile in tiles)
            {
                // create gold icon
                StartCoroutine(CreateGoldIceon(tile.transform));
                yield return time;
            }
        }
        private IEnumerator CreateGoldIceon(Transform tile)
        {
            var goldIcon = Instantiate(goldIconPref, tile.transform.position, Quaternion.identity);
            goldIcon.transform.LookAt(goldIcon.transform.position + CameraManager.Instance.CamPosition.rotation * Vector3.forward,
                CameraManager.Instance.CamPosition.rotation * Vector3.up);
            float scale = goldIcon.transform.localScale.x;
            goldIcon.transform.localScale = Vector3.zero;
            goldIcon.transform.DOMoveY(tile.transform.position.y + 2.5f, 0.3f);
            goldIcon.transform.DOScale(scale, 0.3f);
            AudioManager.Instance.Play2DSound(SoundTypes.GoldProduce);
            yield return _animDelay;
            goldIcon.transform.DOScale(0, 0.2f).OnComplete(() =>
            {
                Destroy(goldIcon);
            });
        }
        #endregion

        public void AddNewTile(TileType type)
        {
            if (TileCount.ContainsKey(type))
                TileCount[type] = TileCount[type] + 1;
            else
                TileCount.Add(type, 1);
        }
        public void RemoveNewTile(TileType type)
        {
            if (TileCount.ContainsKey(type))
                TileCount[type] = TileCount[type] - 1;
        }

        public int GetTileCount(TileType type)
        {
            if (TileCount.ContainsKey(type))
                return TileCount[type];
            else return 0;
        }

        public int GetPriceOfTile(TileType type, int basePrice, int priceIncrease)
        {
            int count = GetTileCount(type);
            switch (type)
            {
                case TileType.Grass:
                case TileType.Fielts:
                case TileType.House:
                case TileType.TowerUpgrade:
                    count = Mathf.Min(count, 6);
                    break;

                default:
                    break;
            }
            return basePrice + (count * priceIncrease);
        }


        private void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            if (evt.TurnState == TurnStates.TurnBegin && GameModel.Instance.PlayerData.DayCount != 1)
                StartCoroutine(EconomyTileProdection());
        }

        private void OnEnable()
        {
            EventManager.AddListener<TurnStateChangeEvent>(OnTurnStateChange);
        }
        private void OnDisable()
        {
            EventManager.RemoveListener<TurnStateChangeEvent>(OnTurnStateChange);
        }

        private void OnDrawGizmos()
        {
            if (EnemySpawnPoints.Count > 0)
            {
                foreach (var item in EnemySpawnPoints)
                {
                    Gizmos.DrawSphere(item, 0.5f);
                }
            }

        }
    }
}

