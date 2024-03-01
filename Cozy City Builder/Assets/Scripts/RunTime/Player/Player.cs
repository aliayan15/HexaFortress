using Managers;
using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private LayerMask gridPosLayer;
        [SerializeField] private GameObject pfUnwalkable;
        [SerializeField] private SOGameProperties gameData;
        [Header("Settings")]
        [SerializeField] private float tileMoveSpeed = 10.0f;
        [Space(10)]
        [Header("Debug")]
        [SerializeField] private bool isDebug;
        [SerializeField] private SOTileData[] tilesToPlace;

        public int MyGold { get; private set; } = 100;
        public int TileCount { get; set; } = 4;
        public UnityAction OnTilePlaced;

        private SOTileData _currentTile;
        private Transform _tileToBuild;
        private TileBase _tileBase;
        private bool _isBuildMode = false;
        private bool _canBuild = false;
        private int _placedTileCount = 0;

        private void Awake()
        {
            GameManager.Instance.player = this;
        }

        private void Start()
        {
            GameManager.Instance.SetState(GameStates.GAME);
        }

        private void Update()
        {
            if (isDebug)
                DebugGame();

            if (!_canBuild) return;
            if (!_isBuildMode) return;

            UpdatePosition();
            // place tile
            if (Input.GetMouseButtonDown(0))
            {
                if (RaycastTile(out HexGridNode grid))
                {
                    if (!grid.CanBuildHere) { CantBuildHere(); return; }
                    if (!_tileBase.CanBuildHere(grid)) { CantBuildHere(); return; }

                    PlaceTile(grid);
                }
            }
            // rotate
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f)
                RotateTile(_tileBase);
            else if (scroll < 0f)
                RotateTile(_tileBase, true);
            // cancel placing
            if (Input.GetMouseButtonDown(1))
            {
                _isBuildMode = false;
                Destroy(_tileToBuild.gameObject);
                _tileBase = null;
            }
        }

        private void DebugGame()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (RaycastTile(out HexGridNode grid))
                {
                    if (grid.MyTile is not PathTile) return;
                    List<Vector3> pathList = GridManager.Instance.PathFinding.FindPath(grid.Position,
                        GridManager.Instance.PlayerCastle.MyHexNode.Position);
                    if (pathList == null) return;

                    for (int i = 0; i < pathList.Count - 1; i++)
                    {
                        pathList[i].Set(pathList[i].x, pathList[i].y + 0.1f, pathList[i].z);
                        Debug.DrawLine(pathList[i], pathList[i + 1], Color.green, 3f);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && tilesToPlace.Length > 0)
                EnterBuildMode(tilesToPlace[0]);
            if (Input.GetKeyDown(KeyCode.Alpha2) && tilesToPlace.Length > 1)
                EnterBuildMode(tilesToPlace[1]);
            if (Input.GetKeyDown(KeyCode.Alpha3) && tilesToPlace.Length > 2)
                EnterBuildMode(tilesToPlace[2]);
            if (Input.GetKeyDown(KeyCode.Alpha4) && tilesToPlace.Length > 3)
                EnterBuildMode(tilesToPlace[3]);
            if (Input.GetKeyDown(KeyCode.Alpha5) && tilesToPlace.Length > 4)
                EnterBuildMode(tilesToPlace[4]);

        }

        #region Building
        private void PlaceTile(HexGridNode grid)
        {
            _isBuildMode = false;
            _tileToBuild.transform.position = grid.Position;
            _tileBase.Init(grid);
            OnTilePlaced?.Invoke();
            TileManager.Instance.AddNewTile(_tileBase.MyType);
            _placedTileCount++;
            CheckPlacedTileCount();
        }

        private void UpdatePosition()
        {
            Vector3 pos = GetTargetPosition();
            _tileToBuild.position = Vector3.Lerp(_tileToBuild.position, pos, tileMoveSpeed * Time.deltaTime);
        }

        public void EnterBuildMode(SOTileData tile)
        {
            if (!_canBuild) return;
            _currentTile = tile;
            _tileToBuild = Instantiate(_currentTile.Prefab).transform;
            _tileBase = _tileToBuild.GetComponent<TileBase>();
            Vector3 pos = GetTargetPosition();
            _tileToBuild.position = pos;
            _isBuildMode = true;
        }

        private void CantBuildHere()
        {
            Debug.Log("CantBuildHere");
        }

        private Vector3 GetTargetPosition()
        {
            RaycastTile(out RaycastHit hit, out HexGridNode grid);
            Vector3 targetPos = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
            if (grid != null)
            {
                if (grid.CanBuildHere)
                    targetPos = grid.Position;
            }
            return targetPos;
        }
        private void RotateTile(TileBase tile, bool left = false)
        {
            tile.Rotate(left);
        }
        // Check placed tile count for spawn enemy
        private void CheckPlacedTileCount()
        {
            if (_placedTileCount >= TileCount)
            {
                GameManager.Instance.SetTurnState(TurnStates.EnemySpawnStart);
            }
        }
        #endregion

        #region Gold
        public void AddGold(int amount)
        {
            MyGold += amount;
            Debug.Log("My Gold: " + MyGold);
        }
        #endregion

        #region State Change
        private void OnTurnStateChange(TurnStates state)
        {
            _canBuild = state == TurnStates.TurnBegin;
            if (state == TurnStates.TurnEnd)
            {
                this.Timer(0.2f, () => { GameManager.Instance.SetTurnState(TurnStates.TurnBegin); });
            }
        }
        private void OnEnable()
        {
            GameManager.OnTurnStateChange += OnTurnStateChange;
        }
        private void OnDisable()
        {
            GameManager.OnTurnStateChange -= OnTurnStateChange;
        }
        #endregion

        #region Ray
        private bool RaycastTile(out HexGridNode grid)
        {
            grid = null;
            var ray = CameraManager.Instance.MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, gridPosLayer, QueryTriggerInteraction.Collide))
            {
                grid = GridManager.Instance.GetGridNode(hit.point);
                if (grid != null)
                {
                    return true;
                }
            }
            return false;
        }
        private void RaycastTile(out RaycastHit hit, out HexGridNode grid)
        {
            grid = null;
            var ray = CameraManager.Instance.MainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000, gridPosLayer, QueryTriggerInteraction.Collide))
            {
                grid = GridManager.Instance.GetGridNode(hit.point);
            }
        }
        #endregion
    }
}
