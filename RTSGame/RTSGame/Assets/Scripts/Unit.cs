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

        #region Knight
        private bool isAttacking;
        private bool startAttack;
        private GameObject attackingTarget;
        private List<GameObject> enemies = new List<GameObject>();
        private List<float> distEnemies = new List<float>();
        private List<float> dotEnemies = new List<float>();
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
            for (int i = 4; i < totalMines; i++)
            {
                mines.Add(miningParent.transform.GetChild(i).gameObject);
                distanceMines.Add(0);
            }
            for (int i = 4; i < totalWoods; i++)
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
            //4 = Knight

            farms.Clear();
            distanceFarms.Clear();
            for (int i = 0; i < totalFarms; i++)
            {
                farms.Add(farmParent.transform.GetChild(i).gameObject);
                distanceFarms.Add(0);
            }

            if (Vector3.Distance(agent.destination, transform.position) < 5)
                playerInput = false;

            if(job == 1)
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
                        Invoke("GetFood", 3f);
                        gettingFood = true;
                    }
                }
            }
            if(job == 2)
            {
                delWood = false;
                indicatorColor.material.SetColor("_BaseColor", Color.grey);
                if (mines.Count == 0)
                {
                    agent.SetDestination(mineStation.transform.position);

                    DistanceCheck();

                    if (reachedDest && !givingStone && thisStone > 0)
                    {
                        Invoke("DeliverStone", 1f);
                        givingStone = true;
                    }
                }
                if (mines.Count > 0)
                {
                    for (int i = mines.Count - 1; i >= 0; i--)
                    {
                        if (mines[i] == null)
                        {
                            mines.RemoveAt(i);
                            distanceMines.RemoveAt(i);
                        }
                    }

                    if (mines.Count == 0)
                    {
                        agent.SetDestination(mineStation.transform.position);

                        DistanceCheck();

                        if (reachedDest && !givingStone && thisStone > 0)
                        {
                            Invoke("DeliverStone", 1f);
                            givingStone = true;
                        }
                        return;
                    }

                    for (int i = 0; i < mines.Count; i++)
                        distanceMines[i] = Vector3.Distance(transform.position, mines[i].transform.position);

                    if (distanceMines.Count > 0)
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
                if (woods.Count == 0)
                {
                    agent.SetDestination(woodStation.transform.position);

                    DistanceCheck();

                    if (reachedDest && !givingWood && thisWood > 0)
                    {
                        Invoke("DeliverWood", 1f);
                        givingWood = true;
                    }
                }
                if (woods.Count > 0)
                {
                    for (int i = woods.Count - 1; i >= 0; i--)
                    {
                        if (woods[i] == null)
                        {
                            woods.RemoveAt(i);
                            distanceWoods.RemoveAt(i);
                        }
                    }

                    if (woods.Count == 0)
                    {
                        agent.SetDestination(woodStation.transform.position);

                        DistanceCheck();

                        if (reachedDest && !givingWood && thisWood > 0)
                        {
                            Invoke("DeliverWood", 1f);
                            givingWood = true;
                        }
                        return;
                    }

                    for (int i = 0; i < woods.Count; i++)
                        distanceWoods[i] = Vector3.Distance(transform.position, woods[i].transform.position);
                    
                    if(distanceWoods.Count > 0)
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
            if(job == 4)
            {
                GameObject[] enemy1 = GameObject.FindGameObjectsWithTag("Enemy1");
                GameObject[] enemy2 = GameObject.FindGameObjectsWithTag("Enemy2");
                GameObject[] enemy3 = GameObject.FindGameObjectsWithTag("Enemy3");
                delStone = false;
                delWood = false;
                indicatorColor.material.SetColor("_BaseColor", Color.red);
                enemies.Clear();
                for (int i = 0; i < enemy1.Length; i++)
                    enemies.Add(enemy1[i]);
                for (int i = 0; i < enemy2.Length; i++)
                    enemies.Add(enemy2[i]);
                for (int i = 0; i < enemy3.Length; i++)
                    enemies.Add(enemy3[i]);
                distEnemies.Clear();
                dotEnemies.Clear();
                for(int i = 0; i < enemies.Count; i++)
                {
                    distEnemies.Add(0);
                    dotEnemies.Add(0);
                    Vector3 forward = transform.TransformDirection(Vector3.forward);
                    Vector3 toOther = enemies[i].transform.position - transform.position;
                    distEnemies[i] = Vector3.Distance(transform.position, enemies[i].transform.position);
                    dotEnemies[i] = Vector3.Dot(forward.normalized, toOther.normalized);
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (distEnemies[i] < 75 && dotEnemies[i] > 0.5)
                    {
                        agent.SetDestination(enemies[i].transform.position);
                        indicatorColor.material.SetColor("_BaseColor", Color.black);
                    }
                    if (distEnemies[i] < 10 && dotEnemies[i] > 0.5)
                    {
                        indicatorColor.material.SetColor("_BaseColor", Color.red);
                        attackingTarget = enemies[i];
                        agent.isStopped = true;
                        agent.ResetPath();
                        isAttacking = true;
                        break;
                    }
                    else
                        isAttacking = false;
                }
                if(isAttacking && !startAttack)
                {
                    startAttack = true;
                    Invoke("Attack", 1f);
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
            if(mines.Count > 0)
            {
                thisStone++;
                gettingStone = false;
                mines[closestMine].GetComponent<HP_System>().health -= 10;
            }
        }

        private void GetWood()
        {
            if(woods.Count > 0)
            {
                thisWood++;
                gettingWood = false;
                woods[closestWood].GetComponent<HP_System>().health -= 10;
            }
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

        private void Attack()
        {
            if(attackingTarget != null)
                attackingTarget.GetComponent<HP_System>().health -= 10;
            startAttack = false;
        }
    }
}
