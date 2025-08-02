using UnityEngine;

public class particleCollisionHandler : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has a specific tag
        if (collision.gameObject.CompareTag("BasicObstacle"))
        {
            Debug.Log("Ran into basic obstacle!");
            TrackManager.instance.particleVelocity /= 2; // Reduce velocity by half
            Debug.Log("Current Particle Velocity = " + TrackManager.instance.particleVelocity);
            if (TrackManager.instance.particleVelocity < 0.1f)
            {
                //destory the game object if the velocity is too low    
                Debug.Log("Particle Velocity too low, destroying object.");
                GameObject.Destroy(gameObject);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
    }

    void OnCollisionStay2D(Collision2D collision)
    {

    }

    void OnCollisionExit2D(Collision2D collision)
    {

    }
}