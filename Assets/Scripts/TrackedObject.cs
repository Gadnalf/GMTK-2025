using UnityEngine;

public class TrackedObject : MonoBehaviour {
    public Vector2 velocity;
    public float spawnPosition;
    public bool isOnScreen = true;
    private Rigidbody2D rb;

    void Start() {
        // currently assumes kinematic rigidbody
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) {
            Debug.LogError("Rigidbody2D component not found on " + gameObject.name);
        }

        TrackManager.instance.trackedObjects.Add(this);
    }

    // frozen objects will not be updated
    public void UpdateObject(Vector2 globalVelocity) {
        rb.linearVelocity = globalVelocity + velocity;
    }

    public void Freeze() {
        isOnScreen = false;
        rb.linearVelocity = Vector2.zero;
    }

    public void Unfreeze() {
        isOnScreen = true;
    }
}
