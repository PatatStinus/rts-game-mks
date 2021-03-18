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
        private ResourceManager addResources;
        public int job = 0;
        public bool playerInput;
        private GameObject indicator;
        private Renderer indicatorColor;
        private NavMeshAgent agent;
        public bool reachedDest;
        #endregion

        #region Farm
        private List<GameObject> farms = new List<GameObject>();
        private List<float> distanceFarms = new List<float>();
        private GameObject farmParent;
        private int totalFarms = 0;
        private int closestFarm;
        private bool gettingFood;
        #endregion

        #region Mining
        private List<GameObject> mines = new List<GameObject>();
        private List<float> distanceMines = new List<float>();
        private GameObject miningParent;
        private int totalMines = 0;
        private int closestMine;
        private bool gettingStone;
        #endregion

        #region Wood Chopping
        private List<GameObject> woods = new List<GameObject>();
        private List<float> distanceWoods = new List<float>();
        private GameObject woodParent;
        private int totalWoods = 0;
        private int closestWood;
        private bool gettingWood;
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

            addResources = GameObject.FindObjectOfType<ResourceManager>();
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
            agent.SetDestination(position);
            playerInput = true;
            reachedDest = false;
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
                    if(!playerInput && !reachedDest)
                        agent.SetDestination(farms[closestFarm].transform.position);
                    if (reachedDest && !gettingFood)
                    {
                        Invoke("GetFood", 1f);
                        gettingFood = true;
                    }
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
                    if(!playerInput && !reachedDest)
                        agent.SetDestination(mines[closestMine].transform.position);
                    if (reachedDest && !gettingStone)
                    {
                        Invoke("GetStone", 1f);
                        gettingStone = true;
                    }
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
                    if(!playerInput && !reachedDest)
                        agent.SetDestination(woods[closestWood].transform.position);
                    if(reachedDest && !gettingWood)
                    {
                        Invoke("GetWood", 1f);
                        gettingWood = true;
                    }
                }
            }
            if (!playerInput && Vector3.Distance(transform.position, agent.destination) < 10)
                reachedDest = true;
            if (!playerInput && Vector3.Distance(transform.position, agent.destination) > 10)
                reachedDest = false;

            if (reachedDest)
                agent.isStopped = true;
            if (!reachedDest)
                agent.isStopped = false;
        }

        private void GetFood()
        {
            addResources.food++;
            gettingFood = false;
        }

        private void GetStone()
        {
            addResources.stone++;
            gettingStone = false;
        }

        private void GetWood()
        {
            addResources.wood++;
            gettingWood = false;
        }
    }
}
