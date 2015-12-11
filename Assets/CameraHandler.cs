using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour {

    public Transform followTarget;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    // we want to update the camera's position later than other objects
    void LateUpdate()
    {
        //transform.position = new Vector3( followTarget.position.x, transform.position.y, transform.position.z );
    }
}
