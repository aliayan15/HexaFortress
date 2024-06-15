using System.Collections;
using System.Collections.Generic;
using HexaFortress.Game;
using KBCore.Refs;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace HexaFortress.GamePlay
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Transform targetPoint;
        [FormerlySerializedAs("EnemyDeadPar")] [SerializeField] private GameObject enemyDeadPartical;
        [SerializeField] private SkinnedMeshRenderer charater;
        [SerializeField, Child] private HealthBar healthBar;

        public EnemyType EnemyType => _myConfig.EnemyType;
        public int Level => _myConfig.Level;
        public Transform TargetPoint => targetPoint;
        public int Armor => _currentArmor;
        
        private EnemyConfig _myConfig;
        private List<Vector3> _pathVectorList;
        private int _pathIndex = -1;
        private Transform _transform;
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

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        public void Init(EnemyConfig config)
        {
            _myConfig = config;
            _transform = transform;
            _currentHealth = _myConfig.Health;
            _currentArmor = _myConfig.Armor;
            _currentMoveSpeed = _myConfig.MoveSpeed;
            _slowTimer = _myConfig.SlowTime;
            _posY = EnemyManager.Instance.EnemyPosY;
            float ratioH = (float)_currentHealth / _myConfig.Health;
            float ratioA = 0f;
            if (_myConfig.Armor > 0)
                ratioA = (float)_currentArmor / _myConfig.Armor;
            healthBar.Init(ratioH + ratioA);
            healthBar.UpdateBar(ratioH, ratioA);
            _baseColor = charater.material.color;
        }

        private void Update()
        {
            Move();
            healthBar.LookCamera();
            if (_isSlow)
                SlowTimer();
        }

        private void SlowTimer()
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
            if (pathVectorList.Count > 0)
            {
                _pathIndex = 0;
            }
            else
            {
                // no path
                _pathIndex = -1;
            }
        }

        private void Move()
        {
            if (_pathIndex != -1)
            {
                // Move to next path position
                Vector3 nextPathPosition = _pathVectorList[_pathIndex];
                nextPathPosition.y = _posY;
                _transform.position = Vector3.MoveTowards(_transform.position, nextPathPosition,
                    Time.deltaTime * _currentMoveSpeed);
                _transform.LookAt(nextPathPosition, Vector3.up);

                if (Vector3.Distance(_transform.position, nextPathPosition) < _myConfig.ReachedPositionDistance)
                {
                    _pathIndex++;
                    if (_pathIndex >= _pathVectorList.Count)
                    {
                        // End of path
                        _pathIndex = -1;
                        onReachedMovePosition();
                    }
                }
            }
        }

        private void onReachedMovePosition()
        {
            // damage to castle
            GameModel.Instance.CastleTile.TakeDamage(_myConfig.DamageToCastle);
            Destroy(gameObject);
        }

        #endregion

        #region Damage

        public void TakeDamage(DamageData damage)
        {
            if (_isDead) return;
            if ((damage.TargetEnemyType & _myConfig.EnemyType) == 0) return;

            int rndCritNum = Random.Range(0, 101);
            if (rndCritNum <= damage.CritChance)
            {
                damage.Damage *= 2;
                damage.ArmorDamage *= 2;
            }


            int rndSlowNum = Random.Range(0, 101);
            if (rndSlowNum <= damage.SlowChance)
                SlowDown();

            if (damage.TargetEnemyType == _myConfig.EnemyType)
                damage.Damage = Mathf.FloorToInt(damage.Damage * 1.3f);

            if (_currentArmor > 0)
                _currentArmor -= damage.ArmorDamage;
            else
                _currentHealth -= damage.Damage;

            if (_currentHealth <= 0)
                Ondead();
            if (_currentArmor < 0)
                _currentArmor = 0;

            float ratioH = (float)_currentHealth / _myConfig.Health;
            float ratioA = 0f;
            if (_myConfig.Armor > 0)
                ratioA = (float)_currentArmor / _myConfig.Armor;
            healthBar.UpdateBar(ratioH, ratioA);
            if (_canPlayFlash)
                StartCoroutine(DamageFlash());
        }

        private void Ondead()
        {
            _isDead = true;
            _currentHealth = 0;
            string particalID = "enemyDeadPar";
            var deadPartical = ObjectPoolingManager.Instance.SpawnObject(particalID, enemyDeadPartical,
                targetPoint.position, Quaternion.identity);
            deadPartical.GetComponent<ParticalCallBack>().OnStop = delegate
            {
                ObjectPoolingManager.Instance.ReturnObject(particalID, deadPartical);
            };
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
            charater.material.color = Color.red;
            yield return _flashWait;
            charater.material.color = _baseColor;
            yield return _flashWait;
            charater.material.color = Color.red;
            yield return _flashWait;
            charater.material.color = _baseColor;
            _canPlayFlash = true;
        }

        #endregion
    }
}