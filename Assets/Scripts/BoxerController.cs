﻿using UnityEngine;
using System.Collections;

public class BoxerController : MonoBehaviour
{

    public CharController boxer;
    public float visionRange = 50;
    private float attackWait;
    private float currentTime;
    private bool attacking = false;
    private Vector3 defaultRotation;

    // Use this for initialization
    void Start()
    {
        boxer = gameObject.GetComponent<CharController>();
        boxer.speed = 13;
        boxer.endurance = -3;
        boxer.strength = 0;
        boxer.marksmanship = 0;
        boxer.faction = 2;
        Weapon fist = new Weapon();
        fist.weaponName = "Fists";
        fist.damage = 3;
        fist.cooldown = 1.0f;
        fist.melee = true;
        fist.range = 2.5f;
        boxer.AddWeapon(fist);
        defaultRotation = boxer.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookVector = lookAtPlayer();
        Vector3 movementVector = moveToPlayer();
        boxer.Look(lookVector);
        boxer.Move(movementVector);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<CharController>().faction != boxer.faction)
        {
            if (attacking)
            {
                if (Time.time > currentTime + attackWait)
                {
                    boxer.Attack();
                    attacking = false;
                }
            }
            else if (checkAttack() && boxer.weapons[boxer.currentWeapon].isReady())
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
        if (player.GetComponent<CharController>().faction != boxer.faction)
        {
            Vector3 result = player.transform.position - boxer.transform.position;
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
        if (player.GetComponent<CharController>().faction != boxer.faction)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 boxerPos = gameObject.transform.position;
            Vector3 result = playerPos - boxerPos;
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
        Vector3 loc = boxer.transform.position;
        Collider[] enemies = Physics.OverlapSphere(boxer.transform.position, boxer.weapons[boxer.currentWeapon].range);
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
