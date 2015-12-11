using UnityEngine;
using System.Collections;

// (simple) singleton class
// holds the core gameplay logic
public class FlappyBirdMain {
    // game data
    private int score = 0;

    // static instance
    private static FlappyBirdMain sInstance = null;

    // private member data
    private GameObject world = null;
    private FlappyBirdScene scene = null;
    private GroundTileHandler groundTiler = null;
    private PillarHandler pillarHandler = null;

    private float powerupIntervalRemaining = 0;

    // states for a simple state machine
    public enum State
    {
        WaitingToStart,
        Running,
        Dead,
        Count,
    };

    // the current game state
    private State gameState = State.WaitingToStart;

    #region PROPERTIES

    public int Score
    {
        get
        {
            return score;
        }
        private set
        {
            score = value;
        }
    }

    public State GameState
    {
        get
        {
            return gameState;
        }
        private set
        {
            gameState = value;
        }
    }

    #endregion PROPERTIES

    // private ctor
    private FlappyBirdMain()
    {
        gameState = State.WaitingToStart;
    }

    // this should be called when the world also starts
    public void Start()
    {
        // grab the current world instance
        world = GameObject.Find( "TheWorld" );
        
        if( world != null )
        {
            scene = world.GetComponent<FlappyBirdScene>();
        }

        groundTiler = new GroundTileHandler();
        pillarHandler = new PillarHandler( scene.minPillarIntervalDist, scene.maxPillarIntervalDist );
    }

    // grabbing the object instance
    public static FlappyBirdMain getInstance()
    {
        if( sInstance == null )
        {
            sInstance = new FlappyBirdMain();
        }

        return sInstance;
    }

    // start the death sequence
    public void reportPlayerDead()
    {
        setState( State.Dead );
    }

    // score handling
    public void incScore(int amount = 1)
    {
        score += amount;

        // update powerup counter
        powerupIntervalRemaining -= amount;
    }
    
    // updates the core game logic for this game type
    public void Update()
    {
        groundTiler.Update();
        pillarHandler.Update();

        // state updates
        // for something with such a simple logic flow, don't need anything fancy.
        //     a switch-based state machine will do just fine
        switch( gameState )
        {
            case State.WaitingToStart:      updateWaitingToStart(); break;
            case State.Running:             updateRunning(); break;
            case State.Dead:                updateDead(); break;
        }
    }

    // state handling
    public void setState(State nextState)
    {
        // state entry
        switch( nextState )
        {
            case State.WaitingToStart:      enterWaitingToStart(); break;
            case State.Running:             enterRunning(); break;
            case State.Dead:                enterDead(); break;
        }

        // set the state
        gameState = nextState;
    }

    #region STATE_LOGIC

    private void enterWaitingToStart()
    {
        // clear projectiles
        destroyByTag( "Projectile" );
        destroyByTag( "Powerup" );

        // clear obstacles
        pillarHandler.clear();

        // reset player
        GameObject player = scene.Player;

        if( player != null )
        {
            // reset player position
            player.transform.position = new Vector3( 0f, 1.5f, 0f );

            // reset orientation
            player.transform.eulerAngles = new Vector3( 0f, 0f, 0f );

            // reset player state
            PlayerBehavior playerBeh = player.GetComponent<PlayerBehavior>();

            if( playerBeh )
            {
                playerBeh.freeze();
            }
        }
    }

    private void updateWaitingToStart()
    {
        // check start game condition..
        if( Input.GetKeyDown( KeyCode.Space ) )
        {
            setState( State.Running );
        }
    }

    private void enterRunning()
    {
        GameObject player = scene.Player;

        if( player != null )
        {
            // let the player start moving
            PlayerBehavior playerBeh = player.GetComponent<PlayerBehavior>();

            if( playerBeh )
            {
                playerBeh.startMoving();
            }
        }

        // reset stats
        score = 0;
        powerupIntervalRemaining = scene.powerupPointInterval;
    }

    private void updateRunning()
    {
        float runSpeed = getRunSpeed();

        // move the world
        groundTiler.move( runSpeed );
        pillarHandler.move( runSpeed );

        // update distance counter
        // check if need to spawn a powerup
        if( powerupIntervalRemaining <= 0f )
        {
            GameObject powerup = (GameObject)Object.Instantiate( Resources.Load( "SuperStar" ) );

            powerupIntervalRemaining = scene.powerupPointInterval;
        }
    }

    private void enterDead()
    {
        GameObject player = scene.Player;

        if( player != null )
        {
            // freeze player
            PlayerBehavior playerBeh = player.GetComponent<PlayerBehavior>();

            if( playerBeh )
            {
                playerBeh.freeze();
            }
        }

        // show points and stuff
    }

    private void updateDead()
    {
        // check restart game condition..
        if( Input.GetKeyDown( KeyCode.Space ) )
        {
            setState( State.WaitingToStart );
        }
    }

    #endregion STATE_LOGIC

    #region HELPERS

    private float getRunSpeed()
    {
        return scene.Player.GetComponent<PlayerBehavior>().horizontalSpeed;
    }

    private void destroyByTag(string tag)
    {
        GameObject [] gobs = GameObject.FindGameObjectsWithTag( tag );

        foreach( GameObject gob in gobs )
        {
            GameObject.Destroy( gob );
        }
    }

    #endregion HELPERS
}


// mainly a wrapper for the FlappyBirdMain, where the real implementation sits
public class FlappyBirdScene : MonoBehaviour {
    public GameObject player;
    public Camera mainCamera;

    // gameplay tweakable data
    public float minPillarIntervalDist = 2f;
    public float maxPillarIntervalDist = 5f;
    public int powerupPointInterval = 15;

    public GameObject Player
    {
         get {
            return player;
         }
    }
    
    public void Start()
    {
        FlappyBirdMain.getInstance().Start();

        // reset the state
        FlappyBirdMain.getInstance().setState( FlappyBirdMain.State.WaitingToStart );
    }
    

	// Update is called once per frame
	void Update () 
    {
        FlappyBirdMain.getInstance().Update();
	}

    void OnGUI()
    {
        float screenWidth = mainCamera.pixelWidth;
        float screenHeight = mainCamera.pixelHeight;

        // show score in the middle-upper part of the screen
        string scoreStr = string.Format( "{0}", FlappyBirdMain.getInstance().Score );

        GUI.Label( new Rect( screenWidth * 0.5f, screenHeight * 0.1f, 200, 50 ), scoreStr );
    }
}
