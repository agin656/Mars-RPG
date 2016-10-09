using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Weapon Axe = new Weapon();
        Axe.weaponName = "Axe";
        Axe.damage = 50;
        Axe.cooldown = 1.5f;
        Axe.melee = true;
        Axe.range = 6f;

        gameObject.GetComponent<CharController>().AddWeapon(Axe);

        Weapon Gun = new Weapon();
        Gun.weaponName = "Gun";
        Gun.damage = 50;
        Gun.cooldown = 1.0f;
        Gun.melee = false;
        Gun.range = 30f;

        gameObject.GetComponent<CharController>().AddWeapon(Gun);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
