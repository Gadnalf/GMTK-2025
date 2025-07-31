using System;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class ParticleController : MonoBehaviour {
    public float linearDamping = 20;
    public float dashSpeed = 10.0f;
    public float dashTime = 0.3f;
    // rate of deceleration while dashing (0.3 = lose 30% of speed per fixedupdate (0.2 seconds))
    public float dashSlowdown = 0.2f;
    public float maxAngle = 20;
    public float maxVelocity = 6;
    public float turnRate = 5;
    public float accelerationRate = 0.5f;

    // ref
    Rigidbody2D rb;
    public GameObject spriteObject;

    // Lazy unmappable WASD controls
    private static readonly KeyCode UPDASH_KEY = KeyCode.W;
    private static readonly KeyCode UPTURN_KEY = KeyCode.A;
    private static readonly KeyCode DOWNDASH_KEY = KeyCode.S;
    private static readonly KeyCode DOWNTURN_KEY = KeyCode.D;

    // Input
    private bool upDash;
    private bool downDash;
    private bool upTurn;
    private bool downTurn;

    // State
    // steer should range between 1 and -1
    // 1 representing hard left and -1 representing hard right
    private float steering;
    private float previousRotation;
    private float dashTimer;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (dashTimer <= 0) {
            if (Input.GetKeyDown(UPDASH_KEY)) {
                upDash = true;
            } else if (Input.GetKeyDown(DOWNDASH_KEY)) {
                downDash = true;
            }

            if (Input.GetKey(UPTURN_KEY)) {
                upTurn = true;
            } else if (Input.GetKey(DOWNTURN_KEY)) {
                downTurn = true;
            }
        }
        
        float targetRotation = maxAngle * steering;
        float newRotation = Mathf.LerpAngle(previousRotation, targetRotation, Time.deltaTime / Time.fixedDeltaTime);
        spriteObject.transform.rotation = Quaternion.Euler(0, 0, newRotation);
    }

    void FixedUpdate() {
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
            rb.linearVelocity = Vector2.up * dashSpeed;
            dashTimer = 0.5f;
        } else if (downDash) {
            rb.linearVelocity = Vector2.down * dashSpeed;
            dashTimer = 0.5f;
        }

        // apply acceleration and clamp speed if not dashing
        if (dashTimer <= 0) {
            rb.linearVelocity += Vector2.up * accelerationRate * steering;
            if (rb.linearVelocityY > maxVelocity) {
                rb.linearVelocityY = maxVelocity;
            } else if (rb.linearVelocityY < -maxVelocity) {
                rb.linearVelocityY = -maxVelocity;
            }
        } else {
            // while dashing, lerp towards max speed
            if (rb.linearVelocityY > 0) {
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.up * maxVelocity, dashSlowdown);
            } else {
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.up * -maxVelocity, dashSlowdown);
            }
            
        }

        previousRotation = spriteObject.transform.rotation.eulerAngles.z;
        dashTimer -= Time.fixedDeltaTime;
        ClearInput();
    }

    private void ClearInput() {
        upDash = false;
        downDash = false;
        upTurn = false;
        downTurn = false;
    }
}
