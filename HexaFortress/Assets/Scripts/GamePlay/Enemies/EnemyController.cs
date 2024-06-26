using System;
using System.Collections;
using System.Collections.Generic;
using HexaFortress.Game;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace HexaFortress.GamePlay
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [SerializeField] private Transform targetPoint;

        [FormerlySerializedAs("EnemyDeadPar")] [SerializeField]
        private GameObject enemyDeadParticle;

        [SerializeField] private SkinnedMeshRenderer character;
        [SerializeField, Child] private HealthBar healthBar;

        public EnemyType EnemyType => _myConfig.EnemyType;
        public Transform TargetPoint => targetPoint;
        public int Armor => _currentArmor;

        private EnemyConfig _myConfig;
        private List<Vector3> _pathVectorList;
        private int _pathIndex = -1;
        private Transform _transform;
        private Material _material;
        private int _currentHealth;
        private int _currentArmor;
        private float _currentMoveSpeed;
        private bool _isSlow;
        private float _slowTimer;
        private bool _isDead;
        private float _posY = 0.45f;
        private bool _canPlayFlash = true;
        private Color _baseColor;
        private readonly WaitForSeconds _flashWait = new WaitForSeconds(0.06f);
        private Vector3 _nextPathPosition;
        private const string c_enemydeadparticleId = "enemyDeadPar";

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        public void Init(EnemyConfig config)
        {
            _transform = transform;
            _material = character.material;
            _baseColor = _material.color;
            _myConfig = config;
            _currentHealth = _myConfig.Health;
            _currentArmor = _myConfig.Armor;
            _currentMoveSpeed = _myConfig.MoveSpeed;
            _slowTimer = _myConfig.SlowTime;
            _posY = EnemyManager.Instance.EnemyPosY;
            
            healthBar.Init(_myConfig.Armor > 0);
        }

        private void Update()
        {
            MoveAlongPath();
            if (_isSlow)
                UpdateSlowTimer();
        }

        private void LateUpdate()
        {
            healthBar.LookCamera();
        }

        private void UpdateSlowTimer()
        {
            _slowTimer -= Time.deltaTime;
            if (_slowTimer <= 0)
            {
                _isSlow = false;
                _currentMoveSpeed = _myConfig.MoveSpeed;
            }
        }

        #region Movement

        public void SetMovePosition(List<Vector3> pathVectorList)
        {
            _pathVectorList = pathVectorList;
            _pathIndex = pathVectorList.Count > 0 ? 0 : -1;
            SetNextPosition();
        }

        private void MoveAlongPath()
        {
            if (_pathIndex == -1) return;

            _transform.position = Vector3.MoveTowards(_transform.position, _nextPathPosition,
                Time.deltaTime * _currentMoveSpeed);
            
            if (Vector3.Distance(_transform.position, _nextPathPosition) < _myConfig.ReachedPositionDistance)
            {
                _pathIndex++;
                SetNextPosition();
            }

            if (_pathIndex >= _pathVectorList.Count)
            {
                _pathIndex = -1;
                OnReachedMovePosition();
            }
        }

        private void SetNextPosition()
        {
            if (_pathIndex >= _pathVectorList.Count || _pathIndex < 0)
                return;
            _nextPathPosition = _pathVectorList[_pathIndex];
            _nextPathPosition.y = _posY;
            _transform.LookAt(_nextPathPosition, Vector3.up);
        }

        private void OnReachedMovePosition()
        {
            GameModel.Instance.CastleTile.TakeDamage(_myConfig.DamageToCastle);
            Destroy(gameObject);
        }

        #endregion

        #region Damage

        public void TakeDamage(DamageData damage)
        {
            if (_isDead || (damage.TargetEnemyType & _myConfig.EnemyType) == 0) return;

            if (Random.Range(0, 101) <= damage.CritChance)
            {
                damage.Damage *= 2;
                damage.ArmorDamage *= 2;
            }

            if (Random.Range(0, 101) <= damage.SlowChance)
            {
                SlowDown();
            }

            if (damage.TargetEnemyType == _myConfig.EnemyType)
            {
                damage.Damage = Mathf.FloorToInt(damage.Damage * 1.3f);
            }

            ApplyDamage(damage);
            UpdateHealthBar();

            if (_canPlayFlash)
            {
                StartCoroutine(DamageFlash());
            }
        }

        private void ApplyDamage(DamageData damage)
        {
            if (_currentArmor > 0)
            {
                _currentArmor -= damage.ArmorDamage;
                if (_currentArmor < 0)
                {
                    _currentHealth += _currentArmor; // subtract negative armor from health
                    _currentArmor = 0;
                }
            }
            else
            {
                _currentHealth -= damage.Damage;
            }

            if (_currentHealth <= 0)
            {
                OnDeath();
            }
        }

        private void OnDeath()
        {
            _isDead = true;
            _currentHealth = 0;
            var deadParticle = ObjectPoolingManager.Instance.SpawnObject(c_enemydeadparticleId, enemyDeadParticle,
                targetPoint.position, Quaternion.identity);
            deadParticle.GetComponent<ParticleCallBack>().OnStop = () =>
                ObjectPoolingManager.Instance.ReturnObject(c_enemydeadparticleId, deadParticle);
            Destroy(gameObject);
        }

        private void SlowDown()
        {
            if (_isSlow) return;

            _currentMoveSpeed = _myConfig.MoveSpeed * (1.0f - _myConfig.SlowPercent);
            _slowTimer = _myConfig.SlowTime;
            _isSlow = true;
        }

        private IEnumerator DamageFlash()
        {
            _canPlayFlash = false;
            _material.color = Color.red;
            yield return _flashWait;
            _material.color = _baseColor;
            yield return _flashWait;
            _material.color = Color.red;
            yield return _flashWait;
            _material.color = _baseColor;
            _canPlayFlash = true;
        }

        private void UpdateHealthBar()
        {
            float healthRatio = (float)_currentHealth / _myConfig.Health;
            float armorRatio = _myConfig.Armor > 0 ? (float)_currentArmor / _myConfig.Armor : 0f;
            healthBar.UpdateBar(healthRatio, armorRatio);
        }

        #endregion
    }
}