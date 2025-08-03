using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    int comboCount;
    private ParticleController pc;


    void Start()
    {
        pc = GetComponent<ParticleController>();
        comboCount = 0;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (pc.intangible) {
            Debug.Log("Dodged collision with " + collision.gameObject.name);
            return;
        }
        
        // Check if the collided object has a specific tag
        if (collision.gameObject.CompareTag("BasicObstacle")) {
            Debug.Log("Ran into basic obstacle!");
            HandleObstacleCollision(collision.gameObject);
        } else if (collision.gameObject.CompareTag("Booster")) {
            Debug.Log("Ran into Booster!");
            HandleEnterBooster(collision.gameObject);
        }
    }

    void HandleObstacleCollision(GameObject obstacle)
    {
        float obstacleMass = obstacle.GetComponent<ObstacleStats>().collisionMass;
        float particleMass = UpgradeManager.instance.GetValue(UpgradeManager.Upgrades.ParticleWeight);
        Debug.Log("Pre-collision Velocity = " + pc.forwardVelocity);
        pc.forwardVelocity = pc.forwardVelocity * particleMass / (obstacleMass + particleMass); // Reduce velocity by half
        Debug.Log("Particle Mass: " + particleMass + "Obstacle Mass: " + obstacleMass + "Collision Mass Factor = " + particleMass / (obstacleMass + particleMass));
        Debug.Log("Post-collision Velocity = " + pc.forwardVelocity);

        pc.Damage(obstacleMass * 10);
        
        comboCount = 0;
    }

    void HandleEnterBooster(GameObject booster)
    {
        // v0 : just boost velocity
        // We should also boost acceleration, but remove that acceleration when leaving
        float velocityBump = booster.GetComponent<BoosterStats>().boosterVelocityBump;
        float originalVelocity = pc.forwardVelocity;
        float comboMultiplier = 1.0f + 0.2f * comboCount;
        pc.forwardVelocity += velocityBump * comboMultiplier;
        Debug.Log("Velocity boosted from " + originalVelocity + "to " + 
            pc.forwardVelocity + " Delta:(Base: " + velocityBump + ", Combo Multiplier: " + comboMultiplier);
        comboCount += 1;
    }
}