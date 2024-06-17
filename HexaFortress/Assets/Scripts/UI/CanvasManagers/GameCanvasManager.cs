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
        [SerializeField] private UIEvents events;
        [SerializeField] private SelectTileButton[] selectTileButtons;
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
        private readonly int _openAnimID = Animator.StringToHash("Open");
        private readonly int _closeAnimID = Animator.StringToHash("Close");
        private const string _infoKey = "Info";

        #region Tiles
        private void GetTiles()
        {
            var tileWithPrice = 5;
            for (var i = 0; i < tileWithPrice; i++)
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
            buildingUIAnim.SetTrigger(_openAnimID);

        }
        private void GetStartTiles()
        {
            var tiles = TileSelector.Instance.GetStartTiles();
            for (int i = 0; i < tiles.Length; i++)
            {
                selectTileButtons[i].SetTile(tiles[i]);
            }
            buildingUIAnim.SetTrigger(_openAnimID);
        }
        private void CloseAllTileButtons()
        {
            foreach (SelectTileButton button in selectTileButtons)
                button.DeActivate();
            buildingUIAnim.SetTrigger(_closeAnimID);
        }
        public void UpdateTilePrice()
        {
            foreach (SelectTileButton button in selectTileButtons)
                button.UpdatePrice();
        }
        #endregion

        #region Top UI
        private void UpdateCastleHealthUI(CastleHealthChangeEvent evt)
        {
            castleHealthText.text = $"Castle {evt.CurrentHealth}/{evt.MaxHealth}";
            healthSlider.value = (float)evt.CurrentHealth/ evt.MaxHealth;
        }
        private void UpdateGoldUI(int gold)
        {
            goldText.text = gold.ToString();
        }
        private void UpdateDayUI(int dayCount)
        {
            dayText.text = $"Day {dayCount}";
        }
       
        private void UpdateTileCountUI(int remainingTileCount)
        {
            tileCountText.text = remainingTileCount.ToString();
        }

        private void UpdateGoldToolTip(int goldPerDay, int expensesPerDay)
        {
            goldToolTip.content =
                $"Daily gold producing: +{GameModel.Instance.PlayerData.GoldPerDay}\n" +
                $"Daily gold spending: -{GameModel.Instance.PlayerData.ExpensesPerDay}";
        }
        private void UpdateCastleToolTip()
        {
            castleToolTip.content =
                "Game over when castle health reaches 0.\n" +
                $"Castle repair per day: {TileManager.Instance.GetTileCount(TileType.CastleRepair)}";
        }
        #endregion

        #region Upgrade
        private void ShowUpgrades(bool show)
        {
            if (show)
            {
                this.upgrades.SetActive(true);
                var randomUpgrades = UpgradeManager.Instance.GetRandomUpgrade();
                for (short i = 0; i < randomUpgrades.Length; i++)
                {
                    upgradeButtons[i].Init(randomUpgrades[i]);
                }
            }
            else
            {
                upgrades.SetActive(false);
            }
            ToolTipSystem.Instance.CanShow3dWorldUI = !show;
        }
        #endregion

        #region Night Circle
        private void UpdateNightCircle(float ratio)
        {
            nightCircle.fillAmount = ratio;
        }
        private void ShowNightUI(bool show)
        {
            nightObj.SetActive(show);
        }
        #endregion
        
        public void ShowInfoUI(bool show)
        {
            infoUI.SetActive(show);
            if (!show)
                PlayerPrefs.SetInt(_infoKey, 1);
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
        
        #region State Change
        private void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            if (evt.TurnState == TurnStates.EnemySpawnStart)
            {
                CloseAllTileButtons();
            }
            if (evt.TurnState == TurnStates.TurnBegin)
            {
                if (GameModel.Instance.PlayerData.DayCount == 1)
                    GetStartTiles();
                else
                    GetTiles();
            }
        }
        private void OnGameStateChange(GameStateChangeEvent evt)
        {
            if (evt.GameState != GameStates.GAME && evt.GameState != GameStates.NONE)
            {
                CloseAllTileButtons();
            }
            if (evt.GameState == GameStates.GAME)
            {
                UpdateCastleToolTip();
                if (PlayerPrefs.GetInt(_infoKey, 0) == 0)
                    ShowInfoUI(true);
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener<TurnStateChangeEvent>(OnTurnStateChange);
            EventManager.AddListener<GameStateChangeEvent>(OnGameStateChange);
            EventManager.AddListener<CastleHealthChangeEvent>(UpdateCastleHealthUI);
            events.OnDayChange += UpdateDayUI;
            events.OnGoldChange += UpdateGoldUI;
            events.OnGoldIncomeChange += UpdateGoldToolTip;
            events.OnTileCountChange += UpdateTileCountUI;
            events.OnCastleToolTipChage += UpdateCastleToolTip;
            events.ShowNightUI += ShowNightUI;
            events.UpdateNightCircle += UpdateNightCircle;
            events.ShowUpgradesEvent += ShowUpgrades;
        }
        private void OnDisable()
        {
            EventManager.RemoveListener<TurnStateChangeEvent>(OnTurnStateChange);
            EventManager.RemoveListener<GameStateChangeEvent>(OnGameStateChange);
            EventManager.RemoveListener<CastleHealthChangeEvent>(UpdateCastleHealthUI);
            events.OnDayChange -= UpdateDayUI;
            events.OnGoldChange -= UpdateGoldUI;
            events.OnGoldIncomeChange -= UpdateGoldToolTip;
            events.OnTileCountChange -= UpdateTileCountUI;
            events.OnCastleToolTipChage -= UpdateCastleToolTip;
            events.ShowNightUI -= ShowNightUI;
            events.UpdateNightCircle -= UpdateNightCircle;
            events.ShowUpgradesEvent -= ShowUpgrades;
        }
        #endregion
    }
}
