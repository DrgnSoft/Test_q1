using UnityEngine;
using System.Collections;

public class TurretBehavior : MonoBehaviour {

    // gameplay tweakable data
    public float shootDistance = 3f;            // game distance unit to the player
    public float chanceToShootBehind = 30f;     // [0:100] percent chance to shoot when behind the player
    public float bulletSpeed = 1f;              // projectile speed

    // private member data
    private GameObject player = null;
    private bool shotFront = false;
    private bool shotBehind = false;

	// Use this for initialization
	void Start() 
    {
	    player = GameObject.Find( "Player" );
	}
	
	// Update is called once per frame
	void Update() 
    {
        if( player != null )
        {
            // distance from the player
            float dist = transform.position.x - player.transform.position.x;

            // check shoot conditions
            if( !shotFront && 
                dist <= shootDistance )
            {
                // shoot at player
                shoot( player.transform.position );

                shotFront = true;
            }
            else if( !shotBehind && 
                     dist <= -shootDistance )
            {
                // check chance to shoot at player
                float chance = Mathf.Clamp( chanceToShootBehind, 0, 100 ) / 100f;

                if( Random.value <= chance )
                {
                    // shoot
                    shoot( player.transform.position );
                }

                shotBehind = true;
            }
        }
	}

    public void shoot(Vector3 targetPos)
    {
        GameObject projectileObj = (GameObject)Object.Instantiate( Resources.Load( "PelletProjectile" ) );
        PelletProjectile proj = projectileObj.GetComponent<PelletProjectile>();

        if( proj != null )
        {
            proj.setParams( transform.position, targetPos - transform.position, bulletSpeed );
        }
    }
}
