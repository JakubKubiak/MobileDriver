using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDriverSpawner : MonoBehaviour
{
    public GameObject carPrefab;
    public List<GameObject> carPrefabs = new List<GameObject>();
    float spawnTimer;
    public Vector3[] spawnPositions;
    public float minSpawnRate = 2;
    public float maxSpawnRate = 12;
    public DriverController car;
    private int currentCarIDX = 0;

    void Start()
    {
        car = GetComponent<DriverController>();
        spawnTimer = Random.Range(minSpawnRate, maxSpawnRate);
        InvokeRepeating("InitialSpawn", 1, 8);
    }

    void Update()
    {
        //spawnTimer -= Time.deltaTime;
        //if(spawnTimer<= 8)
        //{
        //    Spawn();
        //    spawnTimer = Random.Range(minSpawnRaate, maxSpawnRate);
        //}
    }
    void Spawn()
    {
        Vector3 pos = spawnPositions[Random.Range(0, spawnPositions.Length)];
        pos.z += +gameObject.transform.position.z;
        Instantiate(carPrefab, pos, Quaternion.identity);
    }
    void InitialSpawn()
    {
        for (int i = 20; i < 35; i++)
        {
            int idx = Random.Range(0, spawnPositions.Length);
            Vector3 pos = new Vector3(spawnPositions[idx].x, 0, (transform.position.z + 2 * i * 3));
            pos.z += spawnPositions[idx].z;
            Debug.Log(pos);
            Instantiate(GetCarPrefab(), pos, Quaternion.identity);
        }
    }
    GameObject GetCarPrefab()
    {
        GameObject currentCarPrefab = carPrefabs[currentCarIDX];

        if (currentCarIDX < carPrefabs.Count - 1)
        {
            currentCarIDX++;
        }

        else
        {
            currentCarIDX = 0;
        }
        return currentCarPrefab;
    }
}
