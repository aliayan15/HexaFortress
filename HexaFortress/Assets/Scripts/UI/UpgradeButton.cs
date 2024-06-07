using HexaFortress.GamePlay;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexaFortress.UI
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI dec;
        [SerializeField] private TextMeshProUGUI level;
        [Space(10)]
        [SerializeField] private SOGameProperties gameData;

        private SOUpgradeData _myData;


        public void Init(SOUpgradeData data)
        {
            _myData = data;
            switch (data.TileType)
            {
                case TileType.Tower:
                    icon.sprite = gameData.TowerIcon;
                    break;
                case TileType.Cannon:
                    icon.sprite = gameData.CannonIcon;
                    break;
                case TileType.Mortar:
                    icon.sprite = gameData.MortarIcon;
                    break;
            }
            dec.text = data.Description;
            level.text = "Level: " + UpgradeManager.Instance.GetUpgradeLevel(data);
        }

        public void OnSelect()
        {
            UpgradeManager.Instance.Upgrade(_myData);
            UIManager.Instance.gameCanvasManager.ShowUpgrades(false);
            AudioManager.Instance.PlayBtnSound();
        }
    }
}

