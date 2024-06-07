using System.Collections.Generic;
using HexaFortress.Game;
using Managers;
using MyUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace HexaFortress.GamePlay
{
    public class Player : SingletonMono<Player>
    {
        [SerializeField] private LayerMask gridPosLayer;
        [SerializeField] private GameObject pfUnwalkable;
        [SerializeField] private SOGameProperties gameData;
        [Header("Settings")]
        [SerializeField] private float tileMoveSpeed = 10.0f;
        [SerializeField] private short tileCountPerDay = 4;
        [SerializeField] private float goNightTime = 2f;
        [Space(10)]
        [Header("Debug")]
        [SerializeField] private bool isDebug;
        [SerializeField] private SOTileData[] tilesToPlace;
        [Header("Animation")]
        [SerializeField] private AnimationCurve placementAnimY;

        [field: SerializeField]
        public int MyGold { get; private set; }
        public int GoldPerDay { get; private set; } = 0;
        public int ExpensesPerDay { get; private set; } = 0;
        public int RemainingTileCount { get; set; }
        public bool IsBuilding => _isBuildMode;

        public UnityAction OnTilePlaced;
        public UnityAction OnTileCanceled;

        private SOTileData _currentTile;
        private Transform _tileToBuild;
        private TileBase _tileBase;
        private bool _isBuildMode = false;
        private bool _canBuild = false;
        private short _placedTileCount = 0;
        private HexGridNode _lastInsertedGrid = null;
        private bool _isSpaceHold = false;
        private float _goNightTimer = 0;

       

        private void Start()
        {
            GameManager.Instance.StartGame();
            //TODO UIManager
            // if (PlayerPrefs.GetInt("Info", 0) == 0)
            //     UIManager.Instance.gameCanvasManager.ShowInfoUI(true);
        }

        private void Update()
        {
            if (isDebug)
                DebugGame();

            if (!_canBuild) return; // onGame
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _goNightTimer = 0;
                _isSpaceHold = true;
               //TODO  UIManager.Instance.gameCanvasManager.ShowNightUI(true);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _isSpaceHold = false;
                //TODO UIManager.Instance.gameCanvasManager.ShowNightUI(false);
            }
            if (_isSpaceHold)
            {
                _goNightTimer += Time.deltaTime;
                //TODO UIManager.Instance.gameCanvasManager.UpdateNightCircle(_goNightTimer / goNightTime);
                if (_goNightTimer >= goNightTime)
                {
                    // skip to night
                    GameManager.Instance.SetTurnState(TurnStates.EnemySpawnStart);
                    _isSpaceHold = false;
                    //TODO UIManager.Instance.gameCanvasManager.ShowNightUI(false);
                }
            }

            if (!_isBuildMode) return;

            UpdatePosition();
            // place tile
            if (Input.GetMouseButtonDown(0))
            {
                if (RaycastTile(out HexGridNode grid))
                {
                    if (!grid.CanBuildHere) return;
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
            SetIsBuilding(false);
            Destroy(_tileToBuild.gameObject);
            _tileBase.OnPlayerHand(false);
            _tileBase = null;
            SetToolTipCanShowUI(true);
            OnTileCanceled?.Invoke();
        }
        private void PlaceTile(HexGridNode grid)
        {
            SetIsBuilding(false);
            _tileToBuild.transform.position = grid.Position;
            _tileBase.Init(grid);
            OnTilePlaced?.Invoke();
            // placement anim
            SetToolTipCanShowUI(false);
            var anim = _tileToBuild.gameObject.AddComponent<CurveAnimation>();
            anim.curve = placementAnimY;

            _tileBase.OnPlayerHand(false);
            _placedTileCount++;
            CheckPlacedTileCount();
            AudioManager.Instance.Play2DSound(SoundTypes.TilePlace);
        }

        private void UpdatePosition()
        {
            Vector3 pos = GetTargetPosition();
            _tileToBuild.position = Vector3.Lerp(_tileToBuild.position, pos, tileMoveSpeed * Time.deltaTime);
        }

        public void EnterBuildMode(SOTileData tile)
        {
            if (!_canBuild) return;
            if (_isBuildMode)
                CanselSelection();
            _currentTile = tile;
            _tileToBuild = Instantiate(_currentTile.Prefab).transform;
            _tileBase = _tileToBuild.GetComponent<TileBase>();
            Vector3 pos = GetTargetPosition();
            _tileToBuild.position = pos;
            SetIsBuilding(true);
            SetToolTipCanShowUI(false);
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
            // firs day 4 tile then 5 tile
            if (GameManager.Instance.DayCount == 2)
                tileCountPerDay = 5;

            RemainingTileCount = tileCountPerDay - _placedTileCount;
            if (RemainingTileCount <= 0)
            {
                GameManager.Instance.SetTurnState(TurnStates.EnemySpawnStart);
            }
            //TODO UIManager.Instance.gameCanvasManager.UpdateTileCountUI();
        }
        #endregion

        private void SetToolTipCanShowUI(bool canShow)
        {
            var evt = Events.ToolTipCanShowUIEvent;
            evt.CanShow = canShow;
            EventManager.Broadcast(evt);
        }

        #region Gold
        public void AddGold(int amount)
        {
            MyGold += amount;
            if (MyGold < 0)
                MyGold = 0;
            //TODO UIManager.Instance.gameCanvasManager.UpdateGoldUI();
        }

        public void AddGoldPerDay(int amount)
        {
            GoldPerDay += amount;
            //TODO UIManager.Instance.gameCanvasManager.UpdateGoldToolTip();
        }
        public void AddExpensesPerDay(int amount)
        {
            ExpensesPerDay += amount;
            //TODO UIManager.Instance.gameCanvasManager.UpdateGoldToolTip();
        }
        #endregion

        #region State Change
        private void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            _canBuild = evt.TurnState == TurnStates.TurnBegin;
            if (_canBuild)
            {
                _placedTileCount = 0;
                CheckPlacedTileCount();
                AddGold(GoldPerDay);
                AddGold(-ExpensesPerDay);
            }
            else if (_isBuildMode)
            {
                CanselSelection();
            }
            if (evt.TurnState == TurnStates.TurnEnd)
            {
                this.Timer(0.2f, () =>
                {
                    if (GameManager.Instance.GameState == GameStates.GAME)
                        GameManager.Instance.SetTurnState(TurnStates.TurnBegin);
                });
            }
            //TODO ToolTipSystem.Instance.CanShow3dWorldUI = _canBuild;
        }
        private void OnGameStateChange(GameStateChangeEvent evt)
        {
            bool isGame = evt.GameState == GameStates.GAME;
            _canBuild = isGame;
            //TODO ToolTipSystem.Instance.CanShowUI = isGame;
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

        private void SetIsBuilding(bool isBuilding)
        {
            _isBuildMode = isBuilding;
            OnPlayerBuildModeEvent evt = Events.OnPlayerBuildModeEvent;
            evt.IsBuilding = isBuilding;
            EventManager.Broadcast(evt);
        }
    }
}
