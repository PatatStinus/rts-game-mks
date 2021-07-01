using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUISwitch : MonoBehaviour
{
    public GameObject jobsPanel;
    public GameObject villagerPanel;
    public GameObject buildingPanel;

    public void Villager()
    {
        villagerPanel.SetActive(true);
        jobsPanel.SetActive(false);
        buildingPanel.SetActive(false);
    }

    public void Jobs()
    {
        villagerPanel.SetActive(false);
        jobsPanel.SetActive(true);
        buildingPanel.SetActive(false);
    }

    public void Buildings()
    {
        villagerPanel.SetActive(false);
        jobsPanel.SetActive(false);
        buildingPanel.SetActive(true);
    }
}
