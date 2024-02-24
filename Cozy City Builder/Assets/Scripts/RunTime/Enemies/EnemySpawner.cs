using Managers;
using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
public class EnemySpawner : SingletonMono<EnemySpawner>
{

    private bool isDebug = false;
    private bool _isSpawning;

    private void Update()
    {
    }

    #region Spawn
    private void StartSpawn()
    {
        if (_isSpawning) return;
        _isSpawning = true;
        StartCoroutine(SpawnLoop());
    }
    private IEnumerator SpawnLoop()
    {
        // calculate how many and witch enemies will be spawn
        // spawn every point by order
        yield return null;
    }
    #endregion

    private void OnTurnStateChange(TurnStates state)
    {
        // start spawning
    }

    private void OnGameStateChange(GameStates state)
    {
        // when game over stop spawning
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChange += OnGameStateChange;
        GameManager.OnTurnStateChange += OnTurnStateChange;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChange -= OnGameStateChange;
        GameManager.OnTurnStateChange -= OnTurnStateChange;
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

