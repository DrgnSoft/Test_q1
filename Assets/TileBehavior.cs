using UnityEngine;
using System.Collections;

public class TileBehavior : MonoBehaviour {

    private bool offscreen = false;

    public bool isOffscreen
    {
        get {
            return offscreen;
        }
    }

    void OnBecameInvisible()
    {
        // not necessarily true that this is called when the object goes offscreen..
        // but works fine for what's needed here
        offscreen = true;
    }
}
