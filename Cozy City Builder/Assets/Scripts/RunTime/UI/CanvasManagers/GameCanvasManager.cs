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
        [SerializeField] private ToolTipTrigger castleToolTip;
        [SerializeField] private GameObject infoUI;
        [SerializeField] private Animator buildingUIAnim;
        [Header("Upgrade")]
        [SerializeField] GameObject upgrades;
        [SerializeField] UpgradeButton[] upgradeButtons;
        [Header("Night")]
        [SerializeField] private Image nightCircle;
        [SerializeField] GameObject nightObj;


        #region Tiles
        public void GetTiles()
        {
            int tileWithPrice = 5;
            for (int i = 0; i < tileWithPrice; i++)
            {
                if (i == 0) { selectTileButtons[i].SetTile(TileSelector.Instance.GetTowerTile()); continue; }
                selectTileButtons[i].SetTile(TileSelector.Instance.GetTileWithPrice());
            }
            // path tile
            selectTileButtons[7].SetTile(TileSelector.Instance.GetPathTile());
            int rndNum = Random.Range(0, 100);
            if (rndNum < 50)
                selectTileButtons[8].SetTile(TileSelector.Instance.GetPathTile());
            else
                selectTileButtons[8].DeActivate();

            foreach (SelectTileButton button in selectTileButtons)
                if (button.MyTile == null)
                    button.DeActivate();
            buildingUIAnim.SetTrigger("Open");

        }
        private void GetStartTiles()
        {
            var tiles = TileSelector.Instance.GetStartTiles();
            for (int i = 0; i < tiles.Length; i++)
            {
                selectTileButtons[i].SetTile(tiles[i]);
            }
            buildingUIAnim.SetTrigger("Open");
        }
        private void CloseAllTileButtons()
        {
            foreach (SelectTileButton button in selectTileButtons)
                button.DeActivate();
            buildingUIAnim.SetTrigger("Close");
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
            goldToolTip.content = "Daily gold producing: +" + GameManager.Instance.player.GoldPerDay + "\n"
                + "Daily gold spending: -" + GameManager.Instance.player.ExpensesPerDay;
        }
        public void UpdateCastleToolTip()
        {
            castleToolTip.content = "Game over when castle health reaches 0.\n" +
                "Castle repair per day: " + TileManager.Instance.GetTileCount(TileType.CastleRepair);
        }
        #endregion

        #region Upgrade
        public void ShowUpgrades(bool show)
        {
            if (show)
            {
                this.upgrades.SetActive(true);
                var upgrades = UpgradeManager.Instance.GetRandomUpgrade();
                for (short i = 0; i < upgrades.Length; i++)
                {
                    upgradeButtons[i].Init(upgrades[i]);
                }
            }
            else
            {
                upgrades.SetActive(false);
            }
        }
        #endregion

        public void UpdateNightCircle(float ratio)
        {
            nightCircle.fillAmount = ratio;
        }
        public void ShowNightUI(bool show)
        {
            nightObj.SetActive(show);
        }

        public void ShowInfoUI(bool show)
        {
            infoUI.SetActive(show);
            if (!show)
                PlayerPrefs.SetInt("Info", 1);
        }

        public void StartAgain()
        {
            SceneManager.LoadScene(1);
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
            if (state != GameStates.GAME && state != GameStates.NONE)
            {
                CloseAllTileButtons();
                UpdateGoldUI();
                UpdateDayUI();
                UpdateTileCountUI();
                UpdateCastleHealthUI();
            }
            if (state == GameStates.GAME)
            {
                UpdateCastleToolTip();
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
