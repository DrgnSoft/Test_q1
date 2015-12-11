using UnityEngine;
using System.Collections;

public class PelletProjectile : MonoBehaviour {
    // tweakable data
    public float spinSpeed = 1f;

    // private member data
    private Vector3 moveDir;

	// Use this for initialization
	void Start () 
    {
	}

    // init parameters
    public void setParams(Vector3 position, Vector3 direction, float speed)
    {
        transform.position = position;
        moveDir = direction.normalized * speed;
    }
	
	// Update is called once per frame
	void Update () 
    {
        // self rotation
	    transform.Rotate( Vector3.forward, spinSpeed );

        // update motion
        transform.position = transform.position + moveDir;
	}
    
    void OnBecameInvisible()
    {
        // kill when no longer visible
        Destroy( this.gameObject );
    }
}
