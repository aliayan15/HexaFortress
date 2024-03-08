using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine.SceneManagement;

namespace UI.CanvasManagers
{
    public class GameCanvasManager : MonoBehaviour
    {

        [SerializeField]
        [Tooltip("Sondan basla")]
        private SelectTileButton[] selectTileButtons;
        [HorizontalLine]
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private TextMeshProUGUI castleHealthText;
        [SerializeField] private TextMeshProUGUI tileCountText;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private ToolTipTrigger goldToolTip;


        #region Tiles
        private void GetTiles()
        {

            for (int i = 0; i < 3; i++)
                selectTileButtons[i].SetTile(TileSelector.Instance.GetTileWithPrice());
            for (int i = 0; i < 3; i++)
                selectTileButtons[i + 3].SetTile(TileSelector.Instance.GetFreeTile());


            selectTileButtons[6].SetTile(TileSelector.Instance.GetPathTile());
            int rndNum = Random.Range(0, 100);
            if (rndNum < 45)
                selectTileButtons[7].SetTile(TileSelector.Instance.GetPathTile());
            else
                selectTileButtons[7].DeActivate();

            foreach (SelectTileButton button in selectTileButtons)
                if (button.MyTile == null)
                    button.DeActivate();

        }
        private void GetStartTiles()
        {
            var tiles = TileSelector.Instance.GetStartTiles();
            for (int i = 0; i < tiles.Length; i++)
            {
                selectTileButtons[i].SetTile(tiles[i]);
            }
        }
        private void CloseAllTileButtons()
        {
            foreach (SelectTileButton button in selectTileButtons)
                button.DeActivate();
        }
        #endregion

        #region Top UI
        public void UpdateGoldUI()
        {
            goldText.text = GameManager.Instance.player.MyGold.ToString();
        }
        public void UpdateDayUI()
        {
            dayText.text = "Day " + GameManager.Instance.DayCount;
        }
        public void UpdateCastleHealthUI()
        {
            var castle = GridManager.Instance.PlayerCastle;
            castleHealthText.text = "Castle " + castle.CastleHealth + "/" + castle.MaxCastleHealth;
            healthSlider.value = (float)castle.CastleHealth / castle.MaxCastleHealth;
        }
        public void UpdateTileCountUI()
        {
            tileCountText.text = GameManager.Instance.player.RemainingTileCount.ToString();
        }

        public void UpdateGoldToolTip()
        {
            goldToolTip.content = "Gold per day: +" + GameManager.Instance.player.GoldPerDay;
        }
        #endregion

        public void StartAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnTurnStateChange(TurnStates state)
        {
            if (state == TurnStates.EnemySpawnStart)
            {
                CloseAllTileButtons();
            }
            if (state == TurnStates.TurnBegin)
            {
                if (GameManager.Instance.DayCount == 1)
                    GetStartTiles();
                else
                    GetTiles();
                UpdateGoldUI();
                UpdateDayUI();
                UpdateTileCountUI();
                UpdateCastleHealthUI();
            }
        }

        private void OnGameStateChange(GameStates state)
        {
            if (state != GameStates.GAME)
            {
                CloseAllTileButtons();
                UpdateGoldUI();
                UpdateDayUI();
                UpdateTileCountUI();
                UpdateCastleHealthUI();
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
