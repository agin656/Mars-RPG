using UnityEngine;
using System.Collections;

public class BerserkerBoss : MonoBehaviour
{

    public CharController berserkerBoss;
    public float visionRange = 50;
    private float attackWait;
    private float currentTime;
    private bool attacking = false;
    private Vector3 defaultRotation;

    // Use this for initialization
    void Start()
    {
        berserkerBoss = gameObject.GetComponent<CharController>();
        berserkerBoss.speed = 5;
        berserkerBoss.endurance = 30;
        berserkerBoss.strength = 30;
        berserkerBoss.marksmanship = 0;
        berserkerBoss.faction = 0;
        Weapon axe = new Weapon();
        axe.weaponName = "Axe";
        axe.damage = 20;
        axe.cooldown = 5.0f;
        axe.melee = true;
        axe.range = 10f;
        berserkerBoss.AddWeapon(axe);
        gameObject.GetComponent<Animator>().Play("idleAxe");
        defaultRotation = berserkerBoss.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject God = GameObject.FindGameObjectWithTag("God");
        if (berserkerBoss.getHealth() == 0)
        {
            God.GetComponent<God>().barbAlive = false;
        }
        Vector3 lookVector = lookAtPlayer();
        Vector3 movementVector = moveToPlayer();
        berserkerBoss.Look(lookVector);
        berserkerBoss.Move(movementVector);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != berserkerBoss.faction)
        {
            if (attacking)
            {
                if (Time.time > currentTime + attackWait)
                {
                    berserkerBoss.Attack();
                    attacking = false;
                }
            }
            else if (checkAttack() && berserkerBoss.weapons[berserkerBoss.currentWeapon].isReady())
            {
                currentTime = Time.time;
                attackWait = Random.Range(0.2f, 0.35f);
                attacking = true;
            }
        }
        if (attacking) gameObject.GetComponent<Animator>().Play("readyAxe");
    }

    private Vector3 moveToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != berserkerBoss.faction)
        {
            Vector3 result = player.transform.position - berserkerBoss.transform.position;
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
        if (player.GetComponent<CharController>().faction != berserkerBoss.faction)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 berserkerBossPos = gameObject.transform.position;
            Vector3 result = playerPos - berserkerBossPos;
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
        Vector3 loc = berserkerBoss.transform.position;
        Collider[] enemies = Physics.OverlapSphere(berserkerBoss.transform.position, berserkerBoss.weapons[berserkerBoss.currentWeapon].range);
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
