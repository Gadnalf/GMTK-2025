using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {
    public float maxY = 20;
    public float minY = -20;
    public float spawnX = 12f; // X position where obstacles will spawn
    public float spawnInterval = 2.0f;
    public int waveSize = 10;
    public GameObject obstaclePrefab;
    public float randomOffset = 0.5f;
    public float randomScaleMin = 0.8f;
    public float randomScaleMax = 1.2f;

    private float nextSpawnTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (nextSpawnTime <= 0) {
            SpawnWave();
            nextSpawnTime = spawnInterval;
        } else {
            nextSpawnTime -= Time.fixedDeltaTime;
        }
    }

    void SpawnWave() {
        for (int i = 0; i < waveSize; i++) {
            float spawnY = Random.Range(minY, maxY);
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);
            SpawnObstacle(spawnPosition);
        }
    }

    void SpawnObstacle(Vector2 position) {
        // Spawn prefab at supplied position with randomized offset and scale
        GameObject obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity);
        float randomScale = Random.Range(randomScaleMin, randomScaleMax);
        obstacle.transform.localScale = new Vector3(randomScale, randomScale, 1);
        obstacle.transform.position = new Vector2(position.x + Random.Range(-randomOffset, randomOffset), position.y);
    }
}
