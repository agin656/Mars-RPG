using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public CharController player;
    public float look;
    public float move;

    private bool moving = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 moveVector = getMoveVector();
        if (moving)player.Move(moveVector);

        player.Look(getLookLocation());

        if (Input.GetKeyDown(KeyCode.Mouse1)) player.SwitchWeapon();

        if (Input.GetKeyDown(KeyCode.Mouse0)) player.Attack();
    }

    private Vector3 getMoveVector(){
        float x = 0;
        float y = 0;
        if (Input.GetKey(KeyCode.S)) y -= 1;
        if (Input.GetKey(KeyCode.W)) y += 1;
        if (Input.GetKey(KeyCode.D)) x += 1;
        if (Input.GetKey(KeyCode.A)) x -= 1;


        if (x != 0 || y != 0) moving = true;
        else moving = false;

        return new Vector3(x, 0, y);
    }

    private Vector3 getLookLocation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            Vector3 vector = hit.point - player.transform.position;
            vector.y = 0;
            return vector;
        }
        return Vector3.zero;
    }

}
