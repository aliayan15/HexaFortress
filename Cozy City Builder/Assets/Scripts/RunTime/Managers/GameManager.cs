using UnityEngine;
using System;
using Players;
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
        GAMEOVER
    }
    public enum TurnStates
    {
        None,
        TurnBegin,
        EnemySpawn,
        TurnEnd
    }

    public class GameManager : SingletonMono<GameManager>
    {
        public static event Action<GameStates> OnGameStateChange;

        [Space(10)]
        [Header("Refs")]
        public UIManager uiManager;
        [HideInInspector] public Player player { get; set; }


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
                default:
                    break;
            }

            if (isDebuging) Debug.Log(GameState);
        }

        private void SetGame()
        {
            uiManager.SetGame();
        }

        private void SetMenu()
        {
            uiManager.SetMenu();
        }

        private void SetGameOver()
        {
            uiManager.SetGameOver();
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
            //ShowCursor(false);
        }

        // Load next scene when main scene load.
        private void LoadNextScene()
        {
            GameState = GameStates.NONE;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // next scene can be game or menu scene.
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
