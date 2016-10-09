﻿using UnityEngine;
using System.Collections;

public class AssassinController : MonoBehaviour
{

    public CharController assassin;
    public float visionRange = 50;
    private float attackWait;
    private float currentTime;
    private bool attacking = false;
    private Vector3 defaultRotation;

    // Use this for initialization
    void Start()
    {
        assassin = gameObject.GetComponent<CharController>();
        assassin.speed = 17;
        assassin.endurance = 3;
        assassin.strength = 0;
        assassin.marksmanship = 0;
        Weapon sword = new Weapon();
        sword.weaponName = "Sword";
        sword.damage = 20;
        sword.cooldown = 1.0f;
        sword.melee = true;
        sword.range = 4f;
        assassin.AddWeapon(sword);
        defaultRotation = assassin.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookVector = lookAtPlayer();
        Vector3 movementVector = moveToPlayer();
        assassin.Look(lookVector);
        assassin.Move(movementVector);
        if (attacking)
        {
            if (Time.time > currentTime + attackWait)
            {
                assassin.Attack();
                attacking = false;
            }
        }
        else if (checkAttack() && assassin.weapons[assassin.currentWeapon].isReady())
        {
            currentTime = Time.time;
            attackWait = Random.Range(0.2f, 0.35f);
            attacking = true;
        }
    }

    private Vector3 moveToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 result = player.transform.position - assassin.transform.position;
        result.y = 0;
        if (result.magnitude < visionRange)
        {
            return result;
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
        Vector3 assassinPos = gameObject.transform.position;
        Vector3 result = playerPos - assassinPos;
        result.y = 0;
        if (result.magnitude < 50)
        {
            return result;
        }
        else
        {
            return (defaultRotation);
        }

    }

    private bool checkAttack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 loc = assassin.transform.position;
        Collider[] enemies = Physics.OverlapSphere(assassin.transform.position, assassin.weapons[assassin.currentWeapon].range);
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
