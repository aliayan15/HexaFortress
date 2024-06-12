using System;
using System.Collections.Generic;
using HexaFortress.Game;
using UnityEngine;
using UnityEngine.Events;

namespace HexaFortress.GamePlay
{
    public class PlayerTilePlacer : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private UIEvents events;
        [Header("Settings")]
        [SerializeField] private float tileMoveSpeed = 10.0f;
        [SerializeField] private short tileCountPerDay = 4;
        [SerializeField] private LayerMask gridPosLayer;
        [Header("Animation")]
        [SerializeField] private AnimationCurve placementAnimY;
        [Header("Debug")]
        [SerializeField] private bool isDebug;
        [SerializeField] private SOTileData[] tilesToPlace;
        
        public int RemainingTileCount { get; set; }
        public UnityAction OnTilePlaced;
        public UnityAction OnTileCanceled;

        private SOTileData _currentTile;
        private Transform _tileToBuild;
        private TileBase _tileBase;
        private bool _isBuildMode = false;
        private short _placedTileCount = 0;
        private HexGridNode _lastInsertedGrid = null;

        private void Awake()
        {
            GameModel.Instance.PlayerTilePlacer = this;
        }

        private void Update()
        {
            if (GameManager.Instance.GameState!=GameStates.GAME) 
                return;
#if UNITY_EDITOR
            if (isDebug)
                DebugTilePlacement();
#endif
            if (!_isBuildMode) return;

            UpdatePosition();
            // rotate
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f)
                RotateTile(_tileBase);
            else if (scroll < 0f)
                RotateTile(_tileBase, true);
        }

        private void OnDeselectKeyPressed()
        {
            CanselSelection();
        }

        private void OnSelectKeyPressed()
        {
            if (RaycastTile(out HexGridNode grid))
            {
                if (!grid.CanBuildHere) return;
                if (!_tileBase.CanBuildHere(grid))
                {
                    CantBuildHere();
                    return;
                }

                PlaceTile(grid);
            }
        }

        private void DebugTilePlacement()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (RaycastTile(out HexGridNode grid))
                {
                    if (grid.MyTile is not PathTile) return;
                    List<Vector3> pathList = GridManager.Instance.PathFinding.FindPath(grid.Position,
                        GameModel.Instance.CastleTile.MyHexNode.Position);
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
            // TODO AudioManager.Instance.Play2DSound(SoundTypes.TilePlace);
        }

        private void UpdatePosition()
        {
            Vector3 pos = GetTargetPosition();
            _tileToBuild.position = Vector3.Lerp(_tileToBuild.position, pos, tileMoveSpeed * Time.deltaTime);
        }

        public void EnterBuildMode(SOTileData tile)
        {
            if (GameManager.Instance.GameState!=GameStates.GAME) 
                return;
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
            if (GameModel.Instance.PlayerData.DayCount == 2)
                tileCountPerDay = 5;

            RemainingTileCount = tileCountPerDay - _placedTileCount;
            if (RemainingTileCount <= 0)
            {
                GameManager.Instance.SetTurnState(TurnStates.EnemySpawnStart);
            }
            events.OnTileCountChange.Invoke(RemainingTileCount);
        }
        private void SetIsBuilding(bool isBuilding)
        {
            _isBuildMode = isBuilding;
            OnPlayerBuildModeEvent evt = Events.OnPlayerBuildModeEvent;
            evt.IsBuilding = isBuilding;
            EventManager.Broadcast(evt);
        }
        #endregion
        
        private void SetToolTipCanShowUI(bool canShow)
        {
            events.ShowToolTipUI.Invoke(canShow);
        }
        
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
        
        #region State Change
        private void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            if (evt.TurnState==TurnStates.TurnBegin)
            {
                _placedTileCount = 0;
                CheckPlacedTileCount();
            }
            else if (_isBuildMode)
            {
                CanselSelection();
            }
            events.Show3dWorldUI.Invoke(evt.TurnState==TurnStates.TurnBegin);
        }
      
        private void OnEnable()
        {
            EventManager.AddListener<TurnStateChangeEvent>(OnTurnStateChange);
            inputReader.SelectEvent += OnSelectKeyPressed;
            inputReader.DeselectEvent += OnDeselectKeyPressed;
        }
        private void OnDisable()
        {
            EventManager.RemoveListener<TurnStateChangeEvent>(OnTurnStateChange);
            inputReader.SelectEvent -= OnSelectKeyPressed;
            inputReader.DeselectEvent -= OnDeselectKeyPressed;
        }
        #endregion
    }
}