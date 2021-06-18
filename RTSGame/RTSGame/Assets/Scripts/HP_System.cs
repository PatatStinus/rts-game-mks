using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_System : MonoBehaviour
{
    [SerializeField] public float health = 100;

    private void Update()
    {
        if (health < 1)
            Destroy(gameObject);
    }
}
