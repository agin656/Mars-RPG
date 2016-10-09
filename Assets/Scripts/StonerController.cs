using UnityEngine;
using System.Collections;

public class StonerController : MonoBehaviour {

    public CharController stoner;
    public float visionRange = 50f;
    private float attackWait;
    private float currentTime;
    private bool attacking = false;
    private Vector3 defaultRotation;
    private Vector3 attackVector;

    // Use this for initialization
    void Start () {
        stoner = gameObject.GetComponent<CharController>();
        stoner.speed = 3;
        stoner.endurance = 3;
        stoner.strength = 0;
        stoner.marksmanship = 0;
        stoner.faction = 0;
        Weapon rock = new Weapon();
        rock.weaponName = "Rock";
        rock.damage = 10;
        rock.cooldown = 2.0f;
        rock.melee = false;
        rock.range = 20f;
        stoner.AddWeapon(rock);
        defaultRotation = stoner.transform.forward;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 lookVector;
        if (!attacking) lookVector = lookAtPlayer();
        else lookVector = attackVector;
        Vector3 movementVector = moveToPlayer();
        stoner.Look(lookVector);
        stoner.Move(movementVector);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != stoner.faction)
        {
            if (attacking)
            {
                if (Time.time > currentTime + attackWait)
                {
                    stoner.Attack();
                    attacking = false;
                }
            }
            else if (stoner.weapons[stoner.currentWeapon].isReady())
            {
                checkAttack();
            }
        }
    }

    private Vector3 moveToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != stoner.faction)
        {
            Vector3 result = player.transform.position - stoner.transform.position;
            result.y = 0;
            if (result.magnitude < visionRange && result.magnitude > 0.8 * stoner.weapons[stoner.currentWeapon].range)
            {
                return result;
            }
            else if (result.magnitude < 0.5 * stoner.weapons[stoner.currentWeapon].range)
            {
                return -result;
            }
        }
        return Vector3.zero;
    }

    private Vector3 lookAtPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != stoner.faction)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 stonerPos = gameObject.transform.position;
            Vector3 result = playerPos - stonerPos;
            result.y = 0;
            if (result.magnitude < visionRange)
            {
                return result;
            }
        }
        return defaultRotation;
    }

    private void checkAttack()
    {
        RaycastHit hit;
        Vector3 loc = stoner.transform.position;
        if (Physics.SphereCast(stoner.transform.position, 0.5f, stoner.transform.forward, out hit, stoner.weapons[stoner.currentWeapon].range)) {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                attackVector = stoner.transform.forward;
                attacking = true;
                currentTime = Time.time;
                attackWait = Random.Range(0.2f, 0.35f);
            }
        }
    }
}
