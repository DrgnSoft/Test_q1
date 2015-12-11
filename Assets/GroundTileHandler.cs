using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GroundTileHandler {

    private LinkedList<GameObject> tiles = new LinkedList<GameObject>();
    private GameObject mainCamera = null;

    // ctor
    public GroundTileHandler()
    {
        mainCamera = GameObject.Find( "Main Camera" );

        // initial fill
        fill();
    }

    public void Update()
    {
        flush();
        fill();
    }

    public void move(float dist)
    {
        // move all tiles
        foreach( GameObject tile in tiles )
        {
            tile.transform.position = new Vector3( tile.transform.position.x - dist, 
                                                   tile.transform.position.y, 
                                                   tile.transform.position.z );
        }
    }

    // helpers
    public void fill()
    {
        // tile fill going from left to right
        float screenWidth = mainCamera.GetComponent<Camera>().orthographicSize * 2f;

        // iterate through the current tiles and find the left-most and right-most span
        float leftSpan = float.MaxValue;
        float rightSpan = float.MinValue;

        if( tiles.Count != 0 )
        {
            foreach( GameObject tile in tiles )
            {
                // tile is still alive..
                if( tile != null )
                {
                    // check left-most position
                    if( tile.transform.position.x < leftSpan )
                    {
                        leftSpan = tile.transform.position.x;
                    }

                    // check right-most position
                    // apply the sprite width since the pivot point is to the left
                    SpriteRenderer sprite = tile.GetComponent<SpriteRenderer>();
                    float rightPos = sprite != null ? tile.transform.position.x + sprite.sprite.bounds.size.x
                                                    : tile.transform.position.x;

                    if( rightPos > rightSpan )
                    {
                        rightSpan = rightPos;
                    }
                }
            }
        }
        else
        {
            // no tiles created yet, so treat it as the initial fill
            leftSpan = -screenWidth;
            rightSpan = -screenWidth;
        }

        // start filling from the right-most point
        float currentXPos = rightSpan;

        while( currentXPos < screenWidth )
        {
            // create and add a tile
            GameObject newTile = (GameObject)Object.Instantiate( Resources.Load( "GroundTile" ) );
            tiles.AddLast( newTile );

            // set its position
            newTile.transform.position = new Vector3( currentXPos,
                                                      newTile.transform.position.y,
                                                      newTile.transform.position.z );

            // increment
            currentXPos += newTile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
    }

    // removes offscreen tiles
    public void flush()
    {
        // build the list of tiles to remove
        List< LinkedListNode<GameObject> > toRemove = new List< LinkedListNode<GameObject> >();
        
        for( LinkedListNode<GameObject> node = tiles.First; node.Next != null; node = node.Next )
        {
            TileBehavior tileBeh = node.Value.GetComponent<TileBehavior>();

            if( tileBeh == null ||      // something horrible happened
                tileBeh.isOffscreen )   // tile went offscreen
            {
                toRemove.Add( node );
            }
        }

        // remove them
        foreach( LinkedListNode<GameObject> node in toRemove )
        {
            if( node.Value != null )
            {
                Object.Destroy( node.Value );
            }

            tiles.Remove( node );
        }
    }
}
