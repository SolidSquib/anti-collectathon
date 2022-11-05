using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileSpawner : MonoBehaviour
{
    public GameObject _Player;
    public GameObject _HostilePrefab = null;
    public GameObject _HostileSpawningPrefab = null;
    public float _HostileSpawnDelay = 1f; // how long after the HostileWarning will the actual Hostile appear

    public float _SpawnWindowMin = 3f;
    public float _spawnWindowMax = 10f;

    private void Start()
    {
        StartCoroutine(SpawningLoop());
    }

    IEnumerator SpawningLoop()
    {
        float randomTime = 0f;
        while (true)
        {
            randomTime = Random.Range(_SpawnWindowMin, _spawnWindowMax);
            yield return new WaitForSeconds(randomTime);
            StartCoroutine(SpawnHostile());
        }
    }

    IEnumerator SpawnHostile ()
    {
        // Spawn the Warning
        Vector3 spawnLocation = _Player.transform.position;
        GameObject newWarning = Instantiate(_HostileSpawningPrefab);
        newWarning.transform.position = spawnLocation;

        yield return new WaitForSeconds(_HostileSpawnDelay);

        // Spawn the Enemy
        GameObject newEnemy = Instantiate(_HostilePrefab);
        newEnemy.GetComponent<HostileBehaviour>()._Player = _Player;
    }
}
