using UnityEngine;
using System.Collections;

public class AllianceBoss : MonoBehaviour
{

    public CharController allianceBoss;
    public float visionRange = 50;
    private float attackWait;
    private float currentTime;
    private bool attacking = false;
    private Vector3 defaultRotation;

    // Use this for initialization
    void Start()
    {
        allianceBoss = gameObject.GetComponent<CharController>();
        allianceBoss.speed = 10;
        allianceBoss.endurance = 30;
        allianceBoss.strength = 10;
        allianceBoss.marksmanship = 0;
        allianceBoss.faction = 2;
        Weapon fist = new Weapon();
        fist.weaponName = "Fists";
        fist.damage = 5;
        fist.cooldown = 0.9f;
        fist.melee = true;
        fist.range = 4f;
        allianceBoss.AddWeapon(fist);
        defaultRotation = allianceBoss.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookVector = lookAtPlayer();
        Vector3 movementVector = moveToPlayer();
        allianceBoss.Look(lookVector);
        allianceBoss.Move(movementVector);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != allianceBoss.faction)
        {
            if (attacking)
            {
                if (Time.time > currentTime + attackWait)
                {
                    allianceBoss.Attack();
                    attacking = false;
                }
            }
            else if (checkAttack() && allianceBoss.weapons[allianceBoss.currentWeapon].isReady())
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
        if (player.GetComponent<CharController>().faction != allianceBoss.faction)
        {
            Vector3 result = player.transform.position - allianceBoss.transform.position;
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
        if (player.GetComponent<CharController>().faction != allianceBoss.faction)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 allianceBossPos = gameObject.transform.position;
            Vector3 result = playerPos - allianceBossPos;
            result.y = 0;
            if (result.magnitude < 50)
            {
                return result;
            }
        }
        return (defaultRotation);
    }

    private bool checkAttack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 loc = allianceBoss.transform.position;
        Collider[] enemies = Physics.OverlapSphere(allianceBoss.transform.position, allianceBoss.weapons[allianceBoss.currentWeapon].range);
        foreach (Collider enemy in enemies)
        {
            if (enemy.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
}
