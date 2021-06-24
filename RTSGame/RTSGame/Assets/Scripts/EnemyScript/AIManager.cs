using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] public GameObject createVillager;
    [SerializeField] public GameObject spawnLoc;
    public int team;
    public int wood = 0;
    public int food = 0;
    public int stone = 0;
    private int vSpawnCount;
    public int totalVillagers = 30;
    private int villagerCount = 0;
    private bool spawning = false;

    private void Update()
    {
        if (vSpawnCount > 0)
        {
            createVillager.GetComponent<EnemyUnit>().team = team;
            Instantiate(createVillager, spawnLoc.transform);
            vSpawnCount--;
        }

        villagerCount = spawnLoc.transform.childCount - 1;

        if (!spawning && villagerCount < totalVillagers)
        {
            float time = Random.Range(10f, 20f);
            Invoke("SpawnVillager", time);
            spawning = true;
        }
    }
    
    public void SpawnVillager()
    {
        vSpawnCount++;
        spawning = false;
    }
}
