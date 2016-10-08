using UnityEngine;
using System.Collections;

public class BerserkerController : MonoBehaviour {

    public CharController berserker;

    // Use this for initialization
    void Start () {
        berserker = gameObject.GetComponent<CharController>();
        berserker.movementSpeed = 5;
    }

	// Update is called once per frame
	void Update () {
        Vector3 lookVector = lookAtPlayer();
        Vector3 movementVector = moveToPlayer();
        berserker.Look(lookVector);
        berserker.Move(movementVector);
    }

    private Vector3 moveToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 result = player.transform.position - berserker.transform.position;
        result.y = 0;
        return result;
    }

    private Vector3 lookAtPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 berserkerPos = gameObject.transform.position;
        Vector3 result = Vector3.MoveTowards(berserkerPos, playerPos, 0.03f);
        result.y = 0;
        return result;
    }
}
