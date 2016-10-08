using UnityEngine;
using System.Collections;

public class StonerController : MonoBehaviour {

    public CharController stoner;
    public float visionRange = 50f;
    private float attackWait;
    private float currentTime;
    private bool attacking = false;
    private Vector3 defaultRotation;

    // Use this for initialization
    void Start () {
        stoner = gameObject.GetComponent<CharController>();
        stoner.movementSpeed = 8;
        Weapon rock = new Weapon();
        rock.weaponName = "Rock";
        rock.damage = 10;
        rock.cooldown = 2.0f;
        rock.melee = true;
        rock.range = 20f;
        stoner.AddWeapon(rock);
        defaultRotation = stoner.transform.forward;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 lookVector = lookAtPlayer();
        Vector3 movementVector = moveToPlayer();
        stoner.Look(lookVector);
        stoner.Move(movementVector);
        if (attacking)
        {
            if (Time.time > currentTime + attackWait)
            {
                stoner.Attack();
                attacking = false;
            }
        }
        else if (checkAttack() && stoner.weapons[stoner.currentWeapon].isReady())
        {
            currentTime = Time.time;
            attackWait = Random.Range(0.2f, 0.35f);
            attacking = true;
        }
    }

    private Vector3 moveToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 result = player.transform.position - stoner.transform.position;
        result.y = 0;
        if (result.magnitude < visionRange && result.magnitude > 0.9 * stoner.weapons[stoner.currentWeapon].range)
        {
            return result;
        }
        else if (result.magnitude < 0.7 * stoner.weapons[stoner.currentWeapon].range)
        {
            return -result;
        } else {
            return Vector3.zero;
        }
    }

    private Vector3 lookAtPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 stonerPos = gameObject.transform.position;
        Vector3 result = playerPos - stonerPos;
        result.y = 0;
        if (result.magnitude < visionRange)
        {
            return result;
        } else
        {
            return defaultRotation;
        }
    }

    private bool checkAttack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        RaycastHit hit;
        Vector3 loc = stoner.transform.position;
        Collider[] enemies = Physics.OverlapSphere(stoner.transform.position, stoner.weapons[stoner.currentWeapon].range);
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
