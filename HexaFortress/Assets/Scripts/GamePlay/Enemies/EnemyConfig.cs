﻿using HexaFortress.Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace HexaFortress.GamePlay
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObject/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [FormerlySerializedAs("EnemyPrefab")] public EnemyController enemyControllerPrefab;
        public int Level;
        public int Health;
        public int Armor;
        public float MoveSpeed;
        public EnemyType EnemyType;
        public float ReachedPositionDistance = 0.1f;
        public float SlowPercent = 0.3f;
        public float SlowTime = 2f;
        public short DamageToCastle;
    }
}