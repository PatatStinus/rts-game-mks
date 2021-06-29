using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawnBase : MonoBehaviour
{
    [SerializeField] private ResourceManager resources;
    [SerializeField] private GameObject farmPrefab;
    [SerializeField] private GameObject barracksPrefab;
    private bool spawnBarrack;
    private bool spawnFarm;

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit) && hit.transform.name == "Plane" && spawnFarm && resources.wood >= 10 && resources.food >= 10 && resources.stone >= 10)
            {
                farmPrefab.transform.position = new Vector3(hit.point.x, farmPrefab.transform.localScale.y / 2, hit.point.z);
                Instantiate(farmPrefab);
                spawnFarm = false;
                resources.wood -= 10;
                resources.stone -= 10;
                resources.food -= 10;
            }

            if (Physics.Raycast(ray, out hit) && hit.transform.name == "Plane" && spawnBarrack && resources.wood >= 10 && resources.food >= 10 && resources.stone >= 10)
            {
                barracksPrefab.transform.position = new Vector3(hit.point.x, farmPrefab.transform.localScale.y / 2, hit.point.z);
                Instantiate(barracksPrefab);
                spawnBarrack = false;
                resources.wood -= 10;
                resources.stone -= 10;
                resources.food -= 10;
            }
        }
    }

    public void SpawnFarm()
    {
        StartCoroutine(TimeFarmBuilding());
    }

    public void SpawnBarracks()
    {
        StartCoroutine(TimeBarracksBuilding());
    }

    IEnumerator TimeFarmBuilding()
    {
        yield return new WaitForSeconds(0.1f);
        spawnFarm = true;
    }

    IEnumerator TimeBarracksBuilding()
    {
        yield return new WaitForSeconds(0.1f);
        spawnBarrack = true;
    }
}
