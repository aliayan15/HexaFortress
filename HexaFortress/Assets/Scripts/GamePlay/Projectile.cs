using HexaFortress.Game;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected float speed;
        [SerializeField] protected LayerMask rayLayer;
        [SerializeField] protected float lifeTime = 5f;

        protected DamageData _damageData;
        protected Transform _target;
        public virtual void SetTarget(Transform target, DamageData damage)
        {
            _damageData = damage;
            _target = target;
            transform.LookAt(_target);
        }

        protected virtual void FixedUpdate()
        {
            // chack with ray
            float distance = speed * Time.fixedDeltaTime;
            CheckHit(distance);
            Move(distance);
            // lifetime
            lifeTime -= Time.fixedDeltaTime;
            if (lifeTime < 0)
                Destroy();
        }

        protected virtual void Move(float distance)
        {
            if (_target != null)
                transform.LookAt(_target);
            transform.Translate(transform.forward * distance, Space.World);
        }

        protected virtual void CheckHit(float distance)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, distance + 0.25f, rayLayer, QueryTriggerInteraction.Collide))
            {
                if (hitInfo.collider.TryGetComponent(out IDamageable enemy))
                    enemy.TakeDamage(_damageData);
                Destroy();
                return;
            }
        }

        protected void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
