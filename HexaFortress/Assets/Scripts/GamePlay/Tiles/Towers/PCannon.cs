using HexaFortress.Game;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class PCannon : Projectile
    {
        [SerializeField] private float radius;
        [SerializeField] private GameObject cannonExplosion;
        private Vector3 control;
        private Vector3 _startPos;
        private Vector3 _endPos;
        private float _time = 0;
        private const string _particalID = "cannonExplosion";

        public override void SetTarget(Transform target, DamageData damage)
        {
            base.SetTarget(target, damage);
            _startPos = transform.position;
            var dis = target.position - _startPos;
            var controlPos = (dis / 2) + _startPos;
            controlPos.y = _startPos.y + 0.1f;
            control = controlPos;
            _endPos = target.position;
        }

        protected override void FixedUpdate()
        {
            if (_target != null)
                _endPos = _target.position;
            _time += Time.fixedDeltaTime * speed;
            transform.position = GetPathPos(_time);
            Vector3 forward = GetPathPos(_time + 0.001f) - transform.position;
            if (forward != Vector3.zero)
                transform.forward = forward;
            if (_time >= 1)
            {
                CheckHit(1f);
            }
        }

        private Vector3 GetPathPos(float t)
        {
            Vector3 ac = Vector3.Lerp(_startPos, control, t);
            Vector3 cb = Vector3.Lerp(control, _endPos, t);

            return Vector3.Lerp(ac, cb, t);
        }

        protected override void CheckHit(float distance)
        {
            var colls = Physics.OverlapSphere(transform.position, radius, rayLayer);
            foreach (var item in colls)
            {
                if (item.TryGetComponent(out IDamageable enemy))
                {
                    enemy.TakeDamage(_damageData);
                }
            }

            var partical = ObjectPoolingManager.Instance.SpawnObject(_particalID, cannonExplosion, transform.position,
                Quaternion.identity);
            partical.GetComponent<ParticleCallBack>().OnStop = delegate
            {
                ObjectPoolingManager.Instance.ReturnObject(_particalID, partical);
            };
            Destroy(gameObject);
        }
    }
}