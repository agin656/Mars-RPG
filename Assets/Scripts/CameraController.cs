using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject followThing;
    public float distance = 35.0f;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = followThing.transform.position + new Vector3(0,distance,-(distance/2));
	}
}
