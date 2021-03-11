using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace RTS
{
    public class Unit : MonoBehaviour
    {
        #region Variables
        #region General
        public int job = 0;
        private GameObject indicator;
        private Renderer indicatorColor;
        private NavMeshAgent agent;
        #endregion

        #region Farm
        [SerializeField] private GameObject farmParent;
        [SerializeField] private List<GameObject> farms;
        [SerializeField] private List<float> distanceFarms;
        private int totalFarms = 0;
        private int closestFarm;
        #endregion

        #region Mining
        [SerializeField] private GameObject miningParent;
        [SerializeField] private List<GameObject> mines;
        [SerializeField] private List<float> distanceMines;
        private int totalMines = 0;
        private int closestMine;
        #endregion

        #region Wood Chopping
        [SerializeField] private GameObject woodParent;
        [SerializeField] private List<GameObject> woods;
        [SerializeField] private List<float> distanceWoods;
        private int totalWoods = 0;
        private int closestWood;
        #endregion
        #endregion

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            indicator = transform.GetChild(0).gameObject;
            indicatorColor = indicator.GetComponent<Renderer>();
            indicator.SetActive(false);
        }

        public void Selected(bool select) { indicator.SetActive(select); }

        public void MoveUnit(Vector3 position) { agent.SetDestination(position); }

        private void Update()
        {
            //0 = Farming
            //1 = Mining
            //2 = Chopping wood

            if(job == 0)
            {
                indicatorColor.material.SetColor("_Color", Color.yellow);
                if (farms.Count > 0)
                {
                    for (int i = 0; i < farms.Count; i++)
                    {
                        distanceFarms[i] = Vector3.Distance(transform.position, farms[i].transform.position);
                    }
                    closestFarm = distanceFarms.IndexOf(distanceFarms.Min());
                    agent.SetDestination(farms[closestFarm].transform.position);
                }
            }
            if(job == 1)
            {
                indicatorColor.material.SetColor("_Color", Color.grey);
                if (mines.Count > 0)
                {
                    for (int i = 0; i < mines.Count; i++)
                    {
                        distanceMines[i] = Vector3.Distance(transform.position, mines[i].transform.position);
                    }
                    closestMine = distanceMines.IndexOf(distanceMines.Min());
                    agent.SetDestination(mines[closestMine].transform.position);
                }
            }
            if(job == 2)
            {
                indicatorColor.material.SetColor("_Color", Color.red);
                if (woods.Count > 0)
                {
                    for (int i = 0; i < woods.Count; i++)
                    {
                        distanceWoods[i] = Vector3.Distance(transform.position, woods[i].transform.position);
                    }
                    closestWood = distanceWoods.IndexOf(distanceWoods.Min());
                    agent.SetDestination(woods[closestWood].transform.position);
                }
            }
            if(Input.GetKeyDown("f"))
            {
                farms.Add(farmParent.transform.GetChild(totalFarms).gameObject);
                distanceFarms.Add(0);
                totalFarms++;
                mines.Add(miningParent.transform.GetChild(totalMines).gameObject);
                distanceMines.Add(0);
                totalMines++;
                woods.Add(woodParent.transform.GetChild(totalWoods).gameObject);
                distanceWoods.Add(0);
                totalWoods++;
            }
        }
    }
}
