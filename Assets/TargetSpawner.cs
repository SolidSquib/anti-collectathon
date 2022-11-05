using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public List<GameObject> m_targetPrefabs = new List<GameObject>();
    public float m_spawnRadius = 100.0f;
    public float m_minSpawnDelay = 3.0f;
    public float m_maxSpawnDelay = 10.0f;
    public int m_minSpawnedTargets = 1;
    public int m_maxSpawnedTargets = 10;
    public List<Target> m_targets = new List<Target>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawningLoop());
    }

    // Update is called once per frame
    void Update()
    {
        if (m_targets.Count < m_minSpawnedTargets)
        {
            SpawnTarget();
        }
    }

    void SpawnTarget()
    {
        if (m_targetPrefabs.Count > 0 && m_targets.Count < m_maxSpawnedTargets)
        {
            Vector2 spawnPos = new Vector2(Random.Range(-m_spawnRadius, m_spawnRadius), Random.Range(-m_spawnRadius, m_spawnRadius));

            int targetIndex = Random.Range(0, m_targetPrefabs.Count - 1);
            GameObject newTarget = Instantiate(m_targetPrefabs[targetIndex], spawnPos, Quaternion.identity, transform);
            Target target = newTarget.GetComponent<Target>();
            if (target != null)
            {
                m_targets.Add(target);
            }
            else
            {
                Debug.LogError("Target prefab is invalid");
                Destroy(newTarget);
            }            
        }
    }

    IEnumerator SpawningLoop()
    {
        while (true)
        {
            float randomTime = Random.Range(m_minSpawnDelay, m_maxSpawnDelay);
            yield return new WaitForSeconds(randomTime);
            SpawnTarget();
        }        
    }

    public void RemoveTarget(Target target)
    {
        m_targets.Remove(target);
    }

    void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, m_spawnRadius);
        UnityEditor.Handles.color = new Color(0.1f, 0.8f, 0.3f, 0.1f);
        UnityEditor.Handles.DrawSolidDisc(transform.position, transform.forward, m_spawnRadius);
    }
}
