using DG.Tweening;
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

        public void SetGameOver()
        {
            _canvases.HideAllExceptOne(GAMEOVER);
            GAMEOVER.canvasGroup.alpha = 0;
            GAMEOVER.canvasGroup.DOFade(1f, 1f);
        }
        public void SetGameWin()
        {
            _canvases.HideAllExceptOne(GAMEWIN);
        }
        public void SetLoading()
        {
            _canvases.HideAllExceptOne(LOADING);
        }
        public void SetMenu()
        {
            _canvases.HideAllExceptOne(MENU);
        }
        public void SetGame()
        {
            _canvases.HideAllExceptOne(GAME);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
