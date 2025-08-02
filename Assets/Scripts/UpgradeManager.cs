using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {
    public static UpgradeManager instance;

    public static class Upgrades {
        public const string StartingVelocity = "StartingVelocity";
        public const string MaxVelocity = "MaxVelocity";
        public const string AccelerationRate = "AccelerationRate";
        public const string ParticleWeight = "ParticleWeight";
        public const string DashSpeed = "DashSpeed";
        public const string DashTime = "DashTime";
        public const string TurnRate = "TurnRate";
    }

    private Dictionary<string, float> baseValues = new Dictionary<string, float> {
        {Upgrades.StartingVelocity, 2},
        {Upgrades.MaxVelocity, 6},
        {Upgrades.AccelerationRate, 0.5f},
        {Upgrades.ParticleWeight, 1},
        {Upgrades.DashSpeed, 10},
        {Upgrades.DashTime, 0.3f},
        {Upgrades.TurnRate, 5}
    };

    private Dictionary<string, float> upgradeIncrements = new Dictionary<string, float> {
        {Upgrades.StartingVelocity, 0.1f},
        {Upgrades.MaxVelocity, 1},
        {Upgrades.AccelerationRate, 0.1f},
        {Upgrades.ParticleWeight, 1},
        {Upgrades.DashSpeed, 1},
        {Upgrades.DashTime, 0.1f},
        {Upgrades.TurnRate, 1}
    };

    private Dictionary<string, int> maxLevels = new Dictionary<string, int> {
        {Upgrades.StartingVelocity, 10},
        {Upgrades.MaxVelocity, 10},
        {Upgrades.AccelerationRate, 5},
        {Upgrades.ParticleWeight, 5},
        {Upgrades.DashSpeed, 5},
        {Upgrades.DashTime, 5},
        {Upgrades.TurnRate, 3}
    };

    private Dictionary<string, int> curLevels = new Dictionary<string, int> {
        {Upgrades.StartingVelocity, 0},
        {Upgrades.MaxVelocity, 0},
        {Upgrades.AccelerationRate, 0},
        {Upgrades.ParticleWeight, 0},
        {Upgrades.DashSpeed, 0},
        {Upgrades.DashTime, 0},
        {Upgrades.TurnRate, 0}
    };

    private Dictionary<string, int> upgradeCost = new Dictionary<string, int> {
        {Upgrades.StartingVelocity, 100},
        {Upgrades.MaxVelocity, 100},
        {Upgrades.AccelerationRate, 2500},
        {Upgrades.ParticleWeight, 3000},
        {Upgrades.DashSpeed, 2500},
        {Upgrades.DashTime, 2500},
        {Upgrades.TurnRate, 10000}
    };

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public float GetValue(string upgrade) {
        return baseValues[upgrade] + upgradeIncrements[upgrade] * curLevels[upgrade];
    }

    public int GetUpgradeCost(string upgrade) {
        // Upgrades double in cost per level
        return upgradeCost[upgrade] << curLevels[upgrade];
    }

    public int GetLevel(string upgrade) {
        return curLevels[upgrade];
    }

    public int GetMaxLevel(string upgrade) {
        return maxLevels[upgrade];
    }

    public bool CanUpgrade(string upgrade) {
        return curLevels[upgrade] < maxLevels[upgrade];
    }

    public bool Upgrade(string upgrade) {
        if (CanUpgrade(upgrade)) {
            curLevels[upgrade]++;
            return true;
        }
        return false;
    }
}