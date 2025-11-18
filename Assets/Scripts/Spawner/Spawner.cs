using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public static Action OnWaveCompleted;

    [Header("Settings")]
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private float delayBtwWaves = 1f;

    [Header("Spawn Delay Interval")]
    [SerializeField] private float minSpawnDelay = 0.5f;
    [SerializeField] private float maxSpawnDelay = 1.5f;

    [Header("Poolers")]
    [SerializeField] private ObjectPooler enemyWave10Pooler;
    [SerializeField] private ObjectPooler enemyWave11To20Pooler;
    [SerializeField] private ObjectPooler enemyWave21To30Pooler;
    [SerializeField] private ObjectPooler enemyWave31To40Pooler;
    [SerializeField] private ObjectPooler enemyWave41To50Pooler;

    private float _spawnTimer;
    private int _enemiesSpawned;
    private int _enemiesRemaining;
    private Waypoint _waypoint;

    private void Start()
    {
        _waypoint = GetComponent<Waypoint>();
        StartNewWave();  
    }

    private void Update()
    {
        if (_enemiesSpawned >= enemyCount) return;

        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0f)
        {
            _spawnTimer = GetRandomDelay();
            SpawnEnemy();
        }
    }

    private void StartNewWave()
    {
        _enemiesSpawned = 0;
        _enemiesRemaining = enemyCount;
        _spawnTimer = 0.1f;   
    }

    private void SpawnEnemy()
    {
        _enemiesSpawned++;

        GameObject instance = GetPooler().GetInstanceFromPool();
        Enemy enemy = instance.GetComponent<Enemy>();
        enemy.Waypoint = _waypoint;
        enemy.ResetEnemy();

        enemy.transform.position = transform.position;
        instance.SetActive(true);
    }

    private float GetRandomDelay()
    {
        return Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    private ObjectPooler GetPooler()
    {
        int wave = LevelManager.Instance.CurrentWave;

        if (wave <= 10) return enemyWave10Pooler;
        if (wave <= 20) return enemyWave11To20Pooler;
        if (wave <= 30) return enemyWave21To30Pooler;
        if (wave <= 40) return enemyWave31To40Pooler;
        return enemyWave41To50Pooler;
    }

    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(delayBtwWaves);
        StartNewWave();  
    }

    private void RecordEnemy(Enemy enemy)
    {
        _enemiesRemaining--;

        if (_enemiesRemaining <= 0)
        {
            OnWaveCompleted?.Invoke();  
            StartCoroutine(NextWave());
        }
    }

    private void OnEnable()
    {
        Enemy.OnEndReached += RecordEnemy;
        EnemyHealth.OnEnemyKilled += RecordEnemy;
    }

    private void OnDisable()
    {
        Enemy.OnEndReached -= RecordEnemy;
        EnemyHealth.OnEnemyKilled -= RecordEnemy;
    }
}
