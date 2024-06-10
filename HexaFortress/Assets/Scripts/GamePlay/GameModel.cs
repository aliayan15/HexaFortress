using UnityEngine;

namespace HexaFortress.GamePlay
{
    [CreateAssetMenu(fileName = "GameModel", menuName = "ScriptableObject/Game Config")]
    public class GameModel : ScriptableObject
    {
        private static GameModel _instance;
        public static GameModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load(nameof(GameModel)) as GameModel;
                }

                return _instance;
            }
        }

        public PlayerTilePlacer PlayerTilePlacer { get; set; }
        public PlayerData PlayerData { get; set; }
        public CastleTile CastleTile { get; set; }
    }
}