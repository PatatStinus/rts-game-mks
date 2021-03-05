using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour
{
    private Transform civSpawnLocation;
    public GameObject objectToCreate;
    public GameObject civSpawn;
    private bool spawnReady = true;
    private float cooldownTime = 10f;

    private void Update()
    {
        if(spawnReady)
        {
            cooldownTime -= 1 * Time.deltaTime;
            if (cooldownTime <= 0f)
            {
                civSpawnLocation = civSpawn.transform;
                Instantiate(objectToCreate, civSpawnLocation);
                cooldownTime = 10f;
                spawnReady = false;
            }
        }

        Debug.Log(cooldownTime);
    }
    public void SpawnObject()
    {
        spawnReady = true;
    }
}
