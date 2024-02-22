using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
public class EnemySpawner : SingletonMono<EnemySpawner>
{

    private bool isDebug = false;

    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
        if (!isDebug) return;
        foreach (var point in TileManager.Instance.EnemySpawnPoints)
        {
            Gizmos.DrawSphere(point, 0.3f);
        }
    }
}

