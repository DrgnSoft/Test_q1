using UnityEngine;
using System.Collections;

public class SuperStarBehavior : MonoBehaviour {

    public float moveSpeed = 0.1f;
    public float spinSpeed = 5;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        // self rotation
	    transform.Rotate( Vector3.forward, spinSpeed );

        // moving on its own
        transform.position = transform.position + Vector3.left * moveSpeed;
	}

    void OnBecameInvisible()
    {
        // kill when no longer visible
        Destroy( this.gameObject );
    }
}
