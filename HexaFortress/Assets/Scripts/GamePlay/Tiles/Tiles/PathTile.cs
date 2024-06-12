using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class PathTile : TileBase
    {
        [Header("Settings")]
        [SerializeField] private Transform[] connectionPoints;
        [SerializeField] private Transform[] spawnPoints;


        public bool IsSpawnPoint { get; private set; } = false;
        public Transform[] ConnectionPoints => connectionPoints;

        public override void Init(HexGridNode myNode)
        {
            base.Init(myNode);
            myNode.SetIsWalkable(true);
            CheckSpawnPoints();
        }

        private void CheckSpawnPoints()
        {
            foreach (Transform t in spawnPoints)
            {
                TileManager.Instance.AddEnemySpawnPoint(t.position);
            }
            var neighbourGrids = GridManager.Instance.GetSurroundingGrids(_myHexNode);
            foreach (var item in neighbourGrids)
            {
                if (!item.MyTile) continue;
                if (item.MyTile.MyType != TileType.Path) continue;

                PathTile pathTile = item.MyTile as PathTile;
                // check surroundingGrid have connection point to this tile
                for (int i = 0; i < pathTile.connectionPoints.Length; i++)
                {
                    if (_myHexNode != GridManager.Instance.GetGridNode(pathTile.connectionPoints[i].position)) continue;

                    // check this tile have connection point to surroundingGrid
                    for (int j = 0; j < connectionPoints.Length; j++)
                    {
                        if (pathTile.MyHexNode != GridManager.Instance.GetGridNode(connectionPoints[j].position)) continue;

                        // connection point
                        // remove this spawn point
                        RemoveSpawnPoint(j);
                        // remove pathTile spawn point
                        pathTile.RemoveSpawnPoint(i);

                        if (TileManager.Instance.EnemySpawnPoints.Count == 0)
                        {
                            // path connected
                            pathTile.AddSpawnPoint(i);
                        }
                    }
                }

            }
        }

        public void RemoveSpawnPoint(int index)
        {
            TileManager.Instance.RemoveEnemySpawnPoint(spawnPoints[index].position);
        }
        public void AddSpawnPoint(int index)
        {
            TileManager.Instance.AddEnemySpawnPoint(spawnPoints[index].position);
        }

        public void RemoveSpawnPointNearCastle()
        {
            for (int j = 0; j < connectionPoints.Length; j++)
            {
                if (GameModel.Instance.CastleTile.MyHexNode !=
                    GridManager.Instance.GetGridNode(connectionPoints[j].position)) continue;

                // spawn point near castle for start path
                RemoveSpawnPoint(j);
            }
        }

        public override bool CanBuildHere(HexGridNode grid)
        {
            var surroundingGrids = GridManager.Instance.GetSurroundingGrids(grid);
            bool build = false;
            foreach (var item in surroundingGrids)
            {
                if (!item.MyTile) continue;
                if (item.MyTile.MyType == TileType.Castle)
                    return false;
                if (item.MyTile.MyType != TileType.Path) continue;
                PathTile pathTile = item.MyTile as PathTile;
                // check surroundingGrid have connection point to this tile
                foreach (var point in pathTile.connectionPoints)
                {
                    if (grid == GridManager.Instance.GetGridNode(point.position))
                    {
                        // check this tile have connection point to surroundingGrid
                        foreach (var myPoint in connectionPoints)
                        {
                            if (pathTile.MyHexNode == GridManager.Instance.GetGridNode(myPoint.position))
                                build = true;
                        }
                    }
                }
            }
            return build;
        }



        protected override void OnDisable()
        {

        }
        protected override void OnEnable()
        {

        }
    }
}

