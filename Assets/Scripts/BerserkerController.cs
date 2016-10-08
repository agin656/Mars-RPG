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
        berserker.movementSpeed = 10;
        Weapon axe = new Weapon();
        axe.weaponName = "Axe";
        axe.damage = 20;
        axe.cooldown = 1.5f;
        axe.melee = true;
        axe.range = 6f;
        berserker.AddWeapon(axe);
        defaultRotation = berserker.transform.forward;
    }

	// Update is called once per frame
	void Update () {
        Vector3 lookVector = lookAtPlayer();
        Vector3 movementVector = moveToPlayer();
        berserker.Look(lookVector);
        berserker.Move(movementVector);
        Debug.Log(movementVector);
        if (attacking)
        {
            if(Time.time > currentTime + attackWait)
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

    private Vector3 moveToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 result = player.transform.position - berserker.transform.position;
        result.y = 0;
        if (result.magnitude < visionRange)
        {
            return result;
        } else
        {
            return Vector3.zero;
        }
    }

    private Vector3 lookAtPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 berserkerPos = gameObject.transform.position;
        Vector3 result = playerPos - berserkerPos;
        result.y = 0;
        if(result.magnitude < 50)
        {
            return result;
        } else
        {
            return (defaultRotation);
        }
        
    }

    private bool checkAttack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        RaycastHit hit;
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
