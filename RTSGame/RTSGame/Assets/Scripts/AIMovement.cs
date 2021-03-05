using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private NavMeshAgent simpleMovement;
    private GameObject indicator;
    private bool playerSelected;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Player" && playerSelected == false)
                {
                    playerSelected = true;
                    simpleMovement = hit.transform.GetComponent<NavMeshAgent>();
                    //indicator.SetActive(true);
                    return;
                }

                if(playerSelected == true)
                {
                    playerSelected = false;
                    //indicator.SetActive(false);
                    simpleMovement.SetDestination(hit.point);
                }
            }
        }
    }
}
