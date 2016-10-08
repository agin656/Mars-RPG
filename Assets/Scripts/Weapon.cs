using UnityEngine;
using System.Collections;

public class Weapon {

    public CharController owner;
    public GameObject model;

    public string weaponName = "fists";
    public float damage = 10;
    public bool melee = true;
    public float range = 5;
    public float cooldown = 0.5f;

    private bool ready = true;
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
}
