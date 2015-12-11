using UnityEngine;
using System.Collections;

// cause we need some kind of visual feedback..
public class GrazeParticleBehavior : MonoBehaviour {

    // tweakable data
    public float minLinearSpeed = 0.01f;
    public float maxLinearSpeed = 0.03f;
    public float minSpinSpeed = 3f;
    public float maxSpinSpeed = 9f;

    public int duration = 20;
    public int fadeAwayTime = 10;

    // private member data
    private Vector3 dir;
    private float speed;
    private float spinSpeed;
    private int lifetime = 0;

    void Start()
    {
        // direction
        Vector2 dir2 = Random.insideUnitCircle;
        dir = new Vector3( dir2.x, dir2.y, 0f );

        // speeds
        speed = Random.Range( minLinearSpeed, maxLinearSpeed );
        spinSpeed = Random.Range( minSpinSpeed, maxSpinSpeed );
    }
    
	// Update is called once per frame
	void Update() 
    {
        int maxDuration = duration + fadeAwayTime;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        // rotate
        transform.Rotate( Vector3.forward, spinSpeed );

        // move
        transform.position = transform.position + dir * speed;
        
        if( lifetime >= duration )
        {
            // start fading
            int fadeTime = lifetime - duration;
            float alpha = Mathf.Clamp01( fadeTime / fadeAwayTime );

            sprite.color = new Color( sprite.color.r, sprite.color.g, sprite.color.b, alpha );
        }

        // exceeded max lifetime
        if( lifetime >= maxDuration )
        {
            // die
            Destroy( this.gameObject );
        }

        lifetime += 1;
	}
}
