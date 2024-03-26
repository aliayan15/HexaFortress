using KBCore.Refs;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyType EnemyType => enemyType;
    public int Level => level;
    public Transform TargetPoint => targetPoint;

    [SerializeField] private Transform targetPoint;
    [SerializeField, Child] private HealthBar healthBar;
    [SerializeField] private SOGameProperties data;
    [HorizontalLine]
    [Header("Stats")]
    [SerializeField] private int level;
    [SerializeField] private int health;
    [SerializeField] private int armor;
    [SerializeField] private float moveSpeed;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float reachedPositionDistance = 0.1f;
    [SerializeField] private float slowPercent = 0.3f;
    [SerializeField] private float slowTime = 2f;
    [SerializeField] private short damageToCastle;


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

    private void OnValidate()
    {
        this.ValidateRefs();
    }

    private void Start()
    {
        _transform = transform;
        _currentHealth = health;
        _currentArmor = armor;
        _currentMoveSpeed = moveSpeed;
        _slowTimer = slowTime;
        _posY = EnemySpawner.Instance.EnemyPosY;
        float ratioH = (float)_currentHealth / health;
        float ratioA = 0f;
        if (armor > 0)
            ratioA = (float)_currentArmor / armor;
        healthBar.Init(ratioH + ratioA);
        healthBar.UpdateBar(ratioH, ratioA);
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
            _currentMoveSpeed = moveSpeed;
        }
    }

    #region Movement
    public void SetMovePosition(List<Vector3> pathVectorList)
    {
        _pathVectorList = pathVectorList;
        //if (pathVectorList.Count > 0)
        //{
        //    // Remove first position so he doesn't go backwards
        //    pathVectorList.RemoveAt(0);
        //}
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
            _transform.position = Vector3.MoveTowards(_transform.position, nextPathPosition, Time.deltaTime * _currentMoveSpeed);
            _transform.LookAt(nextPathPosition, Vector3.up);

            if (Vector3.Distance(transform.position, nextPathPosition) < reachedPositionDistance)
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
        GridManager.Instance.PlayerCastle.TakeDamage(damageToCastle);
        Destroy(gameObject);
    }
    #endregion

    #region Damage
    public void TakeDamage(DamageData damage)
    {
        if (_isDead) return;
        int rndCritNum = Random.Range(0, 101);
        if (rndCritNum <= damage.CritChance)
        {
            damage.Damage *= 2;
            damage.ArmorDamage *= 2;
        }


        int rndSlowNum = Random.Range(0, 101);
        if (rndSlowNum <= damage.SlowChance)
            SlowDown();

        if (damage.TypeBonus == enemyType)
            damage.Damage = Mathf.FloorToInt(damage.Damage * 1.3f);

        if (_currentArmor > 0)
            _currentArmor -= damage.ArmorDamage;
        else
            _currentHealth -= damage.Damage;

        if (_currentHealth <= 0)
            Ondead();
        if (_currentArmor < 0)
            _currentArmor = 0;

        float ratioH = (float)_currentHealth / health;
        float ratioA = 0f;
        if (armor > 0)
            ratioA = (float)_currentArmor / armor;
        healthBar.UpdateBar(ratioH, ratioA);
    }

    private void Ondead()
    {
        _isDead = true;
        _currentHealth = 0;
        Instantiate(data.EnemyDeadPar, targetPoint.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void SlowDown()
    {
        if (_isSlow) return;

        _currentMoveSpeed = moveSpeed - (moveSpeed * slowPercent);
        _slowTimer = slowTime;
        _isSlow = true;
    }
    #endregion
}

