using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharController : MonoBehaviour {

    private Rigidbody rb;
    private Animator animator;
    public float movementSpeed = 20;
    public Weapon[] weapons = new Weapon[2];
    public int currentWeapon = 0;
    public float maxHealth = 100.0f;
    private float health = 100.0f;
    private bool stunned = false;
    private bool moving = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        weapons[0] = new Weapon();
        weapons[0].owner = this;
        weapons[1] = new Weapon();
        weapons[1].owner = this;
    }
    void Update()
    {
        if (health <= 0) Die();
    }

    public void Move(Vector3 vector) {
        if (!stunned && vector != Vector3.zero)
        {
            rb.MovePosition(transform.position + vector.normalized * movementSpeed * Time.deltaTime);
            animator.Play("Walk");
        }
    }
    public void Look(Vector3 dirVector){
        if (dirVector != Vector3.zero)
        gameObject.transform.rotation = Quaternion.LookRotation(dirVector);
    }
    public void SwitchWeapon()
    {
        currentWeapon = (currentWeapon + 1) % 2;
        animator.Play("idle"+ weapons[currentWeapon].weaponName);
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
        if (weapons[0].weaponName != "Fists") ind = 1;
        if (weapons[1].weaponName != "Fists") return;


        weapons[ind] = weapon;
        weapons[ind].owner = this;

        animator.Play("idle" + weapons[currentWeapon].weaponName);

    }

    public float getHealth()
    {
        return health;
    }
    public void ApplyDamage(float amount)
    {
        health -= amount;
        if (health < 0) health = 0;
    }
    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }
    public void ApplyKnockback(Weapon weapon)
    {
        StartCoroutine(knockback(weapon));
    }
    private IEnumerator knockback(Weapon weapon)
    {
        stunned = true;
        Vector3 ownerPos = weapon.owner.gameObject.transform.position;
        Vector3 targetPos = gameObject.transform.position;
        Vector3 force = targetPos - ownerPos;
        for (int i = 0; i < 10; i++)
        {
            if (weapon.melee)
            {
                rb.MovePosition(transform.position + force.normalized * Mathf.Sqrt(movementSpeed) * Mathf.Sqrt(weapon.damage) * 2f * Time.deltaTime);
            } else
            {
                rb.MovePosition(transform.position + force.normalized * Mathf.Sqrt(movementSpeed) * Mathf.Sqrt(weapon.damage) * Time.deltaTime);
            }
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
