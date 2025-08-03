using UnityEngine;

public class OxygenScript : MonoBehaviour {
    public float maxOffset = 0.5f;

    void Start() {
        // Initialize velocity to be offset
        Vector2 initialVelocity = new Vector2(Random.Range(-maxOffset, maxOffset), Random.Range(-maxOffset, maxOffset));
        GetComponent<EphemeralObject>().velocity = initialVelocity;
    }
}
