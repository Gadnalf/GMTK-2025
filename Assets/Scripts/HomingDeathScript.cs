using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class HomingDeathScript : MonoBehaviour {
    public float homingSpeed = 30f;
    private ParticleController player;
    private Rigidbody2D rb;

    void Start() {
        player = GameManager.instance.player; // Get the player reference from the GameManager
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) {
            Debug.LogError("Rigidbody2D component not found on " + gameObject.name);
        }
    }

    // home in on target
    void FixedUpdate() {
        transform.position = new Vector3(transform.position.x, player.transform.position.y, 0);
        rb.linearVelocity = Vector2.left * homingSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Debug.Log("HomingDeathScript: Player hit! Triggering explosion.");
            Explode();
        }
    }

    public void Explode() {
        // Do explosion effects here
        gameObject.SetActive(false);
    }
}