using KBCore.Refs;
using Managers;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : TowerTileBase
{
    [HorizontalLine]
    [SerializeField] private float range;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile projectile;


    private Enemy _currentTarget = null;

    

    protected override void OnFire()
    {
        // check distance
        if (_currentTarget)
        {
            Vector3 targetPos = _currentTarget.transform.position;
            targetPos.y = transform.position.y;
            if (Vector3.Distance(transform.position, targetPos) > range)
                _currentTarget = null;
        }
        // get an enemy
        if (!_currentTarget)
        {
            _currentTarget = GetEnemy();
            if (!_currentTarget) return; // no enemy
        }
        // shoot
        var bullet = Instantiate(projectile, firePoint.position, Quaternion.identity);
        bullet.SetTarget(_currentTarget.TargetPoint, _damageData);
        AudioManager.Instance.Play2DSound(SoundTypes.TowerFire);
    }

    private Enemy GetEnemy()
    {
        var cols = Physics.OverlapSphere(transform.position, range, enemyLayer, QueryTriggerInteraction.Collide);
        if (cols == null) return null;
        if (cols.Length == 0) return null;
        foreach (var collider in cols)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                return enemy;
            }
        }
        return null;
    }

#if UNITY_EDITOR
    [SerializeField] private bool drawGizmos = false;
    private void OnDrawGizmos()
    {
        if (drawGizmos)
            Gizmos.DrawSphere(transform.position, range);
    }
#endif
}

