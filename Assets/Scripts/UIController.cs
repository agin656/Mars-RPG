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

    public GameObject statMenu;

    public int availablePoints = 25;

    public Text enduranceVal;
    public Text speedVal;
    public Text strengthVal;
    public Text marksmenshipVal;
    public Text available;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E) && !MainMenuManager.start)
        {
            Debug.Log("poo");
            if (statMenu.active) closeMenu();
            else openMenu();
        }
        if (statMenu.active)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && availablePoints > 0)
            {
                player.endurance++;
                player.maxHealth = 100 + (player.endurance * 10);
                player.Heal(10);
                availablePoints--;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && availablePoints > 0)
            {
                player.speed++;
                availablePoints--;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && availablePoints > 0)
            {
                player.strength++;
                availablePoints--;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && availablePoints > 0)
            {
                player.marksmanship++;
                availablePoints--;
            }
        }
	}

    void OnGUI()
    {
        healthbar.transform.localScale = new Vector3(player.getHealth() / player.maxHealth,1,1);
        setImage(icon1, 0);
        setImage(icon2, 1);
        setOpcities(icon1, player.currentWeapon,0);
        setOpcities(icon2, player.currentWeapon,1);

        enduranceVal.text = "=      " + player.endurance + "      (press '1' to add point)";
        speedVal.text = "=      " + player.speed + "      (press '2' to add point)";
        strengthVal.text = "=      " + player.strength + "      (press '3' to add point)";
        marksmenshipVal.text = "=      "+ player.marksmanship + "      (press '4' to add point)";
        available.text = "STATS           Available Points = "+availablePoints;
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

    void Alert(string message)
    {

    }

    void openMenu()
    {
        statMenu.SetActive(true);
    }
    void closeMenu()
    {
        statMenu.SetActive(false);
    }
}
