using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public CharController owner;
    public GameObject model;

    public string weaponName = "fists";
    public float damage = 10;
    public bool melee = true;
    public float range = 5;
    public float cooldown = 0.5f;

    private bool ready = true;
    private bool active = true;

    public void Attack()
    {
        if (!active) return;
        if (!ready) return;
        Collider[] enemies = Physics.OverlapSphere(owner.gameObject.transform.position, range);

        foreach(Collider enemy in enemies)
        {
            if (enemy.gameObject.transform.root != owner.transform.root)
            {
                enemy.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
                enemy.gameObject.SendMessage("ApplyKnockback", owner.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
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

    private IEnumerator Cooldown()
    {
        ready = false;
        yield return new WaitForSeconds(cooldown);
        ready = true;
    }
}
