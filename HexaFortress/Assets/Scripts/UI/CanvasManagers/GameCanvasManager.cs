using HexaFortress.Game;
using HexaFortress.GamePlay;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HexaFortress.UI
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
                if (i == 0)
                {
                    selectTileButtons[i].SetTile(TileSelector.Instance.GetTowerTile());
                    continue;
                }
                if (i == 2)
                {
                    int rndNum1 = Random.Range(0, 100);
                    if (rndNum1 < 40) // second tower
                    {
                        selectTileButtons[i].SetTile(TileSelector.Instance.GetTowerTile());
                        continue;
                    }
                }
                selectTileButtons[i].SetTile(TileSelector.Instance.GetTileWithPrice());
            }
            // path tile
            selectTileButtons[7].SetTile(TileSelector.Instance.GetPathTile());
            int rndNum2 = Random.Range(0, 100);
            if (rndNum2 < 70) // 2 path chance
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
        public void UpdateTilePrice()
        {
            foreach (SelectTileButton button in selectTileButtons)
                button.UpdatePrice();
        }
        #endregion

        #region Top UI
        public void UpdateGoldUI()
        {
            goldText.text = Player.Instance.MyGold.ToString();
        }
        public void UpdateDayUI()
        {
            dayText.text = "Day " + GameManager.Instance.DayCount;
        }
       
        public void UpdateTileCountUI()
        {
            tileCountText.text = Player.Instance.RemainingTileCount.ToString();
        }

        public void UpdateGoldToolTip()
        {
            goldToolTip.content = "Daily gold producing: +" + Player.Instance.GoldPerDay + "\n"
                + "Daily gold spending: -" + Player.Instance.ExpensesPerDay;
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
            ToolTipSystem.Instance.CanShow3dWorldUI = !show;
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
            ToolTipSystem.Instance.CanShow3dWorldUI = !show;
        }
        public void ToggleInfoUI()
        {
            ShowInfoUI(!infoUI.activeInHierarchy);
        }

        public void StartAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        private void UpdateCastleHealthUI(CastleHealthChangeEvent evt)
        {
            castleHealthText.text = "Castle " + evt.CurrentHealth + "/" + evt.MaxHealth;
            healthSlider.value = (float)evt.CurrentHealth/ evt.MaxHealth;
        }
        
        #region State Change
        private void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            if (evt.TurnState == TurnStates.EnemySpawnStart)
            {
                CloseAllTileButtons();
            }
            if (evt.TurnState == TurnStates.TurnBegin)
            {
                if (GameManager.Instance.DayCount == 1)
                    GetStartTiles();
                else
                    GetTiles();
                UpdateGoldUI();
                UpdateDayUI();
                UpdateTileCountUI();
            }
        }
        private void OnGameStateChange(GameStateChangeEvent evt)
        {
            if (evt.GameState != GameStates.GAME && evt.GameState != GameStates.NONE)
            {
                CloseAllTileButtons();
                UpdateGoldUI();
                UpdateDayUI();
                UpdateTileCountUI();
            }
            if (evt.GameState == GameStates.GAME)
            {
                UpdateCastleToolTip();
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener<TurnStateChangeEvent>(OnTurnStateChange);
            EventManager.AddListener<GameStateChangeEvent>(OnGameStateChange);
            EventManager.AddListener<CastleHealthChangeEvent>(UpdateCastleHealthUI);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<TurnStateChangeEvent>(OnTurnStateChange);
            EventManager.RemoveListener<GameStateChangeEvent>(OnGameStateChange);
            EventManager.RemoveListener<CastleHealthChangeEvent>(UpdateCastleHealthUI);
        }
        #endregion
    }
}
