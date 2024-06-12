using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class EnemyFactory
    {
        public Enemy Create(EnemyConfig config)
        {
            Enemy newEnemy = Object.Instantiate(config.EnemyPrefab);
            newEnemy.Init(config);
            return newEnemy;
        }
    }
}