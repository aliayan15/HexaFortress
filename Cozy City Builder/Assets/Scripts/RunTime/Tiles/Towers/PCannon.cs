using UnityEngine;


public class PCannon : Projectile
{
    private Vector3 control;
    private Vector3 _startPos;
    private float _time = 0;

    public override void SetTarget(Transform target, DamageData damage)
    {
        base.SetTarget(target, damage);
        _startPos = transform.position;
        var dis = target.position - _startPos;
        var controlPos = (dis / 2) + _startPos;
        controlPos.y = _startPos.y + 0.1f;
        control = controlPos;
    }

    protected override void FixedUpdate()
    {
        _time += Time.fixedDeltaTime * speed;
        transform.position = GetPathPos(_time);
        transform.forward = GetPathPos(_time + 0.001f) - transform.position;
        if (_time >= 1)
        {
            Destroy(this);
        }
    }

    private Vector3 GetPathPos(float t)
    {
        Vector3 ac = Vector3.Lerp(_startPos, control, t);
        Vector3 cb = Vector3.Lerp(control, _target.position, t);

        return Vector3.Lerp(ac, cb, t);
    }


    protected override void CheckHit(float distance)
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, distance + 0.25f, rayLayer, QueryTriggerInteraction.Collide))
        {
            if (hitInfo.collider.TryGetComponent(out IDamageable enemy))
                enemy.TakeDamage(_damageData);
            Destroy(this);
            return;
        }
    }
}

