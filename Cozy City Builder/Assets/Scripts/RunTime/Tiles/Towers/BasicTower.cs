using Managers;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : TowerTileBase
{
    [HorizontalLine]
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected float range;
    [SerializeField] protected bool useAllAxisForMuzzle = true;
    [SerializeField] protected bool useHalfCircle = false;
    [Space(5)]
    [SerializeField] protected Projectile projectile;
    [SerializeField] protected SoundTypes fireSound;


    private Enemy _currentTarget = null;

    protected virtual void Update()
    {
        if (!_currentTarget) return;

        if (useAllAxisForMuzzle)
            muzzle.LookAt(_currentTarget.transform.position);
        else
        {
            muzzle.LookAt(_currentTarget.transform.position);
            var ros = muzzle.rotation.eulerAngles;
            ros.x = 0;
            ros.z = 0;
            muzzle.rotation = Quaternion.Euler(ros);
        }
    }

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
        var bullet = Instantiate(projectile, firePoint.position, firePoint.rotation);
        bullet.SetTarget(_currentTarget.TargetPoint, _damageData);
        AudioManager.Instance.Play2DSound(fireSound);
    }

    private Enemy GetEnemy()
    {
        var cols = Physics.OverlapSphere(transform.position, range, enemyLayer, QueryTriggerInteraction.Collide);
        if (cols == null) return null;
        if (cols.Length == 0) return null;
        float dis = float.PositiveInfinity;
        int index = -1;
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].TryGetComponent(out Enemy enemy))
            {
                if ((enemy.EnemyType & enemyType) == 0) continue;
                if (useHalfCircle)
                {
                    Vector3 toOther = Vector3.Normalize(enemy.transform.position - transform.position);
                    if (Vector3.Dot(transform.right, toOther) < 0)
                        continue;
                }
                float disTotower = Vector3.Distance(enemy.transform.position, transform.position);
                if (disTotower < dis)
                {
                    index = i;
                    dis = disTotower;
                }
            }
        }
        if (index == -1) return null;
        else return cols[index].GetComponent<Enemy>();
    }

#if UNITY_EDITOR
    [SerializeField] protected bool drawGizmos = false;
    private void OnDrawGizmos()
    {
        if (drawGizmos)
            Gizmos.DrawSphere(transform.position, range);
    }
#endif
}

