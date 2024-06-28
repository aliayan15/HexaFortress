using UnityEngine;
using System;
using MyUtilities;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace HexaFortress.Game
{
    public enum GameStates
    {
        NONE,
        GAME,
        MENU,
        PAUSE,
        GAMEOVER,
        GAMEWIN,
        LOADING
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
        [Space(10)] [Header("Debug")] [SerializeField]
        private bool isDebuging;

        #region Set Game State

        [Space(15)] [Header("Set State")] public GameStates GameStateToSet;
        private GameStates _gameState;

        public GameStates GameState
        {
            get { return _gameState; }
            private set
            {
                _gameState = value;
                var evt = Events.GameStateChangeEvent;
                evt.GameState = value;
                EventManager.Broadcast(evt);
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
            if (isDebuging) Debug.Log(GameState);
        }

        #endregion

        #region Turn State

        [Space(5)] [Header("Turn State")] [SerializeField]
        private TurnStates turnStateToSet;

        private TurnStates _turnState;

        public TurnStates TurnState
        {
            get => _turnState;
            private set
            {
                _turnState = value;
                var evt = Events.TurnStateChangeEvent;
                evt.TurnState = value;
                EventManager.Broadcast(evt);
            }
        }

        public void SetTurnState(TurnStates state)
        {
            TurnState = state;
        }
        #endregion

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            ShowCursor(true);
            Application.targetFrameRate = 60;
            GameState = GameStates.NONE;
            TurnState = TurnStates.None;
        }

        public void StartGame()
        {
            SetState(GameStates.GAME);
            this.Timer(0.1f, (() =>
            {
                SetTurnState(TurnStates.TurnBegin);
            }));
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