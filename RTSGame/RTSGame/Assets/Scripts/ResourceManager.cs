using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public int wood = 10;
    public int food = 0;
    public int stone = 0;

    [SerializeField] private TextMeshProUGUI woodAmount;
    [SerializeField] private TextMeshProUGUI foodAmount;
    [SerializeField] private TextMeshProUGUI stoneAmount;

    void Update()
    {
        woodAmount.text = wood.ToString();
        foodAmount.text = food.ToString();
        stoneAmount.text = stone.ToString();
    }
}
