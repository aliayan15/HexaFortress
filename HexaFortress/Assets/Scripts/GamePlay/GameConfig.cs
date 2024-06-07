using UnityEngine;

namespace HexaFortress.GamePlay
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObject/Game Config")]
    public class GameConfig : ScriptableObject
    {
        private static GameConfig _instance;

        public static GameConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load(nameof(GameConfig)) as GameConfig;
                }

                return _instance;
            }
        }

        public Player Player { get; set; }
        public CastleTile CastleTile { get; set; }
    }
}