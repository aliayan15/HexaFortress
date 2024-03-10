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
        [SerializeField] private const short tileCountPerDay = 4;
        [Space(10)]
        [Header("Debug")]
        [SerializeField] private bool isDebug;
        [SerializeField] private SOTileData[] tilesToPlace;

        [field: SerializeField]
        public int MyGold { get; private set; }
        public int GoldPerDay { get; private set; } = 0;
        public int RemainingTileCount { get; set; }
        public UnityAction OnTilePlaced;

        private SOTileData _currentTile;
        private Transform _tileToBuild;
        private TileBase _tileBase;
        private bool _isBuildMode = false;
        private bool _canBuild = false;
        private short _placedTileCount = 0;
        private HexGridNode _lastInsertedGrid = null;

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
                CanselSelection();
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
        private void CanselSelection()
        {
            _isBuildMode = false;
            Destroy(_tileToBuild.gameObject);
            _tileBase.OnPlayerHand(false);
            _tileBase = null;
            ToolTipSystem.Instance.CanShowUI = true;
        }
        private void PlaceTile(HexGridNode grid)
        {
            _isBuildMode = false;
            _tileToBuild.transform.position = grid.Position;
            _tileBase.Init(grid);
            OnTilePlaced?.Invoke();
            TileManager.Instance.AddNewTile(_tileBase.MyType);
            _placedTileCount++;
            ToolTipSystem.Instance.CanShowUI = true;
            _tileBase.OnPlayerHand(false);
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
            ToolTipSystem.Instance.CanShowUI = false;
            _tileBase.OnPlayerHand(true);
        }

        private void CantBuildHere()
        {
            Debug.Log("CantBuildHere");
        }

        private Vector3 GetTargetPosition()
        {
            RaycastTile(out RaycastHit hit, out HexGridNode grid);
            Vector3 targetPos = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
            if (grid == null)
            {
                if (_lastInsertedGrid != null)
                    TileInserted(null, false);
                return targetPos;
            }

            if (grid.CanBuildHere)
            {
                targetPos = grid.Position;
                if (_lastInsertedGrid != grid)
                {
                    TileInserted(_lastInsertedGrid, false);
                    TileInserted(grid, true);
                }

                return targetPos;
            }

            if (_lastInsertedGrid != null)
                TileInserted(null, false);
            return targetPos;
        }

        private void TileInserted(HexGridNode grid, bool isnserted)
        {
            _tileBase.OnPlayerInsert(isnserted, grid);
            _lastInsertedGrid = grid;
        }

        private void RotateTile(TileBase tile, bool left = false)
        {
            tile.Rotate(left);
        }
        // Check placed tile count for spawn enemy
        private void CheckPlacedTileCount()
        {
            RemainingTileCount = tileCountPerDay - _placedTileCount;
            if (RemainingTileCount <= 0)
            {
                GameManager.Instance.SetTurnState(TurnStates.EnemySpawnStart);
            }
            UIManager.Instance.gameCanvasManager.UpdateTileCountUI();
        }
        #endregion

        #region Gold
        public void AddGold(int amount)
        {
            MyGold += amount;
            UIManager.Instance.gameCanvasManager.UpdateGoldUI();
        }

        public void AddGoldPerDay(int amount)
        {
            GoldPerDay += amount;
            UIManager.Instance.gameCanvasManager.UpdateGoldToolTip();
        }
        #endregion

        #region State Change
        private void OnTurnStateChange(TurnStates state)
        {
            _canBuild = state == TurnStates.TurnBegin;
            if (_canBuild)
            {
                _placedTileCount = 0;
                CheckPlacedTileCount();
                AddGold(GoldPerDay);
            }
            if (state == TurnStates.TurnEnd)
            {
                this.Timer(0.2f, () =>
                {
                    if (GameManager.Instance.GameState == GameStates.GAME)
                        GameManager.Instance.SetTurnState(TurnStates.TurnBegin);
                });
            }
        }
        private void OnGameStateChange(GameStates state)
        {
            if (state == GameStates.GAMEOVER)
            {
                _canBuild = false;
            }
        }

        private void OnEnable()
        {
            GameManager.OnTurnStateChange += OnTurnStateChange;
            GameManager.OnGameStateChange += OnGameStateChange;
        }
        private void OnDisable()
        {
            GameManager.OnTurnStateChange -= OnTurnStateChange;
            GameManager.OnGameStateChange -= OnGameStateChange;
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

        public void PlayPartical(Vector3 pos)
        {
            var par = Instantiate(gameData.BonusPar, pos, Quaternion.identity);
            par.transform.position += Vector3.up * 0.2f;
            par.Play();
        }
    }
}
