using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grazer : MonoBehaviour {

    public int particleInterval = 3;

    private LinkedList<GameObject> collidedProjectiles = new LinkedList<GameObject>();
    private int particleTimer = 0;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    // iterate and remove null objects
        LinkedListNode<GameObject> node = collidedProjectiles.First;

        while( node != null )
        {
            LinkedListNode<GameObject> next = node.Next;

            if( node.Value == null )
            {
                collidedProjectiles.Remove( node );
            }

            node = next;
        }

        // update timer
        if( particleTimer > 0 )
        {
            particleTimer -= 1;
        }
	}

    void OnCollisionStay2D(Collision2D coll)
    {
        if( particleTimer > 0 )
        {
            return;
        }

        foreach( ContactPoint2D contact in coll.contacts )
        {
            if( contact.collider.gameObject.tag == "Projectile" )
            {
                GameObject particle = (GameObject)Instantiate( Resources.Load( "GrazeParticle" ) );
                particle.transform.position = contact.point;

                particleTimer = particleInterval;
            }
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        // collided with a projectile
        PelletProjectile projectile = coll.gameObject.GetComponent<PelletProjectile>();

        if( projectile != null )
        {
            // check if already collided
            if( !checkAlreadyCollided( coll.gameObject ) )
            {
                // apply bonus points
                FlappyBirdMain.getInstance().incScore();    

                // add to list
                collidedProjectiles.AddLast( coll.gameObject );
            }
        }
    }

    private bool checkAlreadyCollided(GameObject gob)
    {
        foreach( GameObject proj in collidedProjectiles )
        {
            if( proj == gob )
            {
                return true;
            }
        }

        return false;
    }
}
