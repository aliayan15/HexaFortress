using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PCannon : Projectile
{

    private Vector3 _destination;

    public override void SetTarget(Transform target, DamageData damage)
    {
        base.SetTarget(target, damage);
        _destination = target.position;
    }

    protected override void FixedUpdate()
    {
        var vel = BallisticVelocity(_destination, 0);
        // chack with ray
        float distance = speed * Time.fixedDeltaTime * vel.magnitude;
        CheckHit(distance);
        transform.Translate(speed * Time.fixedDeltaTime * vel, Space.World);
        // lifetime
        lifeTime -= Time.fixedDeltaTime;
        if (lifeTime < 0)
            Destroy();
    }

    private Vector3 BallisticVelocity(Vector3 destination, float angle)
    {
        Vector3 dir = destination - transform.position; // get Target Direction
        float height = dir.y; // get height difference
        dir.y = 0; // retain only the horizontal difference
        float dist = dir.magnitude; // get horizontal direction
        float a = angle * Mathf.Deg2Rad; // Convert angle to radians
        dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
        dist += height / Mathf.Tan(a); // Correction for small height differences

        // Calculate the velocity magnitude
        float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * dir.normalized; // Return a normalized vector.
    }

    protected override void CheckHit(float distance)
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, distance + 0.25f, rayLayer, QueryTriggerInteraction.Collide))
        {
            if (hitInfo.collider.TryGetComponent(out IDamageable enemy))
                enemy.TakeDamage(_damageData);
            Destroy();
            return;
        }
    }
}

