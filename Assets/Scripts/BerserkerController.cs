using UnityEngine;
using System.Collections;

public class BerserkerController : MonoBehaviour {

    public CharController berserker;
    public float visionRange = 50;
    private float attackWait;
    private float currentTime;
    private bool attacking = false;
    private Vector3 defaultRotation;

    // Use this for initialization
    void Start () {
        berserker = gameObject.GetComponent<CharController>();
        berserker.speed = 5;
        berserker.endurance = 0;
        berserker.strength = 0;
        berserker.marksmanship = 0;
        berserker.faction = 0;
        Weapon axe = new Weapon();
        axe.weaponName = "Axe";
        axe.damage = 8;
        axe.cooldown = 3.0f;
        axe.melee = true;
        axe.range = 5f;
        berserker.AddWeapon(axe);
        gameObject.GetComponent<Animator>().Play("idleAxe");
        defaultRotation = berserker.transform.forward;
    }

	// Update is called once per frame
	void Update () {
        Vector3 lookVector = lookAtPlayer();
        Vector3 movementVector = moveToPlayer();
        berserker.Look(lookVector);
        berserker.Move(movementVector);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != berserker.faction)
        {
            if (attacking)
            {
                if (Time.time > currentTime + attackWait)
                {
                    berserker.Attack();
                    attacking = false;
                }
            }
            else if (checkAttack() && berserker.weapons[berserker.currentWeapon].isReady())
            {
                currentTime = Time.time;
                attackWait = Random.Range(0.2f, 0.35f);
                attacking = true;
            }
        }
        if(attacking) gameObject.GetComponent<Animator>().Play("readyAxe");
    }

    private Vector3 moveToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != berserker.faction)
        {
            Vector3 result = player.transform.position - berserker.transform.position;
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
        if (player.GetComponent<CharController>().faction != berserker.faction) {
            Vector3 playerPos = player.transform.position;
            Vector3 berserkerPos = gameObject.transform.position;
            Vector3 result = playerPos - berserkerPos;
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
        Vector3 loc = berserker.transform.position;
        Collider[] enemies = Physics.OverlapSphere(berserker.transform.position, berserker.weapons[berserker.currentWeapon].range);
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
