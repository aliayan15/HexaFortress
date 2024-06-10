using System;
using DG.Tweening;
using HexaFortress.Game;
using MyUtilities;
using MyUtilities.UI;
using UI;
using UnityEngine;

namespace HexaFortress.UI
{
    public class UIManager : SingletonMono<UIManager>
    {
        // Panels
        public CanvasGrupItem GAME;
        public CanvasGrupItem GAMEWIN;
        public CanvasGrupItem GAMEOVER;
        public CanvasGrupItem MENU;
        public CanvasGrupItem LOADING;


        [Header("Canvas Scriptleri")]
        public GameCanvasManager gameCanvasManager;

        private CanvasGrupItem[] _canvases;

        protected override void Awake()
        {
            base.Awake();
            _canvases = new[] { MENU, GAME, GAMEOVER, GAMEWIN, LOADING };
        }
        public void QuitGame()
        {
            Application.Quit();
        }
        private void SetLoading()
        {
            _canvases.HideAllExceptOne(LOADING);
        }
        private void SetGameOver()
        {
            _canvases.HideAllExceptOne(GAMEOVER);
            GAMEOVER.canvasGroup.alpha = 0;
            GAMEOVER.canvasGroup.DOFade(1f, 1f);
        }
        private void SetGameWin()
        {
            _canvases.HideAllExceptOne(GAMEWIN);
        }
       
        private void SetMenu()
        {
            _canvases.HideAllExceptOne(MENU);
        }
        private void SetGame()
        {
            _canvases.HideAllExceptOne(GAME);
        }

        
        private void OnGameStateChange(GameStateChangeEvent obj)
        {
            switch (obj.GameState)
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
                case GameStates.LOADING:
                    SetLoading();
                    break;
                default:
                    break;
            }
        }
        private void OnEnable()
        {
            EventManager.AddListener<GameStateChangeEvent>(OnGameStateChange);
        }
        
        private void OnDisable()
        {
            EventManager.RemoveListener<GameStateChangeEvent>(OnGameStateChange);
        }
    }
}
