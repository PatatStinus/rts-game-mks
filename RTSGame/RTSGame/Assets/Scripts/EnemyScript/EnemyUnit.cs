using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyUnit : MonoBehaviour
{
    #region Variables
    #region General
    public int team;
    public int job = 1;
    private AIManager addResources;
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
    private int thisStone;
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
    private int thisWood;
    private bool delWood;
    private bool givingWood;
    #endregion

    #region Knight
    private bool isAttacking;
    private bool startAttack;
    private GameObject attackingTarget;
    private GameObject[] enemies1;
    private GameObject[] enemies2;
    private GameObject[] enemies3;
    private GameObject[] player;
    private List<float> distEnemies = new List<float>();
    private List<float> dotEnemies = new List<float>();
    private List<GameObject> allEnemies = new List<GameObject>();
    private float distances;
    private bool gettingPos = false;
    #endregion
    #endregion

    private void Start()
    {
        gameObject.tag = $"Enemy{team}";

        agent = GetComponent<NavMeshAgent>();

        farmParent = GameObject.FindGameObjectWithTag($"Food{team}");
        miningParent = GameObject.FindGameObjectWithTag("Rock");
        woodParent = GameObject.FindGameObjectWithTag("Wood");

        totalFarms = farmParent.transform.childCount;
        totalMines = miningParent.transform.childCount;
        totalWoods = woodParent.transform.childCount;

        addResources = GameObject.FindGameObjectWithTag($"Team{team}").GetComponent<AIManager>();

        woodStation = woodParent.transform.GetChild(team).gameObject;
        mineStation = miningParent.transform.GetChild(team).gameObject;
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

        switch (team)
        {
            case 1:
                GetComponent<Renderer>().material.color = Color.blue;
                break;
            case 2:
                GetComponent<Renderer>().material.color = Color.white;
                break;
            case 3:
                GetComponent<Renderer>().material.color = Color.magenta;
                break;
        }

        job = Random.Range(1, 5);
    }

    private void Update()
    {
        totalFarms = farmParent.transform.childCount;
        farms.Clear();
        distanceFarms.Clear();
        for (int i = 0; i < totalFarms; i++)
        {
            farms.Add(farmParent.transform.GetChild(i).gameObject);
            distanceFarms.Add(0);
        }
        //1 = Farming
        //2 = Mining
        //3 = Chopping wood
        //4 = Knight

        if (job == 1)
        {
            delStone = false;
            delWood = false;
            if (farms.Count > 0)
            {
                for (int i = 0; i < farms.Count; i++)
                    distanceFarms[i] = Vector3.Distance(transform.position, farms[i].transform.position);

                closestFarm = distanceFarms.IndexOf(distanceFarms.Min());

                agent.SetDestination(farms[closestFarm].transform.position);

                DistanceCheck();

                if (reachedDest && !gettingFood)
                {
                    Invoke("GetFood", 3f);
                    gettingFood = true;
                }
            }
        }
        if (job == 2)
        {
            delWood = false;
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
        if (job == 3)
        {
            delStone = false;
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

                if (distanceWoods.Count > 0)
                    closestWood = distanceWoods.IndexOf(distanceWoods.Min());

                agent.SetDestination(woods[closestWood].transform.position);

                DistanceCheck();

                if (reachedDest && !gettingWood && thisWood < 10 && !delWood)
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
        if (job == 4)
        {
            delStone = false;
            delWood = false;
            enemies1 = GameObject.FindGameObjectsWithTag("Enemy1");
            enemies2 = GameObject.FindGameObjectsWithTag("Enemy2");
            enemies3 = GameObject.FindGameObjectsWithTag("Enemy3");
            player = GameObject.FindGameObjectsWithTag("Player");
            distEnemies.Clear();
            dotEnemies.Clear();
            allEnemies.Clear();
            if(team != 1)
            {
                for (int i = 0; i < enemies1.Length; i++)
                    allEnemies.Add(enemies1[i]);
            }
            #region CopyPaste
            if(team != 2)
            {
                for (int i = 0; i < enemies2.Length; i++)
                    allEnemies.Add(enemies2[i]);
            }
            if(team != 3)
            {
                for (int i = 0; i < enemies3.Length; i++)
                    allEnemies.Add(enemies3[i]);
            }
            for (int i = 0; i < player.Length; i++)
            {
                allEnemies.Add(player[i]);
            }
            for(int i = 0; i < allEnemies.Count; i++)
            {
                distEnemies.Add(0);
                dotEnemies.Add(0);
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 toOther = allEnemies[i].transform.position - transform.position;
                distEnemies[i] = Vector3.Distance(transform.position, allEnemies[i].transform.position);
                dotEnemies[i] = Vector3.Dot(forward.normalized, toOther.normalized);
            }
            for (int i = 0; i < allEnemies.Count; i++)
            {
                if (distEnemies[i] < 75 && dotEnemies[i] > 0.5)
                {
                    agent.SetDestination(allEnemies[i].transform.position);
                }

                if (distEnemies[i] < 10 && dotEnemies[i] > 0.5)
                {
                    attackingTarget = allEnemies[i];
                    agent.isStopped = true;
                    isAttacking = true;
                    break;
                }
                else
                {
                    isAttacking = false;
                    attackingTarget = null;
                }
            }
#endregion
            if (isAttacking && !startAttack)
            {
                startAttack = true;
                Invoke("Attack", 1f);
            }
            if (!gettingPos)
            {
                GetEnemyPosition();
                gettingPos = true;
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
        if (mines.Count > 0)
        {
            thisStone++;
            gettingStone = false;
            mines[closestMine].GetComponent<HP_System>().health -= 10;
        }
    }

    private void GetWood()
    {
        if (woods.Count > 0)
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
        if (Vector3.Distance(transform.position, agent.destination) < 10)
            reachedDest = true;
        if (Vector3.Distance(transform.position, agent.destination) > 10)
            reachedDest = false;
    }

    #region KnightMoves
    private void Attack()
    {
        if (attackingTarget != null)
            attackingTarget.GetComponent<HP_System>().health -= 10;
        startAttack = false;
    }

    private void GetEnemyPosition()
    {
        Invoke("GetEnemyPosition", 60f);

        if(distEnemies.Count != 0 && distEnemies.Min() != 0)
            agent.SetDestination(allEnemies[distEnemies.IndexOf(distEnemies.Min())].transform.position);
    }
    #endregion
}
