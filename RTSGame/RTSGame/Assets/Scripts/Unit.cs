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
        public int job = 1;
        public int villagerArray;
        public int totalVillagers;
        private ResourceManager addResources;
        private bool playerInput;
        private GameObject indicator;
        private Renderer indicatorColor;
        private NavMeshAgent agent;
        private bool reachedDest;
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
        private GameObject mineStation;
        private GameObject miningParent;
        private int totalMines = 0;
        private int closestMine;
        private bool gettingStone;
        public int thisStone;
        private bool delStone;
        private bool givingStone;
        #endregion

        #region Wood Chopping
        private List<GameObject> woods = new List<GameObject>();
        private List<float> distanceWoods = new List<float>();
        private GameObject woodStation;
        private GameObject woodParent;
        private int totalWoods = 0;
        private int closestWood;
        private bool gettingWood;
        public int thisWood;
        private bool delWood;
        private bool givingWood;
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

            woodStation = woodParent.transform.GetChild(0).gameObject;
            mineStation = miningParent.transform.GetChild(0).gameObject;
        }

        private void Start()
        {
            for (int i = 0; i < totalFarms; i++)
            {
                farms.Add(farmParent.transform.GetChild(i).gameObject);
                distanceFarms.Add(0);
            }
            for (int i = 1; i < totalMines; i++)
            {
                mines.Add(miningParent.transform.GetChild(i).gameObject);
                distanceMines.Add(0);
            }
            for (int i = 1; i < totalWoods; i++)
            {
                woods.Add(woodParent.transform.GetChild(i).gameObject);
                distanceWoods.Add(0);
            }
        }

        public void Selected(bool select) { indicator.SetActive(select); }

        public void MoveUnit(Vector3 position)
        {
            SetDestination(position);
            playerInput = true;
            reachedDest = false;
        }

        private void Update()
        {
            //1 = Farming
            //2 = Mining
            //3 = Chopping wood

            if (Vector3.Distance(agent.destination, transform.position) < 5)
                playerInput = false;

            if (job == 1)
            {
                delStone = false;
                delWood = false;
                indicatorColor.material.SetColor("_BaseColor", Color.yellow);
                if (farms.Count > 0)
                {
                    for (int i = 0; i < farms.Count; i++)
                        distanceFarms[i] = Vector3.Distance(transform.position, farms[i].transform.position);

                    closestFarm = distanceFarms.IndexOf(distanceFarms.Min());

                    if (!playerInput)
                        agent.SetDestination(farms[closestFarm].transform.position);

                    DistanceCheck();

                    if (reachedDest && !gettingFood)
                    {
                        Invoke("GetFood", 1f);
                        gettingFood = true;
                    }
                }
            }
            if(job == 2)
            {
                delWood = false;
                indicatorColor.material.SetColor("_BaseColor", Color.grey);
                if (mines.Count > 0)
                {
                    if (closestMine < mines.Count)
                    {
                        if (mines[closestMine] == null)
                            mines.RemoveAt(closestMine);
                    }

                    for (int i = 0; i < mines.Count; i++)
                        distanceMines[i] = Vector3.Distance(transform.position, mines[i].transform.position);

                    closestMine = distanceMines.IndexOf(distanceMines.Min());

                    if (!playerInput && !delStone)
                        agent.SetDestination(mines[closestMine].transform.position);

                    DistanceCheck();

                    if (reachedDest && !gettingStone && thisStone < 10 && !delStone)
                    {
                        Invoke("GetStone", 1f);
                        gettingStone = true;
                    }

                    if (thisStone == 10 && reachedDest && !delWood)
                    {
                        agent.SetDestination(mineStation.transform.position);
                        delStone = true;
                    }

                    DistanceCheck();

                    if (delStone && reachedDest && !givingStone && thisStone > 0)
                    {
                        Invoke("DeliverStone", 1f);
                        givingStone = true;
                    }
                    if (thisStone <= 0)
                        delStone = false;
                }
            }
            if(job == 3)
            {
                delStone = false;
                indicatorColor.material.SetColor("_BaseColor", Color.green);
                if (woods.Count > 0)
                {
                    if(closestWood < woods.Count)
                    {
                        if (woods[closestWood] == null)
                            woods.RemoveAt(closestWood);
                    }

                    for (int i = 0; i < woods.Count; i++)
                        distanceWoods[i] = Vector3.Distance(transform.position, woods[i].transform.position);

                    closestWood = distanceWoods.IndexOf(distanceWoods.Min());

                    if (!playerInput && !delWood)
                        agent.SetDestination(woods[closestWood].transform.position);


                    DistanceCheck();

                    if(reachedDest && !gettingWood && thisWood < 10 && !delWood)
                    {
                        Invoke("GetWood", 1f);
                        gettingWood = true;
                    }

                    if (thisWood == 10 && reachedDest && !delStone)
                    {
                        agent.SetDestination(woodStation.transform.position);
                        delWood = true;
                    }

                    DistanceCheck();

                    if (delWood && reachedDest && !givingWood && thisWood > 0)
                    {
                        Invoke("DeliverWood", 1f);
                        givingWood = true;
                    }
                    if (thisWood <= 0)
                        delWood = false;
                }
            }

            DistanceCheck();

            agent.isStopped = reachedDest ? true : false;
        }

        #region GetResourceFunctions
        private void GetFood()
        {
            addResources.food++;
            gettingFood = false;
        }

        private void GetStone()
        {
            thisStone++;
            gettingStone = false;
            mines[closestMine].GetComponent<TreeRockHealth>().health -= 10;
        }

        private void GetWood()
        {
            thisWood++;
            gettingWood = false;
            woods[closestWood].GetComponent<TreeRockHealth>().health -= 10;
        }

        private void DeliverWood()
        {
            thisWood--;
            addResources.wood++;
            givingWood = false;
        }

        private void DeliverStone()
        {
            thisStone--;
            addResources.stone++;
            givingStone = false;
        }
        #endregion

        private void DistanceCheck() 
        {
            if (!playerInput && Vector3.Distance(transform.position, agent.destination) < 10)
                reachedDest = true;
            if (!playerInput && Vector3.Distance(transform.position, agent.destination) > 10)
                reachedDest = false;
        }

        private void SetDestination(Vector3 pos)
        {
            float newTotalVillagers = totalVillagers;

            if(totalVillagers > 14) 
            {
                newTotalVillagers = Mathf.Floor(totalVillagers / 3f * 2f);
                if(villagerArray >= Mathf.Floor(totalVillagers / 3f * 2f))
                    newTotalVillagers = Mathf.Ceil(totalVillagers / 3f);
            }

            float radius = newTotalVillagers * 3;

            var radians = 2 * Mathf.PI / newTotalVillagers * villagerArray;

            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);

            var spawnPos = pos + spawnDir * radius;

            agent.SetDestination(spawnPos);
        }
    }
}
