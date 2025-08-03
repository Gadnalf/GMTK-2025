using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class ParticleController : MonoBehaviour {
    public float linearDamping = 20;
    public float dashSpeed;
    public float dashTime;
    // rate of deceleration while dashing (0.3 = lose 30% of speed per fixedupdate (0.2 seconds))
    public float dashSlowdown = 0.2f;
    public float dodgeTime = 0.3f;
    public float maxAngle = 20;
    public float maxLateralVelocity;
    public float turnRate;
    public float lateralAccelerationRate;
    public float lateralVelocity { get => rb.linearVelocityY; set => rb.linearVelocityY = value; }

    public float forwardAccelerationRate;
    public float forwardVelocity { get => TrackManager.instance.particleVelocity; set => TrackManager.instance.particleVelocity = value; }

    // left and right bounds for particle
    public float particleMinX = -8;
    public float particleMaxX = 3;
    public float horizontalVelocity { get => rb.linearVelocityX; set => rb.linearVelocityX = value; }

    // health
    public float maxHealth = 100;

    // ref
    Rigidbody2D rb;
    public GameObject spriteObject;

    // Lazy unmappable WASD controls
    private static readonly KeyCode UPDASH_KEY = KeyCode.W;
    private static readonly KeyCode UPTURN_KEY = KeyCode.A;
    private static readonly KeyCode DOWNDASH_KEY = KeyCode.S;
    private static readonly KeyCode DOWNTURN_KEY = KeyCode.D;
    private static readonly KeyCode DODGE = KeyCode.Space;

    // Input
    private bool upDash;
    private bool downDash;
    private bool upTurn;
    private bool downTurn;
    private bool dodge;

    // State
    // steer should range between 1 and -1u
    // 1 representing hard left and -1 representing hard right
    private float steering;
    private float previousRotation;
    private float dashTimer;
    private float currentHealth;
    public bool intangible;

    public bool cutsceneDone;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        forwardVelocity = UpgradeManager.instance.GetValue("StartingVelocity");
        maxLateralVelocity = UpgradeManager.instance.GetValue("MaxVelocity");
        lateralAccelerationRate = UpgradeManager.instance.GetValue("AccelerationRate");
        forwardAccelerationRate = UpgradeManager.instance.GetValue("AccelerationRate");
        maxHealth += UpgradeManager.instance.GetValue("ParticleWeight") * 10;
        dashSpeed = UpgradeManager.instance.GetValue("DashSpeed");
        dashTime = UpgradeManager.instance.GetValue("DashTime");
        turnRate = UpgradeManager.instance.GetValue("TurnRate");

        StartCoroutine(IntroCutscene());
    }

    IEnumerator IntroCutscene()
    {
        transform.position = new Vector3(-10, 0, 0);
        cutsceneDone = false;
        for (int i = 0; i < 100; i++)
        {
            transform.position = new Vector3(-10 + ((float) i / 10), 0, 0);
            yield return new WaitForSeconds(0.0025f);
        }
        cutsceneDone = true;
    }

    // Update is called once per frame
    void Update() {
        if (!cutsceneDone) return;
        if (dashTimer <= 0)
        {
            if (Input.GetKeyDown(DODGE))
            {
                dodge = true;
            }

            if (Input.GetKeyDown(UPDASH_KEY))
            {
                upDash = true;
            }
            else if (Input.GetKeyDown(DOWNDASH_KEY))
            {
                downDash = true;
            }

            if (Input.GetKey(UPTURN_KEY))
            {
                upTurn = true;
            }
            else if (Input.GetKey(DOWNTURN_KEY))
            {
                downTurn = true;
            }
        }


        if (intangible) {
            Debug.Log("attempting barrel roll.");
            float targetRotation = spriteObject.transform.eulerAngles.z + 3 * MathF.Sign(steering);
            spriteObject.transform.rotation = Quaternion.Euler(0, 0, targetRotation); // do a barrel roll
        } else {
            float targetRotation = maxAngle * steering;
            float newRotation = Mathf.LerpAngle(previousRotation, targetRotation, Time.deltaTime / Time.fixedDeltaTime);
            spriteObject.transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }
    }

    // this is called in TrackManager.FixedUpdate() before everything else
    public void DoPhysicsStep() {
        // handle dodge
        if (dashTimer <= 0) {
            if (dodge) {
                // dodge is a short burst of speed in the direction the particle is facing
                horizontalVelocity += dashSpeed;
                dashTimer = dodgeTime;
                intangible = true;
            } else {
                intangible = false;
            }
        } 

        // lateral shite first
        if (dashTimer <= 0) {
            if (upTurn) {
                steering += turnRate / 100;
                if (steering > 1) {
                    steering = 1;
                }
            } else if (downTurn) {
                steering -= turnRate / 100;
                if (steering < -1) {
                    steering = -1;
                }
            }
        }

        if (upDash) {
            lateralVelocity = dashSpeed;
            dashTimer = dashTime;
        } else if (downDash) {
            lateralVelocity = -dashSpeed;
            dashTimer = dashTime;
        }

        // apply acceleration and clamp speed if not dashing
        if (dashTimer <= 0) {
            float currentMaxVelocity = maxLateralVelocity * steering;
            lateralVelocity += lateralAccelerationRate * steering * Time.fixedDeltaTime;
            if (lateralVelocity > currentMaxVelocity) {
                lateralVelocity = currentMaxVelocity;
            } else if (lateralVelocity < currentMaxVelocity) {
                lateralVelocity = currentMaxVelocity;
            }
        } else {
            // while dashing, decelerate towards max speed
            if (lateralVelocity > 0) {
                lateralVelocity = Mathf.Lerp(lateralVelocity, maxLateralVelocity, dashSlowdown);
            } else {
                lateralVelocity = Mathf.Lerp(lateralVelocity, -maxLateralVelocity, dashSlowdown);
            }
        }

        // forward acceleration
        forwardVelocity += forwardAccelerationRate * Time.fixedDeltaTime;

        // slingshot towards where health indicates
        float healthRatio = currentHealth / maxHealth;
        float targetHorizontalPosition = healthRatio * -particleMinX + particleMinX;
        // clamp target position to max
        if (targetHorizontalPosition > particleMaxX) {
            targetHorizontalPosition = particleMaxX;
        }
        horizontalVelocity += GetSpringForce(transform.position.x, targetHorizontalPosition, horizontalVelocity, 1.0f, 0.5f, Time.fixedDeltaTime);

        previousRotation = spriteObject.transform.rotation.eulerAngles.z;
        dashTimer -= Time.fixedDeltaTime;
        ClearInput();
    }

    public float GetSpringForce(float current, float target, float velocity, float frequency, float damping, float deltaTime) {
        float omega = 2f * Mathf.PI * frequency;
        float springFactor = omega * omega;
        float dampingFactor = 2f * damping * omega;

        float displacement = current - target;
        float springForce = -springFactor * displacement;
        float dampingForce = -dampingFactor * velocity;

        float acceleration = springForce + dampingForce;
        return acceleration * deltaTime;
    }

    public void Damage(float amount) {
        currentHealth -= amount;
        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }

    private void ClearInput() {
        upDash = false;
        downDash = false;
        upTurn = false;
        downTurn = false;
        dodge = false;
    }
}
