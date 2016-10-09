using UnityEngine;
using System.Collections;

public class NinjaBoss : MonoBehaviour
{

    public CharController ninjaBoss;
    public float visionRange = 50;
    private float attackWait;
    private float currentTime;
    private bool attacking = false;
    private Vector3 defaultRotation;

    // Use this for initialization
    void Start()
    {
        ninjaBoss = gameObject.GetComponent<CharController>();
        ninjaBoss.speed = 15;
        ninjaBoss.endurance = 20;
        ninjaBoss.strength = 10;
        ninjaBoss.marksmanship = 0;
        ninjaBoss.faction = 1;
        Weapon sword = new Weapon();
        sword.weaponName = "Sword";
        sword.damage = 20;
        sword.cooldown = 1.0f;
        sword.melee = true;
        sword.range = 4f;
        ninjaBoss.AddWeapon(sword);
        defaultRotation = ninjaBoss.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookVector = lookAtPlayer();
        Vector3 movementVector = moveToPlayer();
        ninjaBoss.Look(lookVector);
        ninjaBoss.Move(movementVector);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != ninjaBoss.faction)
        {
            if (attacking)
            {
                if (Time.time > currentTime + attackWait)
                {
                    ninjaBoss.Attack();
                    attacking = false;
                }
            }
            else if (checkAttack() && ninjaBoss.weapons[ninjaBoss.currentWeapon].isReady())
            {
                currentTime = Time.time;
                attackWait = Random.Range(0.2f, 0.35f);
                attacking = true;
            }
        }
    }

    private Vector3 moveToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != ninjaBoss.faction)
        {
            Vector3 result = player.transform.position - ninjaBoss.transform.position;
            result.y = 0;
            if (result.magnitude < visionRange)
            {
                return result;
            }
        }
        return Vector3.zero;
    }

    private Vector3 lookAtPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != ninjaBoss.faction)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 ninjaBossPos = gameObject.transform.position;
            Vector3 result = playerPos - ninjaBossPos;
            result.y = 0;
            if (result.magnitude < 50 && player.GetComponent<CharController>().faction != ninjaBoss.faction)
            {
                return result;
            }
        }
        return (defaultRotation);
    }

    private bool checkAttack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 loc = ninjaBoss.transform.position;
        Collider[] enemies = Physics.OverlapSphere(ninjaBoss.transform.position, ninjaBoss.weapons[ninjaBoss.currentWeapon].range);
        foreach (Collider enemy in enemies)
        {
            if (enemy.gameObject.CompareTag("Player") && player.GetComponent<CharController>().faction != ninjaBoss.faction)
            {
                return true;
            }
        }
        return false;
    }
}
