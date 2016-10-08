using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharController : MonoBehaviour {

    private Rigidbody rb;
    public float movementSpeed = 20;
    private Weapon[] weapons = new Weapon[2];
    public int currentWeapon = 0;
    private float health = 100.0f;
    private bool stunned = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        weapons[0] = new Weapon();
        weapons[0].owner = this;
        weapons[1] = new Weapon();
        weapons[1].owner = this;
        weapons[1].Deactivate();
    }
    void Update()
    {
        if (health <= 0) Die();
    }

    public void Move(Vector3 vector) {
        if(!stunned)
        rb.MovePosition(transform.position + vector.normalized * movementSpeed * Time.deltaTime);
    }
    public void Look(Vector3 loc){
        if (loc == Vector3.zero) return;
        gameObject.transform.LookAt(new Vector3(loc.x,transform.position.y,loc.z));
    }
    public void SwitchWeapon()
    {
        weapons[currentWeapon].Deactivate();
        currentWeapon = (currentWeapon + 1) % 2;
        weapons[currentWeapon].Activate();
    }
    public void Attack()
    {
        weapons[currentWeapon].Attack();
    }
    public void Die()
    {
        Destroy(gameObject);
    }

    public void AddWeapon(Weapon weapon)
    {
        int ind = 0;
        if (weapons[0].weaponName != "fists") ind = 1;
        if (weapons[0].weaponName != "fists") return;

        weapons[ind] = weapon;
        weapons[ind].owner = this;

        if (ind != currentWeapon) weapons[ind].Deactivate();

    }

    public float getHealth()
    {
        return health;
    }
    public void ApplyDamage(float amount)
    {
        health -= amount;
    }
    public void Heal(float amount)
    {
        health += amount;
    }
    public void ApplyKnockback(Vector3 source)
    {
        Vector3 force = gameObject.transform.position - source;
        StartCoroutine("knockback", force.normalized);
    }
    private IEnumerator knockback(Vector3 force)
    {
        stunned = true;
        for (int i = 0; i < 15; i++)
        {
            rb.MovePosition(transform.position + force.normalized * movementSpeed * 1.5f * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        stunned = false;
    }

    private Vector3 DegreeToVector(float deg)
    {
        float radians = Mathf.Deg2Rad * deg;
        return new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians));
    }
}
