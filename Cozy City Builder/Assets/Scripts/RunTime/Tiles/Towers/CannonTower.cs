using Managers;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CannonTower : TowerTileBase
{
    [HorizontalLine]
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform muzzle;
    [Space(5)]
    [SerializeField] protected Projectile projectile;
    [SerializeField] protected SoundTypes fireSound;

    private Enemy _currentTarget = null;
    [HideInInspector]
    public HashSet<Enemy> Targets = new HashSet<Enemy>();

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

    private Enemy GetEnemy()
    {
        Enemy enemy = null;
        HashSet<Enemy> deleteSet = new HashSet<Enemy>();
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
                enemy = e;
                armor = e.Armor;
            }
        }
        return enemy;
    }

    public void OnEnemyEnter(Enemy e)
    {
        Targets.Add(e);
    }
    public void OnEnemyExit(Enemy e)
    {
        Targets.Remove(e);
        if (_currentTarget == e)
            _currentTarget = null;

    }
}

