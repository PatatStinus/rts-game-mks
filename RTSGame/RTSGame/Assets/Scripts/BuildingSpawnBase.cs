using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawnBase : MonoBehaviour
{
    [SerializeField] private GameObject buildingPrefab;

    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit) && hit.transform.name == "Plane")
            {
                buildingPrefab.transform.position = new Vector3(hit.point.x, buildingPrefab.transform.localScale.y / 2, hit.point.z);
                Instantiate(buildingPrefab);
            }
        }
    }
}
