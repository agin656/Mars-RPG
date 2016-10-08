using UnityEngine;
using System.Collections;

public class Weapon {

    public CharController owner;
    public GameObject model;

    public string weaponName = "fists";
    public float damage = 10;
    public bool melee = true;
    public float range = 4f;
    public float cooldown = 1.0f;

    public bool ready = true;
    private bool active = true;
    private float currentTime = 0f;

    public void Attack()
    {
        if(!melee)
        {
            Shoot();
            return;
        }
        if (!active) return;
        if (!ready)
        {
            if (Time.time - currentTime > cooldown)
            {
                ready = true;
            }
            else
            {
                return;
            }
        }
        Collider[] enemies = Physics.OverlapSphere(owner.gameObject.transform.position, range);

        currentTime = Time.time;
        ready = false;
        foreach (Collider enemy in enemies)
        {
            if (enemy.gameObject.transform.root != owner.transform.root)
            {
                Vector3 enemyLocation = enemy.gameObject.transform.position;
                Vector3 ownerLocation = owner.gameObject.transform.position;
                Vector3 relativeVector = enemyLocation - ownerLocation;
                Vector3 facing = owner.gameObject.transform.forward;
                float angle = Vector3.Angle(facing, relativeVector);
                if (angle < 90)
                {
                    enemy.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
                    enemy.gameObject.SendMessage("ApplyKnockback", owner.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    public void Shoot()
    {
        if (!active) return;
        if (!ready)
        {
            if (Time.time - currentTime > cooldown)
            {
                ready = true;
            }
            else
            {
                return;
            }
        }
        Vector3 fireDirection = owner.gameObject.transform.forward;
        RaycastHit hit;
        Vector3 loc = owner.gameObject.transform.position;
        currentTime = Time.time;
        ready = false;
        if (Physics.SphereCast(loc, 0.5f, fireDirection, out hit, range))
        {
            if (hit.transform.position != owner.gameObject.transform.position)
            {
                try
                {
                    hit.rigidbody.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
                }
                catch
                {
                    return;
                }
                hit.rigidbody.gameObject.SendMessage("ApplyKnockback", owner.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
            }
            if (hit.transform.root != owner.transform.root)
            {
                return;
            }
        }
    }

    public void Deactivate()
    {
        if (model != null) model.SetActive(false);
        active = false;
    }

    public void Activate()
    {
        if (model != null) model.SetActive(true);
        active = true;
    }

    public bool isReady()
    {
        if (!ready)
        {
            if (Time.time - currentTime > cooldown)
            {
                ready = true;
            }
            else
            {
                return false;
            }
        }
        return ready;
    }
}
