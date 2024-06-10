using HexaFortress.Game;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class StartTilesPlacer:MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private SOGameProperties gameData;
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
            var castleObj = Instantiate(gameData.Castle, castleGrid.Position, Quaternion.identity);
            CastleTile castle = castleObj.GetComponent<CastleTile>();
            castleObj.name = "Player Castle";
            castle.Init(castleGrid);
            CameraManager.Instance.TeleportPosition(castleGrid.Position);
            GameModel.Instance.CastleTile = castle;

            // create one path
            var pathNode = GridManager.Instance.GetGridNode(castle.PathPoint.position);
            var pathObj = Instantiate(gameData.PathTile, pathNode.Position, Quaternion.identity);
            PathTile path = pathObj.GetComponent<PathTile>();
            path.Init(pathNode);
            path.RemoveSpawnPointNearCastle();
            floor.transform.position = castleGrid.Position;
        }

        
    }
}