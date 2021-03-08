using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    public List<NavMeshAgent> simpleMovement;
    public List<GameObject> indicator;
    private bool playerSelected;
    private int selectedPawns = 0;
    private bool selecting;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Player" && playerSelected == false && selecting == true)
                {
                    simpleMovement.Add(hit.transform.GetComponent<NavMeshAgent>());
                    indicator.Add(hit.transform.GetChild(0).gameObject);
                    indicator[selectedPawns].SetActive(true);
                    selectedPawns++;
                    return;
                }

                if (hit.transform.tag == "Player" && playerSelected == false)
                {
                    simpleMovement.Add(hit.transform.GetComponent<NavMeshAgent>());
                    indicator.Add(hit.transform.GetChild(0).gameObject);
                    indicator[selectedPawns].SetActive(true);
                    selectedPawns++;
                    playerSelected = true;
                    return;
                }

                if (playerSelected == true)
                {
                    playerSelected = false;
                    for (int i = 0; i < simpleMovement.Count; i++)
                    {
                        simpleMovement[i].SetDestination(hit.point);
                        indicator[i].SetActive(false);
                    }
                    selectedPawns = 0;
                    indicator.Clear();
                    simpleMovement.Clear();
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            playerSelected = true;
            selecting = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            playerSelected = false;
            selecting = true;
        }
    }
}
