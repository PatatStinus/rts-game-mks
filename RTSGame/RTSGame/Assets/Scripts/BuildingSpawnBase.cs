using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class BuildingSpawnBase : MonoBehaviour
    {
        [SerializeField] private ResourceManager resources;
        [SerializeField] private GameObject farmParent;
        [SerializeField] private GameObject barracksParent;
        [SerializeField] private GameObject farmPrefab;
        [SerializeField] private GameObject barracksPrefab;
        [SerializeField] private UIManager villagerLimit;
        [SerializeField] private GameObject allVillagers;
        public int villagerCount = 10;
        public float farmTime = 3f;
        private bool spawnBarrack;
        private bool spawnFarm;

        void Update()
        {
            //Display on UI: Max Villager Count (villagerCount)
            //Time to get food (farmTime)
            //Error Message when limit reached (UIManager.cs)
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
            
                if (Physics.Raycast(ray, out hit) && hit.transform.name == "Plane" && spawnFarm && resources.wood >= 10 && resources.food >= 10 && resources.stone >= 10)
                {
                    farmPrefab.transform.position = new Vector3(hit.point.x, farmPrefab.transform.localScale.y / 2, hit.point.z);
                    Instantiate(farmPrefab, farmParent.transform);
                    for (int i = 1; i < allVillagers.transform.childCount; i++)
                    {
                        allVillagers.transform.GetChild(i).GetComponent<Unit>().gettingFoodTime /= farmParent.transform.childCount;
                        farmTime /= farmParent.transform.childCount;
                    }
                    spawnFarm = false;
                    resources.wood -= 10;
                    resources.stone -= 10;
                    resources.food -= 10;
                }

                if (Physics.Raycast(ray, out hit) && hit.transform.name == "Plane" && spawnBarrack && resources.wood >= 10 && resources.food >= 10 && resources.stone >= 10)
                {
                    barracksPrefab.transform.position = new Vector3(hit.point.x, farmPrefab.transform.localScale.y / 2, hit.point.z);
                    Instantiate(barracksPrefab, barracksParent.transform);
                    villagerLimit.totalVillagers += 5;
                    villagerCount += 5;
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
}
