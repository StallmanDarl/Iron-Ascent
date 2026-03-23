using UnityEngine;
using System.Collections.Generic;

public class EnemyPocket : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject[] enemyPrefabs;
    public int enemyCount = 4;

    bool activated = false;
    int enemiesAlive = 0;

    void OnTriggerEnter(Collider other)
    {
        if (!activated && other.CompareTag("Player"))
        {
            activated = true;
            ActivatePocket();
        }
    }

    void ActivatePocket()
    {
        Debug.Log("Pocket Activated");

        // Update NavMesh for obstacles
        NavMeshUpdater.Instance.UpdateNavMesh();

        for (int i = 0; i < enemyCount; i++)
            SpawnEnemy();
    }

    void SpawnEnemy()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        GameObject enemy = Instantiate(prefab, point.position, Quaternion.identity);

        enemiesAlive++;

        enemy.GetComponent<EnemyHealth>().OnEnemyDeath += OnEnemyKilled;
    }

    void OnEnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
            PocketCleared();
    }

    void PocketCleared()
    {
        Debug.Log("Pocket Cleared");
        ArenaManager.Instance.PocketCleared();
        Destroy(gameObject);
    }
}