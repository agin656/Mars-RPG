using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

    public GameObject intro;
    public GameObject selection;
    public GameObject selectionText;
    public GameObject canvas;
    public GameObject ui;

    public int faction = -1;

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

        if(faction >=0 )
        {
            canvas.SetActive(false);
            ui.SetActive(true);
            //GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>().faction = faction;
        }
    }
}
