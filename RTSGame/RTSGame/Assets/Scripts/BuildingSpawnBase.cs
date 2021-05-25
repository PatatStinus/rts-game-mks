using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawnBase : MonoBehaviour
{
    [SerializeField] private ResourceManager resources;
    [SerializeField] private GameObject farmPrefab;
    private bool spawnBarrack;
    private bool spawnWood;
    private bool spawnMine;
    private bool spawnFarm;

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit) && hit.transform.name == "Plane" && spawnFarm && resources.wood >= 10)
            {
                farmPrefab.transform.position = new Vector3(hit.point.x, farmPrefab.transform.localScale.y / 2, hit.point.z);
                Instantiate(farmPrefab);
                spawnFarm = false;
                resources.wood -= 10;
            }
        }
    }

    public void SpawnFarm()
    {
        StartCoroutine(TimeBuilding());
    }

    public void SpawnMine()
    {
        
    }

    public void SpawnWood()
    {
        spawnWood = true;
    }

    public void SpawnBarracks()
    {
        spawnBarrack = true;
    }

    public void SpawnTraining()
    {

    }
    IEnumerator TimeBuilding()
    {
        yield return new WaitForSeconds(0.1f);
        spawnFarm = true;
        spawnMine = true;
    }
}
