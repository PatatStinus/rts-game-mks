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
        public bool playerInput;
        private GameObject indicator;
        private Renderer indicatorColor;
        private NavMeshAgent agent;
        #endregion

        #region Farm
        [SerializeField] private List<GameObject> farms;
        [SerializeField] private List<float> distanceFarms;
        private GameObject farmParent;
        private int totalFarms = 0;
        private int closestFarm;
        #endregion

        #region Mining
        [SerializeField] private List<GameObject> mines;
        [SerializeField] private List<float> distanceMines;
        private GameObject miningParent;
        private int totalMines = 0;
        private int closestMine;
        #endregion

        #region Wood Chopping
        [SerializeField] private List<GameObject> woods;
        [SerializeField] private List<float> distanceWoods;
        private GameObject woodParent;
        private int totalWoods = 0;
        private int closestWood;
        #endregion
        #endregion

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            indicator = transform.GetChild(0).gameObject;
            indicatorColor = indicator.GetComponent<Renderer>();
            indicator.SetActive(false);

            farmParent = GameObject.FindGameObjectWithTag("Food");
            miningParent = GameObject.FindGameObjectWithTag("Rock");
            woodParent = GameObject.FindGameObjectWithTag("Wood");

            totalFarms = farmParent.transform.childCount;
            totalMines = miningParent.transform.childCount;
            totalWoods = woodParent.transform.childCount;
        }

        private void Start()
        {
            for (int i = 0; i < totalFarms; i++)
            {
                farms.Add(farmParent.transform.GetChild(i).gameObject);
                distanceFarms.Add(0);
            }
            for (int i = 0; i < totalMines; i++)
            {
                mines.Add(miningParent.transform.GetChild(i).gameObject);
                distanceMines.Add(0);
            }
            for (int i = 0; i < totalWoods; i++)
            {
                woods.Add(woodParent.transform.GetChild(i).gameObject);
                distanceWoods.Add(0);
            }
        }

        public void Selected(bool select) { indicator.SetActive(select); }

        public void MoveUnit(Vector3 position)
        {
            playerInput = true;
            agent.SetDestination(position);
        }

        private void Update()
        {
            //0 = Farming
            //1 = Mining
            //2 = Chopping wood

            if (Vector3.Distance(agent.destination, transform.position) < 5)
                playerInput = false;

            if (job == 0)
            {
                indicatorColor.material.SetColor("_BaseColor", Color.yellow);
                if (farms.Count > 0)
                {
                    for (int i = 0; i < farms.Count; i++)
                    {
                        distanceFarms[i] = Vector3.Distance(transform.position, farms[i].transform.position);
                    }
                    closestFarm = distanceFarms.IndexOf(distanceFarms.Min());
                    if(!playerInput)
                        agent.SetDestination(farms[closestFarm].transform.position);
                }
            }
            if(job == 1)
            {
                indicatorColor.material.SetColor("_BaseColor", Color.grey);
                if (mines.Count > 0)
                {
                    for (int i = 0; i < mines.Count; i++)
                    {
                        distanceMines[i] = Vector3.Distance(transform.position, mines[i].transform.position);
                    }
                    closestMine = distanceMines.IndexOf(distanceMines.Min());
                    if(!playerInput)
                        agent.SetDestination(mines[closestMine].transform.position);
                }
            }
            if(job == 2)
            {
                indicatorColor.material.SetColor("_BaseColor", Color.green);
                if (woods.Count > 0)
                {
                    for (int i = 0; i < woods.Count; i++)
                    {
                        distanceWoods[i] = Vector3.Distance(transform.position, woods[i].transform.position);
                    }
                    closestWood = distanceWoods.IndexOf(distanceWoods.Min());
                    if(!playerInput)
                        agent.SetDestination(woods[closestWood].transform.position);
                }
            }
        }
    }
}
