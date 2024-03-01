using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace UI.CanvasManagers
{
    public class GameCanvasManager : MonoBehaviour
    {

        [SerializeField]
        [Tooltip("Sondan basla")]
        private SelectTileButton[] selectTileButtons;

       

        private void GetTiles()
        {

            for (int i = 0; i < 3; i++)
                selectTileButtons[i].SetTile(TileSelector.Instance.GetTileWithPrice());
            for (int i = 0; i < 3; i++)
                selectTileButtons[i + 3].SetTile(TileSelector.Instance.GetFreeTile());


            selectTileButtons[6].SetTile(TileSelector.Instance.GetPathTile());
            int rndNum = Random.Range(0, 100);
            if (rndNum < 34)
                selectTileButtons[7].SetTile(TileSelector.Instance.GetPathTile());
            else
                selectTileButtons[7].DeActivate();

            foreach (SelectTileButton button in selectTileButtons)
                if (button.MyTile == null)
                    button.DeActivate();
        }
        private void CloseAllTileButtons()
        {
            foreach (SelectTileButton button in selectTileButtons)
                button.DeActivate();
        }

        private void OnTurnStateChange(TurnStates state)
        {
            if (state == TurnStates.EnemySpawnStart)
            {
                CloseAllTileButtons();
            }
            if (state == TurnStates.TurnBegin)
            {
                GetTiles();
            }
        }

        private void OnGameStateChange(GameStates state)
        {
            if (state != GameStates.GAME)
            {
                CloseAllTileButtons();
            }
        }

        private void OnEnable()
        {
            GameManager.OnGameStateChange += OnGameStateChange;
            GameManager.OnTurnStateChange += OnTurnStateChange;
        }

        private void OnDisable()
        {
            GameManager.OnGameStateChange -= OnGameStateChange;
            GameManager.OnTurnStateChange -= OnTurnStateChange;
        }

    }
}
