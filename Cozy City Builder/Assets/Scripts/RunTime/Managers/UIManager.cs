using UnityEngine;
using UI;
using MyUtilities.UI;
using UI.CanvasManagers;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        // Panels
        public CanvasGrupItem GAME;
        public CanvasGrupItem LEVELCOMPLETE;
        public CanvasGrupItem GAMEOVER;
        public CanvasGrupItem MENU;
        public CanvasGrupItem LOADING;


        [Header("Canvas Scriptleri")]
        public GameCanvasManager gameCanvasManager;

        private CanvasGrupItem[] _canvases;

        private void Awake()
        {
            _canvases = new[] { MENU, GAME, LEVELCOMPLETE, GAMEOVER,LOADING };
        }
       
        public void SetGameOver()
        {
            _canvases.HideAllExceptOne(GAMEOVER);
        }
        public void SetLevelComolete()
        {
            _canvases.HideAllExceptOne(LEVELCOMPLETE);
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
    }
}
