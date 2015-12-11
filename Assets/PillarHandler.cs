using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PillarHandler {

    private LinkedList<GameObject> pillars = new LinkedList<GameObject>();

    private float minPillarIntervalDist = 0f;
    private float maxPillarIntervalDist = 0f;
    private float distToSpawn = 0f;

    // ctor
    public PillarHandler(float _minPillarIntervalDist, float _maxPillarIntervalDist)
    {
        minPillarIntervalDist = _minPillarIntervalDist;
        maxPillarIntervalDist = _maxPillarIntervalDist;
    }
	
	// Update is called once per frame
	public void Update() 
    {
	    flush();
	}

    public void move(float dist)
    {
        // move all pillars
        foreach( GameObject pillar in pillars )
        {
            if( pillar != null )
            {
                pillar.transform.position = new Vector3( pillar.transform.position.x - dist,
                                                         pillar.transform.position.y,
                                                         pillar.transform.position.z );
            }
        }

        // update dist to spawn counter
        distToSpawn -= dist;

        // moved far enough..
        if( distToSpawn <= 0f )
        {
            spawn();

            // reset counter
            distToSpawn = Random.Range( minPillarIntervalDist, maxPillarIntervalDist );
        }
    }

    public void clear()
    {
        // clear all instances
        foreach( GameObject pillar in pillars )
        {
            GameObject.Destroy( pillar );
        }

        pillars.Clear();
    }

    public void spawn()
    {
        GameObject newPillar = (GameObject)Object.Instantiate( Resources.Load( "Pillar" ) );
        pillars.AddLast( newPillar );
    }

    // pillars remove themselves from the world, so we just need to check for null here
    public void flush()
    {
        // build the list of pillars to remove
        List< LinkedListNode<GameObject> > toRemove = new List< LinkedListNode<GameObject> >();

        for( LinkedListNode<GameObject> node = pillars.First; node != null && node.Next != null; node = node.Next )
        {
            if( node.Value == null )
            {
                toRemove.Add( node );
            }
        }

        // remove them
        foreach( LinkedListNode<GameObject> node in toRemove )
        {
            pillars.Remove( node );
        }
    }
}
