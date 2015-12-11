using UnityEngine;
using System.Collections;

public class PillarBehavior : MonoBehaviour {
    // tweakable values
    public float minHeight = 1.8f;
    public float maxHeight = 5f;

    // private member data
    private bool scoreReported = false;

	// Use this for initialization
	void Start () 
    {
	    // randomly choose the pillar build
        bool topPillar = Random.value > 0.5f;
        float height = Mathf.Lerp( minHeight, maxHeight, Random.value );
        float yPos = 0f;

        GameObject cameraObj = GameObject.Find( "Main Camera" );
        float screenSize = cameraObj.GetComponent<Camera>().orthographicSize;

        if( topPillar )
        {
            yPos = screenSize - height;

            // flip
            transform.localScale = new Vector3( transform.localScale.x, -transform.localScale.y, transform.localScale.z );
        }
        else
        {
            yPos = height - screenSize;
        }

        // set the final position
        transform.position = new Vector3( transform.position.x, yPos, 0f );
	}
	
	// Update is called once per frame
	void Update () 
    {
	    // check for scoring condition
        if( !scoreReported && 
            transform.position.x <= 0f )
        {
            FlappyBirdMain.getInstance().incScore();

            scoreReported = true;
        }
	}

    void OnBecameInvisible()
    {
        // went offscreen .. destroy myself
        Destroy( this.gameObject );
    }

}
