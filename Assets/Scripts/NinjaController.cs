﻿using UnityEngine;
using System.Collections;

public class NinjaController : MonoBehaviour
{

    public CharController ninja;
    public float visionRange = 50f;
    private float attackWait;
    private float currentTime;
    private bool attacking = false;
    private Vector3 defaultRotation;
    private Vector3 attackVector;

    // Use this for initialization
    void Start()
    {
        ninja = gameObject.GetComponent<CharController>();
        ninja.speed = 10;
        ninja.endurance = -8;
        ninja.strength = 0;
        ninja.marksmanship = 0;
        ninja.faction = 1;
        Weapon shuriken = new Weapon();
        shuriken.weaponName = "Shuriken";
        shuriken.damage = 4;
        shuriken.cooldown = 3.0f;
        shuriken.melee = false;
        shuriken.range = 10f;
        ninja.AddWeapon(shuriken);
        defaultRotation = ninja.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookVector;
        if (!attacking) lookVector = lookAtPlayer();
        else lookVector = attackVector;
        Vector3 movementVector = moveToPlayer();
        ninja.Look(lookVector);
        ninja.Move(movementVector);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != ninja.faction)
        {
            if (attacking)
            {
                if (Time.time > currentTime + attackWait)
                {
                    ninja.Attack();
                    attacking = false;
                }
            }
            else if (ninja.weapons[ninja.currentWeapon].isReady())
            {
                checkAttack();
            }
        }
    }

    private Vector3 moveToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != ninja.faction)
        {
            Vector3 result = player.transform.position - ninja.transform.position;
            result.y = 0;
            if (result.magnitude < visionRange && result.magnitude > 0.75 * ninja.weapons[ninja.currentWeapon].range)
            {
                return result;
            }
            else if (result.magnitude < 0.55 * ninja.weapons[ninja.currentWeapon].range)
            {
                return -result;
            }
        }
        return Vector3.zero;
    }

    private Vector3 lookAtPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != ninja.faction)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 ninjaPos = gameObject.transform.position;
            Vector3 result = playerPos - ninjaPos;
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
        Vector3 loc = ninja.transform.position;
        if (Physics.SphereCast(ninja.transform.position, 0.5f, ninja.transform.forward, out hit, ninja.weapons[ninja.currentWeapon].range))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                attackVector = ninja.transform.forward;
                attacking = true;
                currentTime = Time.time;
                attackWait = Random.Range(0.3f, 0.5f);
            }
        }
    }
}

