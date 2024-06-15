using HexaFortress.Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace HexaFortress.GamePlay
{
    public class StartTilesPlacer:MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private CastleTile castleTile;
        [SerializeField] private PathTile pathTile;
        [SerializeField] private Transform floor;

        private void Awake()
        {
            GetComponent<GridManager>().OnGridCreated += OnGridCreated;
        }

        private void OnGridCreated()
        {
            CreateStartupTiles();
        }

        private void CreateStartupTiles()
        {
            // create castle
            int coordX = Mathf.FloorToInt(GridManager.Instance.GridSize.x / 2);
            int coordY = Mathf.FloorToInt(GridManager.Instance.GridSize.y / 2);
            var castleGrid = GridManager.Instance.GetGridNode(coordX, coordY);
            CastleTile castle = Instantiate(castleTile, castleGrid.Position, Quaternion.identity);
            castle.gameObject.name = "Player Castle";
            castle.Init(castleGrid);
            CameraManager.Instance.TeleportPosition(castleGrid.Position);
            GameModel.Instance.CastleTile = castle;

            // create one path
            var pathNode = GridManager.Instance.GetGridNode(castle.PathPoint.position);
            PathTile path  = Instantiate(pathTile, pathNode.Position, Quaternion.identity);
            path.Init(pathNode);
            path.RemoveSpawnPointNearCastle();
            floor.transform.position = castleGrid.Position;
        }

        
    }
}