using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFarm : MonoBehaviour
{
    [SerializeField] private GameObject farm;
    [SerializeField] private GameObject farmParent;
    private Camera cam;
    private Ray ray;
    private bool isSpawning;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (isSpawning == true)
        {
            //ray = cam.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //
            //if (Physics.Raycast(ray, out hit))
            //    farm.transform.position = hit.point;
            //if (Input.GetMouseButtonDown(0))
                //isSpawning = false;
        }
    }

    public void SpawnFarmButton()
    {
        isSpawning = true;
        Instantiate(farm, farmParent.transform);
    }
}
