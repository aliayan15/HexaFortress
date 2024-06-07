using UnityEngine;

namespace HexaFortress.GamePlay
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Upgrade Data")]
    public class SOUpgradeData : ScriptableObject
    {
        public TileType TileType;
        public UpgradeType Upgrade;
        public string Description;
    }
}

