using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RTS
{
    public class Villager : MonoBehaviour
    {
        [SerializeField] private GameObject objectToCreate;
        [SerializeField] private GameObject civSpawn;
        [SerializeField] private TextMeshProUGUI villagerQueue;
        [SerializeField] private Image circleTimer;
        private float cooldownTime = 10f;
        private int vSpawnCount;
        private int villagerCount = 0;
        public int totalVillagers;
        public List<GameObject> villagerUnits;
        public int jobOne;
        public int jobTwo;
        public int jobThree;

        private void Update()
        {
            if (vSpawnCount > 0)
            {
                cooldownTime -= 1 * Time.deltaTime;
                if (cooldownTime <= 0f && villagerCount < totalVillagers)
                {
                    GameObject villager = Instantiate(objectToCreate, civSpawn.transform);
                    villagerCount++;
                    villagerUnits.Add(villager);
                    cooldownTime = 10f;
                    vSpawnCount--;
                }
                villagerQueue.text = vSpawnCount.ToString("0");
                circleTimer.fillAmount = cooldownTime/10;
            }

            CheckJobs();
        }
        public void SpawnObject()
        {
            if(villagerCount < totalVillagers &&  villagerCount + vSpawnCount < totalVillagers)
            {
                vSpawnCount++;
                if (vSpawnCount > 10)
                    vSpawnCount = 10;
            }
        } 

        private void CheckJobs()
        {
            jobOne = 0;
            jobTwo = 0;
            jobThree = 0;

            for(int i = 0; i < villagerUnits.Count; i++)
            {
                if (villagerUnits[i].GetComponent<Unit>().job == 1)
                    jobOne++;
                if (villagerUnits[i].GetComponent<Unit>().job == 2)
                    jobTwo++;
                if (villagerUnits[i].GetComponent<Unit>().job == 3)
                    jobThree++;
            }
        }
    }
}
