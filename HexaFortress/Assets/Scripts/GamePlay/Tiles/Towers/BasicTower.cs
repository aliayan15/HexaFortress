using Managers;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicTower : TowerTileBase
{
    [HorizontalLine]
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected Transform rangeDir;
    [SerializeField] protected float range;
    [SerializeField] protected bool useAllAxisForMuzzle = true;
    [SerializeField] protected bool isBasicTower = false;
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
            if (isBasicTower)
                _currentTarget = GetEnemyBasic();
            else
                _currentTarget = GetEnemyMortar();
            if (!_currentTarget) return; // no enemy
        }
        // shoot
        var bullet = Instantiate(projectile, firePoint.position, firePoint.rotation);
        bullet.SetTarget(_currentTarget.TargetPoint, _damageData);
        AudioManager.Instance.PlaySound(fireSound);
    }

    // get closest enemy
    private Enemy GetEnemyBasic()
    {
        var cols = Physics.OverlapSphere(transform.position, range, enemyLayer, QueryTriggerInteraction.Collide);
        if (cols == null) return null;
        if (cols.Length == 0) return null;
        float dis = float.MaxValue;
        int index = -1;
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].TryGetComponent(out Enemy enemy))
            {
                if ((enemy.EnemyType & enemyType) == 0) continue;
                Vector3 toOther = Vector3.Normalize(enemy.transform.position - transform.position);
                if (Vector3.Dot(rangeDir.forward, toOther) < 0)
                    continue;
                float enemyDis = Vector3.Distance(transform.position, enemy.transform.position);
                if (enemyDis < dis)
                {
                    index = i;
                    dis = enemyDis;
                }
            }
        }
        if (index == -1) return null;
        else return cols[index].GetComponent<Enemy>();
    }
    // get in the mid
    private Enemy GetEnemyMortar()
    {
        var cols = Physics.OverlapSphere(transform.position, range, enemyLayer, QueryTriggerInteraction.Collide);
        var colsList = cols.ToList();
        if (colsList == null) return null;
        if (colsList.Count == 0) return null;
        for (int i = 0; i < colsList.Count; i++)
        {
            if (colsList[i].TryGetComponent(out Enemy enemy))
            {
                if ((enemy.EnemyType & enemyType) == 0)
                {
                    colsList.RemoveAt(i);
                    i--;
                }
            }
        }
        if (colsList.Count == 0) return null;

        int index = colsList.Count;
        if (index % 2 == 1)
        {
            index--;
        }
        index /= 2;
        index = Mathf.Min(index, colsList.Count - 1);
        index = Mathf.Max(index, 0);
        return colsList[index].GetComponent<Enemy>();
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

