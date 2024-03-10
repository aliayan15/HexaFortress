using UnityEngine;
using UI;
using MyUtilities.UI;
using UI.CanvasManagers;
using MyUtilities;

namespace Managers
{
    public class UIManager : SingletonMono<UIManager>
    {
        // Panels
        public CanvasGrupItem GAME;
        public CanvasGrupItem GAMEWIN;
        public CanvasGrupItem GAMEOVER;
        public CanvasGrupItem MENU;
        //public CanvasGrupItem LOADING;


        [Header("Canvas Scriptleri")]
        public GameCanvasManager gameCanvasManager;

        private CanvasGrupItem[] _canvases;

        protected override void Awake()
        {
            base.Awake();
            _canvases = new[] { MENU, GAME, GAMEOVER, GAMEWIN };
        }

        public void SetGameOver()
        {
            _canvases.HideAllExceptOne(GAMEOVER);
        }
        public void SetGameWin()
        {
            _canvases.HideAllExceptOne(GAMEWIN);
        }
        public void SetLoading()
        {
            //_canvases.HideAllExceptOne(LOADING);
        }
        public void SetMenu()
        {
            _canvases.HideAllExceptOne(MENU);
        }
        public void SetGame()
        {
            _canvases.HideAllExceptOne(GAME);
        }


    }
}
