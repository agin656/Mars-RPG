using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharController : MonoBehaviour {

    private Rigidbody rb;
    public float movementSpeed = 20;
    private GameObject[] weapons = new GameObject[2];
    public int currentWeapon = 0;
    

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void Move(Vector3 vector) {
        rb.MovePosition(transform.position + vector.normalized * movementSpeed * Time.deltaTime);
    }
    public void Look(Vector3 loc){
        if (loc == Vector3.zero) return;
        gameObject.transform.LookAt(new Vector3(loc.x,transform.position.y,loc.z));
    }
    public void SwitchWeapon()
    {
        currentWeapon = (currentWeapon + 1) % 2;
    }
    public void Attack()
    {
        weapons[currentWeapon].SendMessage("Attack", SendMessageOptions.DontRequireReceiver);
    }


    private Vector3 DegreeToVector(float deg)
    {
        float radians = Mathf.Deg2Rad * deg;
        return new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians));
    }
}
