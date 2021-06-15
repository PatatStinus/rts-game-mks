using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRockHealth : MonoBehaviour
{
    [SerializeField] public float health;

    private void Update()
    {
        if (health < 1)
            Destroy(gameObject);
    }
}
