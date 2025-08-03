using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public ParticleController player;
    public HomingDeathScript homingDeathParticle;
    public float deathDelay = 1f; // Delay before returning to upgrade scene after death
    public float winTimer = 30f; // how many seconds before a particle slams into you violently
    public float moneyEarned = 0;
    public float moneyMultiplierOnWin = 1.5f; // Multiplier for money earned on win
    public float crashParticleOffset = 12f; // Offset for the crash particle spawn position
    public TMPro.TextMeshProUGUI timeToImpactDisplay;
    private static readonly string TIME_TO_IMPACT_TEXT_FORMAT = "T-MINUS {0:0.0} SECONDS TO IMPACT";
    private bool isGameOver = false;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        if (isGameOver) return; // Prevent further updates if the game is over

        if (winTimer < 0) {
            winTimer = 0;
            isGameOver = true;
            SpawnCrashParticle();
        } else {
            winTimer -= Time.fixedDeltaTime;
        }
        timeToImpactDisplay.text = string.Format(TIME_TO_IMPACT_TEXT_FORMAT, winTimer);
    }

    public void DoFailState() {
        UpgradeManager.instance.money += (int) moneyEarned;
        StartCoroutine(LoadLevelAfterDelay(deathDelay)); // Load upgrades scene after a 1 second delay
    }

    public void DoWinState() {
        UpgradeManager.instance.money += (int) (moneyEarned * moneyMultiplierOnWin);
        StartCoroutine(LoadLevelAfterDelay(deathDelay)); // Load upgrades scene after a 1 second delay
    }

    private void SpawnCrashParticle() {
        homingDeathParticle.enabled = true;
        homingDeathParticle.transform.position = new Vector3(crashParticleOffset, player.transform.position.y, 0);
    }

    IEnumerator LoadLevelAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("UpgradesScene");
    }
}

    
