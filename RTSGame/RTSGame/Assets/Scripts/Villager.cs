using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Villager : MonoBehaviour
{
    public GameObject objectToCreate;
    public GameObject civSpawn;
    private float cooldownTime = 10f;
    public TextMeshProUGUI villagerQueue;
    public Image circleTimer;
    private int vSpawnCount;

    private void Update()
    {
        if (vSpawnCount > 0)
        {
            cooldownTime -= 1 * Time.deltaTime;
            if (cooldownTime <= 0f)
            {
                Instantiate(objectToCreate, civSpawn.transform);
                cooldownTime = 10f;
                vSpawnCount--;
            }
            villagerQueue.text = vSpawnCount.ToString("0");
            circleTimer.fillAmount = cooldownTime/10;
        }
    }
    public void SpawnObject()
    {
        vSpawnCount++;
        if (vSpawnCount > 10)
            vSpawnCount = 10;
    }
}
