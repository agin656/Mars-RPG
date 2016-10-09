using UnityEngine;
using System.Collections;

public class Weapon {

    public CharController owner;

    public string weaponName = "Fists";
    public float damage = 10;
    public bool melee = true;
    public float range = 4f;
    public float cooldown = 1.0f;

    public bool ready = true;
    private bool active = true;
    private float currentTime = 0f;

    public void Attack()
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

        owner.gameObject.GetComponent<Animator>().Play("attack"+weaponName);

        if (!melee)
        {
            Shoot();
            return;
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
                    damage += (owner.strength / 2);
                    enemy.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
                    enemy.gameObject.SendMessage("ApplyKnockback", owner.weapons[owner.currentWeapon], SendMessageOptions.DontRequireReceiver);
                    damage -= (owner.strength / 2);
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
                    damage += (owner.marksmanship / 2);
                    hit.rigidbody.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
                    damage -= (owner.marksmanship / 2);
                }
                catch
                {
                    return;
                }
                damage += (owner.marksmanship / 2);
                hit.rigidbody.gameObject.SendMessage("ApplyKnockback", owner.weapons[owner.currentWeapon], SendMessageOptions.DontRequireReceiver);
                damage -= (owner.marksmanship / 2);
            }
            if (hit.transform.root != owner.transform.root)
            {
                return;
            }
        }
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
