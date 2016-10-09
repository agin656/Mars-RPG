﻿using UnityEngine;
using System.Collections;

public class InfantryController : MonoBehaviour
{

    public CharController infantry;
    public float visionRange = 50f;
    private float attackWait;
    private float currentTime;
    private bool attacking = false;
    private Vector3 defaultRotation;
    private Vector3 attackVector;

    // Use this for initialization
    void Start()
    {
        infantry = gameObject.GetComponent<CharController>();
        infantry.speed = 3;
        infantry.endurance = 1;
        infantry.strength = 0;
        infantry.marksmanship = 0;
        Weapon gun = new Weapon();
        gun.weaponName = "Gun";
        gun.damage = 20;
        gun.cooldown = 1.0f;
        gun.melee = false;
        gun.range = 30f;
        infantry.AddWeapon(gun);
        defaultRotation = infantry.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookVector;
        if (!attacking) lookVector = lookAtPlayer();
        else lookVector = attackVector;
        Vector3 movementVector = moveToPlayer();
        infantry.Look(lookVector);
        infantry.Move(movementVector);
        if (attacking)
        {
            if (Time.time > currentTime + attackWait)
            {
                infantry.Attack();
                attacking = false;
            }
        }
        else if (infantry.weapons[infantry.currentWeapon].isReady())
        {
            checkAttack();
        }
    }

    private Vector3 moveToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 result = player.transform.position - infantry.transform.position;
        result.y = 0;
        if (result.magnitude < visionRange && result.magnitude > 0.9 * infantry.weapons[infantry.currentWeapon].range)
        {
            return result;
        }
        else if (result.magnitude < 0.6 * infantry.weapons[infantry.currentWeapon].range)
        {
            return -result;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Vector3 lookAtPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 infantryPos = gameObject.transform.position;
        Vector3 result = playerPos - infantryPos;
        result.y = 0;
        if (result.magnitude < visionRange)
        {
            return result;
        }
        else
        {
            return defaultRotation;
        }
    }

    private void checkAttack()
    {
        RaycastHit hit;
        Vector3 loc = infantry.transform.position;
        if (Physics.SphereCast(infantry.transform.position, 0.5f, infantry.transform.forward, out hit, infantry.weapons[infantry.currentWeapon].range))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                attackVector = infantry.transform.forward;
                attacking = true;
                currentTime = Time.time;
                attackWait = Random.Range(0.3f, 0.5f);
            }
        }
    }
}

