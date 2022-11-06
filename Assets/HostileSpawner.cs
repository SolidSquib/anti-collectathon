using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileSpawner : MonoBehaviour
{
    public GameObject _Player;
    public GameObject _HostilePrefab = null;
    public GameObject _HostileSpawningPrefab = null;
    public float _HostileSpawnDelay = 1f; // how long after the HostileWarning will the actual Hostile appear
    public float _HostileSpawnWindowMin = 3f;
    public float _HostileSpawnWindowMax = 10f;

    public GameObject _ThiefPrefab = null;
    public float _ThiefSpawnWindowMin = 3f;
    public float _ThiefSpawnWindowMax = 10f;
    public float m_spawnRadius = 100.0f;

    public bool _SpawnHostiles = true;
    public bool _SpawnThief = true;

    private void OnEnable ()
    {
        if(_SpawnHostiles) StartCoroutine(HostileSpawningLoop());
        if (_SpawnThief) StartCoroutine(ThiefSpawningLoop());
    }

    IEnumerator HostileSpawningLoop()
    {
        float randomTime = 0f;
        while (true)
        {
            randomTime = Random.Range(_HostileSpawnWindowMin, _HostileSpawnWindowMax);
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
        newEnemy.transform.position = spawnLocation;
    }

    IEnumerator ThiefSpawningLoop()
    {
        float randomTime = 0f;
        //while (true)
        //{
            randomTime = Random.Range(_ThiefSpawnWindowMin, _ThiefSpawnWindowMax);
            yield return new WaitForSeconds(randomTime);
            SpawnThief();
        //}
    }

    private void SpawnThief()
    {
        Vector2 spawnLocation = new Vector2(Random.Range(-m_spawnRadius, m_spawnRadius), Random.Range(-m_spawnRadius, m_spawnRadius));
        GameObject newThief = Instantiate(_ThiefPrefab);
        newThief.GetComponent<ThiefBehaviour>()._Player = _Player;
        newThief.GetComponent<ThiefBehaviour>().hostileSpawner = this;
        newThief.transform.position = spawnLocation;
    }

    public void ThiefDestroyed(GameObject thief) 
    {
        StartCoroutine(ThiefSpawningLoop());
    }
}
