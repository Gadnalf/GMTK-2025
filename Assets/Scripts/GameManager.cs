using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public float deathDelay = 1f; // Delay before returning to upgrade scene after death

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void DoFailState() {
        // This method is called when the player wants to return to the upgrade scene
        // It can be triggered by a button or any other event in the game
        StartCoroutine(LoadLevelAfterDelay(deathDelay)); // Load upgrades scene after a 1 second delay
    }

    IEnumerator LoadLevelAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("UpgradesScene");
    }
}

    
