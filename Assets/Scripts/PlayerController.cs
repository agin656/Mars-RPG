using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Weapon Axe = new Weapon();
        //public string weaponName = "fists";
        //public float damage = 10;
        //public bool melee = true;
        //public float range = 4f;
        //public float cooldown = 1.0f;
        Axe.weaponName = "Axe";
        Axe.damage = 50;
        Axe.cooldown = 1.5f;
        Axe.melee = true;
        Axe.range = 6f;

        gameObject.GetComponent<CharController>().AddWeapon(Axe);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
