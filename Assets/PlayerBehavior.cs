using UnityEngine;
using System.Collections;


// player character logic
public class PlayerBehavior : MonoBehaviour {
    public Camera mainCamera; // lazy!

    // gameplay tweakable data
    public float horizontalSpeed = 0.8f;
    public float flapImpulse = 9f;

    // internal tweakables
    private float maxRiseAngle = 45f;
    private float maxFallAngle = -90f;
    private float maxRiseSpeed = 5f;
    private float maxFallSpeed = -25f;

    // game state
    private bool alive = true;

    private bool invulnerable = false;
    private int invulnerableTimer = 0;

    private GameObject grazer = null;

    // properties
    public bool isAlive
    {
        get {
            return alive;
        } 
    }

    void Awake()
    {
    }

	// Use this for initialization
	void Start() 
    {
        // allocate the grazer
        grazer = (GameObject)Instantiate( Resources.Load( "Grazer" ) );
	}

    public void freeze()
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.Sleep();

        alive = false;
    }

    public void startMoving()
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

        rigidBody.WakeUp();

        alive = true;
    }

    public void setInvulnerable(bool set, int duration)
    {
        invulnerable = set;
        invulnerableTimer = duration;

        // set the alpha and collider correspondingly
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Collider2D collider = GetComponent<Collider2D>();
        
        if( invulnerable )
        {
            sprite.color = new Color( sprite.color.r, 
                                      sprite.color.g, 
                                      sprite.color.b, 
                                      0.1f );

            collider.enabled = false;
        }
        else
        {
            sprite.color = new Color( sprite.color.r,
                                      sprite.color.g,
                                      sprite.color.b,
                                      1f );

            collider.enabled = true;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

        // check input
        if( Input.GetKeyDown( KeyCode.Space ) )
        {
            if( alive )
            {
                // true to flappy bird physics, apply the impulse and ignore the current downward velocity
                rigidBody.velocity = new Vector2( 0f, flapImpulse );
            }
        }

        if( alive )
        {
            // do simple rotation
            // (linear interpolation based on fall speed)
            float ySpeed = rigidBody.velocity.y;

            // calculate the parametric value
            float t = Mathf.Clamp01( (ySpeed - maxFallSpeed) / (maxRiseSpeed - maxFallSpeed) );
            float newAngle = Mathf.Lerp( maxFallAngle, maxRiseAngle, t );

            transform.eulerAngles = new Vector3( 0f, 0f, newAngle );
        }

        // clamp vertical movement
        transform.position = new Vector3( transform.position.x, 
                                          Mathf.Clamp( transform.position.y, -mainCamera.orthographicSize, mainCamera.orthographicSize ), 
                                          transform.position.z );

        // sync the grazer
        // *!* this is kinda flakey
        grazer.transform.position = transform.position;
        
        // update timers
        if( invulnerableTimer > 0 )
        {
            invulnerableTimer -= 1;

            if( invulnerableTimer <= 0 )
            {
                setInvulnerable( false, 0 );
            }
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        

        // report the hit to the world
        FlappyBirdMain.getInstance().reportPlayerDead();

        alive = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // check for powerup
        bool isSuperStar = other.gameObject.GetComponent<SuperStarBehavior>() != null;

        if( isSuperStar )
        {
            // do invulnerability
            setInvulnerable( true, 300 );

            // kill the powerup
            Destroy( other.gameObject );
        }
    }
}
