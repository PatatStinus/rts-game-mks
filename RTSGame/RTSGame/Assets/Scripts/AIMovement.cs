using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public Camera cam;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            cam.ScreenPointToRay(Input.mousePosition);
        }
    }
}
