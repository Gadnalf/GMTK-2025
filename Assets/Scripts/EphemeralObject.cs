using UnityEngine;

public class EphemeralObject : MonoBehaviour {
    public Vector2 velocity;
    private Rigidbody2D rb;

    void Start() {
        // currently assumes kinematic rigidbody
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) {
            Debug.LogError("Rigidbody2D component not found on " + gameObject.name);
        }

        TrackManager.instance.ephemeralObjects.Add(this);
    }

    // frozen objects will not be updated
    public void UpdateObject(Vector2 globalVelocity) {
        rb.linearVelocity = globalVelocity + velocity;
    }
}
