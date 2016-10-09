using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    private CharController player;
    public GameObject healthbar;

    public Sprite fist;
    public Sprite sword;
    public Sprite axe;
    public Sprite rock;
    public Sprite star;
    public Sprite gun;

    public Image icon1;
    public Image icon2;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        healthbar.transform.localScale = new Vector3(player.getHealth() / player.maxHealth,1,1);
        setImage(icon1, 0);
        setImage(icon2, 1);
        setOpcities(icon1, player.currentWeapon,0);
        setOpcities(icon2, player.currentWeapon,1);
    }

    void setOpcities(Image icon, int curWeapon, int iconNum)
    {
        if(curWeapon == iconNum)
        {
            Color tmp = icon.transform.parent.GetComponent<Image>().color;
            tmp.a = 1;
            icon.transform.parent.GetComponent<Image>().color = tmp;
        }else
        {
            Color tmp = icon.transform.parent.GetComponent<Image>().color;
            tmp.a = 0.5f;
            icon.transform.parent.GetComponent<Image>().color = tmp;
        }
    }

    void setImage(Image icon, int slotNum)
    {
        switch (player.weapons[slotNum].weaponName)
        {
            case "Fists":
                icon.sprite = fist;
                break;
            case "Sword":
                icon.sprite = sword;
                break;
            case "Axe":
                icon.sprite = axe;
                break;
            case "Rock":
                icon.sprite = rock;
                break;
            case "Shuriken":
                icon.sprite = star;
                break;
            case "Gun":
                icon.sprite = gun;
                break;
        }
    }
}
