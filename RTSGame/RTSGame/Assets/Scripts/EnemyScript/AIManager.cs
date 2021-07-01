using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] private GameObject createVillager;
    [SerializeField] private GameObject spawnLoc;
    [SerializeField] private GameObject farmParent;
    [SerializeField] private GameObject barrackParent;
    [SerializeField] private GameObject townHall;
    [SerializeField] private GameObject farmPrefab;
    [SerializeField] private GameObject barracksPrefab;
    [SerializeField] private WinLoseManager win;
    private int leftRight = 0;
    private bool spawnedFarm = false;
    public int team;
    public int wood = 0;
    public int food = 0;
    public int stone = 0;
    private int vSpawnCount;
    public int totalVillagers = 10;
    private int villagerCount = 0;
    private bool spawning = false;
    private bool matchDecided = false;

    private void Update()
    {
        if (townHall == null && !matchDecided)
        {
            for (int i = 0; i < spawnLoc.transform.childCount; i++)
                Destroy(spawnLoc.transform.GetChild(i).gameObject);
            win.win++;
            matchDecided = true;
            return;
        }
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

        SpawningBuilding();
    }
    
    public void SpawnVillager()
    {
        vSpawnCount++;
        spawning = false;
    }

    private void SpawningBuilding()
    {
        if (!spawnedFarm && wood >= 10 && stone >= 10 && food >= 10 && farmParent.transform.childCount <= 5)
        {
            farmPrefab.tag = $"Enemy{team}";
            leftRight = Random.Range(0, 2);
            if(leftRight == 0)
                farmPrefab.transform.position = new Vector3(0 - Random.Range(20, 151), farmPrefab.transform.position.y, 0 - Random.Range(20, 151));
            if (leftRight == 1)
                farmPrefab.transform.position = new Vector3(0 + Random.Range(20, 151), farmPrefab.transform.position.y, 0 + Random.Range(20, 151));
            Instantiate(farmPrefab, farmParent.transform);
            for (int i = 1; i < spawnLoc.transform.childCount; i++)
            {
                spawnLoc.transform.GetChild(i).gameObject.GetComponent<EnemyUnit>().gettingFoodTime = 3;
                spawnLoc.transform.GetChild(i).gameObject.GetComponent<EnemyUnit>().gettingFoodTime /= farmParent.transform.childCount;
            }
            spawnedFarm = true;
            wood -= 10;
            stone -= 10;
            food -= 10;
        }
        if (spawnedFarm && wood >= 10 && stone >= 10 && food >= 10 && totalVillagers < 30)
        {
            barracksPrefab.tag = $"Enemy{team}";
            leftRight = Random.Range(0, 2);
            if (leftRight == 0)
                barracksPrefab.transform.position = new Vector3(0 - Random.Range(20, 151), barracksPrefab.transform.position.y, 0 - Random.Range(20, 151));
            if (leftRight == 1)
                barracksPrefab.transform.position = new Vector3(0 + Random.Range(20, 151), barracksPrefab.transform.position.y, 0 + Random.Range(20, 151));
            Instantiate(barracksPrefab, barrackParent.transform);
            totalVillagers += 5;
            spawnedFarm = false;
            wood -= 10;
            stone -= 10;
            food -= 10;
        }
        if (totalVillagers > 29)
            spawnedFarm = false;
    }
}
