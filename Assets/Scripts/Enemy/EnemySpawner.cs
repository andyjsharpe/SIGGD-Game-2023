using System;
using System.Collections;
//using UnityEditor.ShaderGraph;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject player; 

    [SerializeField]
    public float minDistance = 5.0f;
    [SerializeField]
    public float maxDistance = 10.0f;

    public float constantSpawnTimer = 0.0f;
    [SerializeField] public float constantSpawnInterval = 7.0f; //seconds
    public float waveSpawnTimer = 0.0f;
    [SerializeField] public float waveSpawnInterval = 60.0f; //seconds
    [SerializeField] public int waveVolume = 5;
    [SerializeField] public float waveSpeed = 3f;

    int enemyCount = 0;

    [SerializeField]
    LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnWave());
    }

    // Update is called once per frame
    void Update()
    {
        // Theres definitely a better way to constant spawn
        // I tried invoke repeating but that breaks with parameters
        constantSpawnTimer += Time.deltaTime;
        waveSpawnTimer += Time.deltaTime;
        if (constantSpawnTimer > constantSpawnInterval) {
            constantSpawnTimer -= constantSpawnInterval;
            SpawnEnemy();
        }
        if (waveSpawnTimer > waveSpawnInterval) {
            waveSpawnTimer -= waveSpawnInterval;
            StartCoroutine(SpawnWave());
        }
    }

    // Optional spawning in a cone for waves
    private Vector2 generatePointInRing(int degrees = -1, int spread = -1) {
        Vector2 dir = Random.insideUnitCircle.normalized;
        if (degrees != -1) {
            int randomDegrees = degrees + Random.Range(-spread, spread);
            dir = new Vector2(Mathf.Sin(Mathf.Deg2Rad * randomDegrees), Mathf.Cos(Mathf.Deg2Rad* randomDegrees));
        }
        float dist = Random.Range(minDistance, maxDistance);
        return dir * dist;
    }

    // Optional spawning in a cone for waves
    void SpawnEnemy(int degrees = -1, int spread = -1) {
        // Spawn object
        Vector2 randomPoint = generatePointInRing();
        Vector3 randomPoint3D = new Vector3(randomPoint.x, 0.0f, randomPoint.y) + player.transform.position;

        // Get the color of the region our random point is in
        GameObject enemy = null;
        Collider[] collArray = Physics.OverlapSphere(randomPoint3D, 1.0f, mask, QueryTriggerInteraction.Collide);
        for (int j = 0; j < collArray.Length; j++) {
            if (collArray[j].gameObject.TryGetComponent(out EnemyReference enemyReference)) {
                enemy = enemyReference.enemy;
                Debug.Log("Spawning " + enemy.name + "!");
            }
        }

        // Generate a new point to spawn the enemy in with the color we found
        randomPoint = generatePointInRing(degrees, spread);
        randomPoint3D = new Vector3(randomPoint.x, 0, randomPoint.y) +  player.transform.position;
        Instantiate(enemy, randomPoint3D, Quaternion.identity);
        Debug.Log("Enemy Spawned, " + enemyCount + " total");
        enemyCount++;
    }

    IEnumerator SpawnWave() {
        Debug.Log("SPAWN WAVE COROUTINE");
        // Randomize spread and direction for wave
        int degrees = Random.Range(0, 360);
        // Deviation from degrees in one direction (2 * spread is the whole arc)
        int spread = Random.Range(45, 90);
        int currentTotalSpawns = 0;
        // Larger numbers make the wave have more enemy volume
        //int waveVolume = 7;
        // As this approaches 1, the wave spawns much faster
        // Don't make it less than 1 or it'll probably break since we wait for speed - sin(x) seconds
        //float waveSpeed = 3;
        while (currentTotalSpawns / waveVolume < Mathf.PI) {
            SpawnEnemy(degrees, spread);
            currentTotalSpawns++;
            yield return new WaitForSeconds(waveSpeed - Mathf.Sin((float)currentTotalSpawns / (float)waveVolume));
        }
    }
}
