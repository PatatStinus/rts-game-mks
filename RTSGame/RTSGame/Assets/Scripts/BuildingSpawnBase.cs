using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawnBase : MonoBehaviour
{
    [SerializeField] private GameObject barrackPrefab;
    private bool spawnBarrack;
    private bool spawnWood;
    private bool spawnMine;
    private bool spawnFarm;

    void Update()
    {
        if(Input.GetMouseButtonUp(0) && spawnBarrack)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit) && hit.transform.name == "Plane")
            {
                barrackPrefab.transform.position = new Vector3(hit.point.x, barrackPrefab.transform.localScale.y / 2, hit.point.z);
                Instantiate(barrackPrefab);
            }
        }
    }

    public void SpawnFarm()
    {

    }

    public void SpawnMine()
    {

    }

    public void SpawnWood()
    {

    }

    public void SpawnBarracks()
    {
        spawnBarrack = true;
    }

    public void SpawnTraining()
    {

    }
}
