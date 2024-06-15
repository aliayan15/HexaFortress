using System.Collections;
using HexaFortress.Game;
using NaughtyAttributes;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class TowerTileBase : TileBase, ITowerUpgradeable
    {
        [Header("Settings")]
        [SerializeField] protected float fireTime;
        [SerializeField] protected int baseDamage;
        [SerializeField] protected int armorDamage;
        [SerializeField] protected int damageUpgrade;
        [SerializeField] protected int armorDamageUpgrade;
        [SerializeField] private int goldExpense;
        [HorizontalLine]
        [SerializeField] protected EnemyType enemyType;
        [SerializeField] protected LayerMask enemyLayer;
        [SerializeField] protected SOGameProperties data;
        [SerializeField] protected GameObject rangeImage;

        private WaitForSeconds _fireTimer;
        private bool _canFire = false;
        protected DamageData _damageData;


        public override void Init(HexGridNode myNode)
        {
            base.Init(myNode);
            _fireTimer = new WaitForSeconds(fireTime);
            _damageData = new DamageData(baseDamage, armorDamage);
            _damageData.TargetEnemyType = enemyType;
            StartCoroutine(FireTimer());
            CheckUpgradeTile();
            if (rangeImage)
                rangeImage.SetActive(false);
            GameModel.Instance.PlayerData.AddExpensesPerDay(goldExpense);
            switch (MyType)
            {
                case TileType.Tower:
                    UpgradeManager.Instance.BacisTowers.Add(this);
                    break;
                case TileType.Cannon:
                    UpgradeManager.Instance.CannonTowers.Add(this);
                    break;
                case TileType.Mortar:
                    UpgradeManager.Instance.MortarTowers.Add(this);
                    break;
            }
        }

        private void CheckUpgradeTile()
        {
            var neighbourGrids = GridManager.Instance.GetSurroundingGrids(_myHexNode);
            foreach (var surroundingTile in neighbourGrids)
            {
                if (!surroundingTile.MyTile)
                    continue;
                if (surroundingTile.MyTile.MyType == TileType.TowerUpgrade)
                    UpgradeDamageTower();
            }
        }

        #region Upgrade
        public void SetEnemyTypeBonus(EnemyType type)
        {
            if (_damageData.TargetEnemyType != EnemyType.None) return;
            _damageData.TargetEnemyType = type;
            PlayPartical();
        }

        public void UpgradeCritEffect()
        {
            _damageData.CritChance += 5;
            PlayPartical();
        }

        private void PlayPartical()
        {
            Player.Instance.PlayPartical(transform.position);
        }

        public void UpgradeSlowEffect()
        {
            _damageData.SlowChance += 5;
            PlayPartical();
        }

        public void UpgradeDamageTower()
        {
            _damageData.Damage += damageUpgrade;
            _damageData.ArmorDamage += armorDamageUpgrade;
            PlayPartical();
        }

        public void UpgradeArmorDamage()
        {
            _damageData.ArmorDamage += 5;
            PlayPartical();
        }
        public void UpgradeHealthDamage()
        {
            _damageData.Damage += 5;
            PlayPartical();
        }
        #endregion

        #region Fire
        protected IEnumerator FireTimer()
        {
            while (true)
            {
                if (_canFire)
                {
                    OnFire();
                }
                yield return _fireTimer;
            }
        }

        protected virtual void OnFire()
        {

        }
        #endregion

        protected override void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            _canFire = evt.TurnState == TurnStates.EnemySpawnStart;
        }
    }
}
