using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseManager : MonoBehaviour
{
    public int win = 0;
    public int loss = 0;
    public GameObject winScreen;
    public GameObject lossScreen;

    void Update()
    {
        if(win == 3)
        {
            winScreen.SetActive(true);
        }
        if (loss == 1)
        {
            lossScreen.SetActive(true);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
