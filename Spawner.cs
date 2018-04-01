using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public Transform targetLocation;
    public GameObject zombiePrefab;
    public GameObject survivorPrefab;
    public GameObject bossPrefab;

    private int counter = 0;

    private Vector3 spawnLocation = Vector3.zero;

	void Start () {
        InvokeRepeating("SpawnZombie", 2.5f, 3f);
	}
	
	void SpawnZombie()
    {
        counter++;

        if(counter % 5 == 0)
        {
            spawnLocation = SpawnLocations();

            Instantiate(bossPrefab, spawnLocation, Quaternion.identity);
        }

        if (counter % 3 == 0)
        {
            spawnLocation = SpawnLocations();

            Instantiate(survivorPrefab, spawnLocation, Quaternion.identity);
        }
        else
        {
            SoundManager.instance.PlayOneShot(SoundManager.instance.zombieGrunt);

            spawnLocation = SpawnLocations();

            Instantiate(zombiePrefab, spawnLocation, Quaternion.identity);
        }
    }

    Vector3 SpawnLocations()
    {
        Vector3 directionSpawn = Vector3.zero;

        int sideSpawn = Random.Range(0, 2);

        switch (sideSpawn)
        {
            case 0:
                directionSpawn = new Vector3(Random.Range(targetLocation.position.x - 3, targetLocation.position.x + 3), Random.Range(targetLocation.position.y + 4, targetLocation.position.y + 7));
                break;

            case 1:
                directionSpawn = new Vector3(Random.Range(targetLocation.position.x + 4, targetLocation.position.x + 7), Random.Range(targetLocation.position.y - 3, targetLocation.position.y + 3));
                break;

            case 2:
                directionSpawn = new Vector3(Random.Range(targetLocation.position.x - 4, targetLocation.position.x - 7), Random.Range(targetLocation.position.y - 3, targetLocation.position.y + 3));
                break;
        }

        return directionSpawn;
    }
}
