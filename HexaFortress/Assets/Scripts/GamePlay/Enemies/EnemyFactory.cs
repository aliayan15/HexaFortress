using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class EnemyFactory
    {
        public EnemyController Create(EnemyConfig config)
        {
            EnemyController newEnemyController = Object.Instantiate(config.enemyControllerPrefab);
            newEnemyController.Init(config);
            return newEnemyController;
        }
    }
}