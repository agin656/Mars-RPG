using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

    public GameObject intro;
    public GameObject selection;
    public GameObject selectionText;
    public GameObject canvas;
    public GameObject ui;

    public Vector3[] spawns = new Vector3[3];

    public int faction = -1;

    static public bool start = true;

	// Use this for initialization
	void Start () {
        ui.SetActive(false);
        canvas.SetActive(true);
        intro.SetActive(true);
        selection.SetActive(false);
        selectionText.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (start)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                intro.SetActive(false);
                selection.SetActive(true);
                selectionText.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                intro.SetActive(false);
                selection.SetActive(false);
                selectionText.SetActive(false);
                faction = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                intro.SetActive(false);
                selection.SetActive(false);
                selectionText.SetActive(false);
                faction = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                intro.SetActive(false);
                selection.SetActive(false);
                selectionText.SetActive(false);
                faction = 1;
            }

            if (faction >= 0)
            {
                Go();
                start = false;
            }
        }
    }
    private void Go()
    {
        canvas.SetActive(false);
        ui.SetActive(true);
        CharController cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>();
        cc.faction = faction;
        cc.gameObject.transform.position = spawns[faction];

        Weapon melee = new Weapon();
        Weapon range = new Weapon();

        switch (faction)
        {
            case 0:
                range.weaponName = "Rock";
                range.damage = 10;
                range.cooldown = 2.0f;
                range.melee = false;
                range.range = 20f;
                melee.weaponName = "Axe";
                melee.damage = 20;
                melee.cooldown = 2.0f;
                melee.melee = true;
                melee.range = 6f;
                break;
            case 1:
                range.weaponName = "Shuriken";
                range.damage = 13;
                range.cooldown = 1.0f;
                range.melee = false;
                range.range = 10f;
                melee.weaponName = "Sword";
                melee.damage = 20;
                melee.cooldown = 1.0f;
                melee.melee = true;
                melee.range = 4f;
                break;
            case 2:
                range.weaponName = "Gun";
                range.damage = 20;
                range.cooldown = 1.0f;
                range.melee = false;
                range.range = 22;
                melee.weaponName = "Fists";
                melee.damage = 10;
                melee.cooldown = 0.5f;
                melee.melee = true;
                melee.range = 4f;
                break;
        }

        cc.AddWeapon(melee);
        cc.AddWeapon(range);
    }
}
