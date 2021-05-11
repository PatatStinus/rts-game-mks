using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private void Update()
    {
        if (vSpawnCount > 0)
        {
            cooldownTime -= 1 * Time.deltaTime;
            if (cooldownTime <= 0f && villagerCount < totalVillagers)
            {
                Instantiate(objectToCreate, civSpawn.transform);
                villagerCount++;
                cooldownTime = 10f;
                vSpawnCount--;
                Debug.Log(villagerCount);
            }
            villagerQueue.text = vSpawnCount.ToString("0");
            circleTimer.fillAmount = cooldownTime/10;
        }
    }
    public void SpawnObject()
    {
        if(villagerCount < totalVillagers && vSpawnCount < totalVillagers)
        {
            vSpawnCount++;
            if (vSpawnCount > 10)
                vSpawnCount = 10;
        }
    } 
}
