using UnityEngine;

public class particleCollisionHandler : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has a specific tag
        if (collision.gameObject.CompareTag("BasicObstacle"))
        {
            Debug.Log("Ran into basic obstacle!");
            HandleObstacleCollision(collision.gameObject);
        }
    }

    void HandleObstacleCollision(GameObject obstacle)
    {
        float obstacleMass = obstacle.GetComponent<ObstacleStats>().collisionMass;
        float particleMass = UpgradeManager.instance.GetValue(UpgradeManager.Upgrades.ParticleWeight);
        Debug.Log("Pre-collision Velocity = " + TrackManager.instance.particleVelocity);
        TrackManager.instance.particleVelocity = TrackManager.instance.particleVelocity * particleMass / (obstacleMass + particleMass); // Reduce velocity by half
        Debug.Log("Particle Mass: " + particleMass + "Obstacle Mass: " + obstacleMass + "Collision Mass Factor = " + particleMass / (obstacleMass + particleMass));
        Debug.Log("Post-collision Velocity = " + TrackManager.instance.particleVelocity);
        if (TrackManager.instance.particleVelocity < 0.1f)
        {
            //destory the game object if the velocity is too low    
            Debug.Log("Particle Velocity too low, destroying object.");
            GameObject.Destroy(gameObject);
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