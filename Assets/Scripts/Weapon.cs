using UnityEngine;
using System.Collections;

public class Weapon {

    public CharController owner;
    public GameObject model;

    public string weaponName = "fists";
    public float damage = 10;
    public bool melee = false;
    public float range = 25;
    public float cooldown = 0.1f;

    private bool ready = true;
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

        foreach(Collider enemy in enemies)
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
                    currentTime = Time.time;
                    ready = false;
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

        //I'm keeping all of the commented out stuff for now because while it was buggy, it did solve some problems that I was having before with raycasting
        Vector3 fireDirection = owner.gameObject.transform.forward;
        //RaycastHit[] allHits;
        RaycastHit hit;
        Vector3 loc = owner.gameObject.transform.position;
        //allHits = Physics.SphereCastAll(loc, 0.5f, fireDirection, range);
        //allHits = Physics.RaycastAll(loc, fireDirection, range);
        //allHits = sortHits(allHits);
        /*foreach (var hit in allHits)
        {
            if (hit.transform.position != owner.gameObject.transform.position)
            {
                Debug.Log(hit.collider.tag);
                try
                {
                    hit.rigidbody.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
                }
                catch
                {
                    return;
                }
                hit.rigidbody.gameObject.SendMessage("ApplyKnockback", owner.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
                currentTime = Time.time;
                ready = false;
            }
            if (hit.transform.root != owner.transform.root)
            {
                return;
            }
        }*/
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
                currentTime = Time.time;
                ready = false;
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

    /*private RaycastHit[] sortHits(RaycastHit[] hits)
    {
        int minIndex = 0;
        for (int i = 0; i < hits.Length; i++)
        {
            float min = range;
            for (int j = 0; j < hits.Length; j++)
            {
                var hit = hits[j];
                if (hit.distance < min)
                {
                    min = hit.distance;
                    minIndex = j;
                }
            }
            var current = hits[i];
            var minHit = hits[minIndex];
            hits[i] = minHit;
            hits[minIndex] = minHit;
        }
        return hits;
    }*/
}
