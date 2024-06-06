using UnityEngine;
using System;
using MyUtilities;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Managers
{
    public enum GameStates
    {
        NONE,
        GAME,
        MENU,
        PAUSE,
        GAMEOVER,
        GAMEWIN
    }
    public enum TurnStates
    {
        None,
        TurnBegin,
        EnemySpawnStart,
        TurnEnd
    }

    public class GameManager : SingletonMono<GameManager>
    {
        public static event Action<GameStates> OnGameStateChange;

        [HideInInspector] public Player player { get; set; }
        public int DayCount { get; private set; } = 1;


        [Space(10)]
        [Header("Debug")]
        [SerializeField] private bool isDebuging;

        private const string CanInstantiateSystemBoost = "MyBooleanProperty";

        #region Set State

        [Space(15)]
        [Header("Set State")]
        public GameStates GameStateToSet;

        private GameStates _gameState;
        public GameStates GameState
        {
            get
            {
                return _gameState;
            }
            set
            {
                _gameState = value;
                OnGameStateChange?.Invoke(value);
            }
        }

        /// <summary>
        /// Set Game State.
        /// </summary>
        /// <param name="state"></param>
        public void SetState(GameStates state)
        {
            if (GameState == state) return;
            GameState = state;
            switch (state)
            {
                case GameStates.GAME:
                    SetGame();
                    break;
                case GameStates.MENU:
                    SetMenu();
                    break;

                case GameStates.GAMEOVER:
                    SetGameOver();
                    break;
                case GameStates.GAMEWIN:
                    SetGameWin();
                    break;
                default:
                    break;
            }

            if (isDebuging) Debug.Log(GameState);
        }

        private void SetGameWin()
        {
            UIManager.Instance.SetGameWin();
        }

        private void SetGame()
        {
            UIManager.Instance.SetGame();
        }

        private void SetMenu()
        {
            UIManager.Instance.SetMenu();
        }

        private void SetGameOver()
        {
            UIManager.Instance.SetGameOver();
        }
        #endregion

        #region Game Turn State

        public static event Action<TurnStates> OnTurnStateChange;
        [Space(5)]
        [Header("Game Turn State")]
        [SerializeField] private TurnStates turnStateToSet;
        private TurnStates _turnState;
        public TurnStates TurnState
        {
            get
            {
                return _turnState;
            }
            private set
            {
                _turnState = value;
                OnTurnStateChange?.Invoke(value);
            }
        }

        public void SetTurnState(TurnStates state)
        {
            TurnState = state;
        }
        public void IncreaseDay(int day)
        {
            DayCount += day;
            UIManager.Instance.gameCanvasManager.UpdateDayUI();
            //if (DayCount == EnemySpawner.Instance.GameEndDay)
            //{
            //    SetState(GameStates.GAMEWIN);
            //}
        }
        #endregion



        private void Start()
        {
#if UNITY_EDITOR
            bool canInstantiateSystemBoost = EditorPrefs.GetBool(CanInstantiateSystemBoost, false);
            if (!canInstantiateSystemBoost)
            {
                // load next scene
                LoadNextScene();
            }
#else
            LoadNextScene();
#endif
            //Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            ShowCursor(true);
            Application.targetFrameRate = 60;
        }

        // Load next scene when main scene load.
        private void LoadNextScene()
        {
            GameState = GameStates.NONE;
            TurnState = TurnStates.None;
            UIManager.Instance.SetLoading();
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // next scene can be game or menu scene.
        }

        public void StartGame()
        {
            DayCount = 1;
            SetState(GameStates.GAME);
            AudioManager.Instance.ClearAudioList();
        }

        #region Mouse Visibility
        public void ShowCursor(bool show)
        {
            if (Cursor.lockState != CursorLockMode.None)
                Cursor.lockState = CursorLockMode.None;
            Cursor.visible = show;
        }


        #endregion

    }
}
