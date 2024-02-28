using System.Collections;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask rayLayer;
    [SerializeField] private float lifeTime = 5f;

    private DamageData _damageData;
    private Transform _target;
    public void SetTarget(Transform target, DamageData damage)
    {
        _damageData = damage;
        _target = target;
        transform.LookAt(_target);
    }

    private void FixedUpdate()
    {
        // chack with ray
        float distance = speed * Time.fixedDeltaTime;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, distance + 0.25f, rayLayer, QueryTriggerInteraction.Collide))
        {
            if (hitInfo.collider.TryGetComponent(out IDamageable enemy))
                enemy.TakeDamage(_damageData);
            Destroy();
            return;
        }
        // move
        if (_target != null)
            transform.LookAt(_target);
        transform.Translate(transform.forward * distance, Space.World);
        // lifetime
        lifeTime -= Time.fixedDeltaTime;
        if (lifeTime < 0)
            Destroy();
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
