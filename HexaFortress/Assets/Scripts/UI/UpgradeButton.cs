using HexaFortress.Game;
using HexaFortress.GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace HexaFortress.UI
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private UIEvents events;
        [Space(10)]
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI dec;
        [SerializeField] private TextMeshProUGUI level;
        [FormerlySerializedAs("generalGameData")]
        [FormerlySerializedAs("gameData")]
        [Space(10)]
        [SerializeField] private GeneralConfig generalConfig;

        private SOUpgradeData _myData;


        public void Init(SOUpgradeData data)
        {
            _myData = data;
            switch (data.TileType)
            {
                case TileType.Tower:
                    icon.sprite = generalConfig.TowerIcon;
                    break;
                case TileType.Cannon:
                    icon.sprite = generalConfig.CannonIcon;
                    break;
                case TileType.Mortar:
                    icon.sprite = generalConfig.MortarIcon;
                    break;
            }
            dec.text = data.Description;
            level.text = "Level: " + UpgradeManager.Instance.GetUpgradeLevel(data);
        }

        public void OnSelect()
        {
            UpgradeManager.Instance.Upgrade(_myData);
            events.ShowUpgradesEvent.Invoke(false);
            AudioManager.Instance.PlayBtnSound();
        }
    }
}

