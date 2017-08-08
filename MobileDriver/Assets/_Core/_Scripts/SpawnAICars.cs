using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAICars : MonoBehaviour {
    public GameObject carPrefab;
    public List<GameObject> carPrefabs = new List<GameObject>();
    float spawnTimer;
    public Vector3[] spawnPositions;
    public float minSpawnRaate = 2;
    public float maxSpawnRate = 12;
    public SimpleCarSteer car;
    private int currentCarIDX = 0;
	// Use this for initialization
	void Start () {
        car = GetComponent<SimpleCarSteer>();
        spawnTimer = Random.Range(minSpawnRaate, maxSpawnRate);
        InvokeRepeating( "InitialSpawn",1,8);
       
    }
	
	// Update is called once per frame
	void Update () {
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
        pos.z+= +gameObject.transform.position.z;


        Instantiate(carPrefab, pos, Quaternion.identity);
    }
    void InitialSpawn()
    {
        for (int i =20; i< 35; i++)
        {
            int idx = Random.Range(0, spawnPositions.Length);
            Vector3 pos = new Vector3(spawnPositions[idx].x,0,(transform.position.z+2*i*3));
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
