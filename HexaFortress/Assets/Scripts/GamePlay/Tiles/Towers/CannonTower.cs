using System.Collections.Generic;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class CannonTower : TowerTileBase
    {
        [HorizontalLine]
        [SerializeField] protected Transform firePoint;
        [SerializeField] protected Transform muzzle;
        [Space(5)]
        [SerializeField] protected Projectile projectile;
        [SerializeField] protected SoundTypes fireSound;

        private EnemyController _currentTarget = null;
        [HideInInspector]
        public HashSet<EnemyController> Targets = new HashSet<EnemyController>();

        protected override void OnFire()
        {
            // get an enemy
            if (!_currentTarget)
            {
                _currentTarget = GetEnemy();
                if (!_currentTarget) return; // no enemy
            }
            // shoot
            var bullet = Instantiate(projectile, firePoint.position, firePoint.rotation);
            bullet.SetTarget(_currentTarget.TargetPoint, _damageData);
            AudioManager.Instance.PlaySound(fireSound);
        }

        private EnemyController GetEnemy()
        {
            EnemyController enemyController = null;
            HashSet<EnemyController> deleteSet = new HashSet<EnemyController>();
            foreach (var e in Targets)
            {
                if (e == null)
                {
                    deleteSet.Add(e);
                }
            }
            Targets.RemoveWhere(e => deleteSet.Contains(e));
            int armor = int.MinValue;
            foreach (var e in Targets)
            {
                if (e.Armor > armor)
                {
                    enemyController = e;
                    armor = e.Armor;
                }
            }
            return enemyController;
        }

        public void OnEnemyEnter(EnemyController e)
        {
            Targets.Add(e);
        }
        public void OnEnemyExit(EnemyController e)
        {
            Targets.Remove(e);
            if (_currentTarget == e)
                _currentTarget = null;

        }
    }
}

