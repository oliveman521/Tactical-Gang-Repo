using CodeMonkey.Utils;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public enum EnemyState
{
    Patrolling, //unaware of players. Wanders/looks around for players
    FarCombat, //has LOS on player, closes distance/shoots
    CloseCombat, //has LOS on player, just shoots
    FarTracking, //closes in some distance on the player's last sighted position
    CloseTracking, //watches the player's last sighted position
}
public class EnemyBehavior2 : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public float health;

    [Header("Movement")]
    public float moveAccel = 1;
    private float maxSpeed;
    public float patrollingSpeed;
    public float combatPositioningSpeed;
    public float trackingPositioningSpeed;
    public float rotSpeed;
    public float rotationDifferenceTolerance= 5;
    public float desiredRotation;
    private bool isLookingAtDesiredRotation = false;

    [Header("PathFinding")]
    public float repathInterval = .5f;
    private float repathIntervalCounter = 0;
    private Vector3 pathDestination;
    private Path path;
    private int currentWaypoint = 0;
    private float nextWaypointDistance = .25f;

    [Header("References")]
    public PolygonCollider2D coneOfView;
    private Seeker seeker;
    private List<Transform> pointsOfInterest = new List<Transform>();
    private Rigidbody2D rb;


    [Header("Masks")]
    public LayerMask solids;
    private ContactFilter2D playersFilter;
    public LayerMask playersMask;


    [Header("State Handling")]
    public EnemyState enemyState = EnemyState.Patrolling;
    private EnemyState lastEnemyState = EnemyState.FarCombat;
    private GameObject target = null;
    private Vector3 lastTargetSighting;
    public float timeTilLoseTarget;
    public float timeTilPatrolling;
    public float advanceDist;
    private float timeSincetargetSighting = 0;

    [Header("Patrolling")]
    public float patrolling_LookAroundTime;
    public float patrolling_WaitTime;
    private float patrolling_LookAroundTimeCounter = 0;
    private float patrolling_WaitTimeCounter = 0;
    private Vector3 nextPOI;



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        //callibrate the wait tiem and idle time. They need to be randomly assigned between each look around/wait, so they need initial calibration.
        patrolling_LookAroundTime = UnityEngine.Random.Range(4, 10);
        patrolling_WaitTime = UnityEngine.Random.Range(.3f, 3);

        //assign some componenets from this object
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        //mask calibration
        playersFilter.useLayerMask = true;
        playersFilter.SetLayerMask(playersMask);

        //build list of points of interest
        GameObject[] POIobjects = GameObject.FindGameObjectsWithTag("Point Of Interest");
        for (int i = 0; i < POIobjects.Length; i++)
        {
            pointsOfInterest.Add(POIobjects[i].GetComponent<Transform>());
        }

        //start up cycle to rescan the area around enemies, makes it so other enemies shouldn't path through one-another
        //StartCoroutine(RescanAreaAroundEnemyOnInterval(.3f));
    }

    // Update is called once per frame
    void Update()
    {
        //state calls
        States_CheckForStateChange();
        States_FirstFrameBehavior();
        State_EveryFrameBehavior();

        //universal calls
        TurnTowardsDesiredRotation();
    }

    //state handling and behavior
    void States_CheckForStateChange()
    {
        if (target)
        {
            //update some stuff if player in LOS
            if (CheckIfTargetstillInLOS())
            {
                timeSincetargetSighting = 0;
                lastTargetSighting = target.transform.position;
            }
            else
            {
                timeSincetargetSighting += Time.deltaTime;
            }

            if (Vector3.Distance(transform.position, target.transform.position) < advanceDist)
                enemyState = EnemyState.CloseCombat;
            if (Vector3.Distance(transform.position, target.transform.position) > advanceDist)
                enemyState = EnemyState.FarCombat;

            if (timeSincetargetSighting >= timeTilLoseTarget)
            {
                target = null;
                enemyState = EnemyState.FarTracking;
            }
        }
        else
        {
            target = CheckForPlayersInCone();
            timeSincetargetSighting += Time.deltaTime;

            if (timeSincetargetSighting > timeTilPatrolling)
            {
                lastTargetSighting = Vector3.zero;
                enemyState = EnemyState.Patrolling;
            }

            if (lastTargetSighting != Vector3.zero)
            {
                if (Vector3.Distance(transform.position, lastTargetSighting) < advanceDist)
                    enemyState = EnemyState.CloseTracking;
                if (Vector3.Distance(transform.position, lastTargetSighting) > advanceDist)
                    enemyState = EnemyState.FarTracking;
            }
        }

    }
    void States_FirstFrameBehavior()
    {
        if (enemyState != lastEnemyState)
        {
            switch (enemyState)
            {
                case EnemyState.Patrolling:
                    maxSpeed = patrollingSpeed;
                    SelectPOI();
                    UpdatePath(nextPOI);
                    break;
                case EnemyState.FarCombat:
                    maxSpeed = combatPositioningSpeed;
                    UpdatePath(target.transform.position);
                    break;
                case EnemyState.CloseCombat:
                    maxSpeed = 0;
                    break;
                case EnemyState.FarTracking:
                    maxSpeed = trackingPositioningSpeed;
                    UpdatePath(lastTargetSighting);
                    break;
                case EnemyState.CloseTracking:
                    maxSpeed = 0;
                    break;
            }
        }
        lastEnemyState = enemyState;

    }
    void State_EveryFrameBehavior()
    {
        //states every frame behavior
        switch (enemyState)
        {
            case EnemyState.Patrolling:
                //once reaching destination, look around
                if (Vector3.Distance(pathDestination, transform.position) < .25f)
                {
                    patrolling_LookAroundTimeCounter += Time.deltaTime;
                    if (isLookingAtDesiredRotation)
                    {
                        patrolling_WaitTimeCounter += Time.deltaTime;
                        if (patrolling_WaitTimeCounter > patrolling_WaitTime)
                        {
                            desiredRotation = rb.rotation + UnityEngine.Random.Range(-180, 180);
                            patrolling_WaitTimeCounter = 0;
                            patrolling_WaitTime = UnityEngine.Random.Range(.3f, 3);
                        }
                    }
                    if (patrolling_LookAroundTimeCounter > patrolling_LookAroundTime)
                    {
                        patrolling_LookAroundTimeCounter = 0;
                        patrolling_LookAroundTime = UnityEngine.Random.Range(4, 10);
                        SelectPOI();
                        UpdatePath(nextPOI);
                    }
                }
                else
                {
                    desiredRotation = MoveAlongPath();
                    UpdatePathOnInterval(nextPOI);
                }
                break;
            case EnemyState.FarCombat:
                //go towards the player
                UpdatePathOnInterval(target.transform.position);
                MoveAlongPath();
                //GetComponent<NPCGunHandler>().TryToShoot(new NPCStateData(false,));
                desiredRotation = UtilsClass.GetAngleFromVector(target.transform.position - transform.position) - 90;// + UnityEngine.Random.Range(-20f,20f);
                break;
            case EnemyState.CloseCombat:
                //GetComponent<NPCGunHandler>().TryToShoot(new NPCStateData(false, 0));
                desiredRotation = UtilsClass.GetAngleFromVector(target.transform.position - transform.position) - 90;// + UnityEngine.Random.Range(-20f,20f);
                break;
            case EnemyState.FarTracking:
                UpdatePathOnInterval(lastTargetSighting);
                MoveAlongPath();
                desiredRotation = UtilsClass.GetAngleFromVector(lastTargetSighting - transform.position) - 90;// + UnityEngine.Random.Range(-20f,20f);
                break;
            case EnemyState.CloseTracking:
                desiredRotation = UtilsClass.GetAngleFromVector(lastTargetSighting - transform.position) - 90;// + UnityEngine.Random.Range(-20f,20f);
                break;
        }
    }



    //player detection
    GameObject CheckForPlayersInCone()
    {
        Collider2D[] playersInCone = new Collider2D[12];
        Physics2D.OverlapCollider(coneOfView, playersFilter, playersInCone);
        List<Collider2D> playersInConeLOSandNotDown = new List<Collider2D>();

        foreach (Collider2D playerCollider in playersInCone)
        {
            if (playerCollider)
            {
                Vector2 vectToPlayer = playerCollider.transform.position - transform.position;
                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, vectToPlayer.normalized, vectToPlayer.magnitude, solids);
                bool down = playerCollider.gameObject.GetComponent<PlayerBase>().down;
                if (!hitInfo && !down)
                {
                    playersInConeLOSandNotDown.Add(playerCollider);
                }
            }
        }

        GameObject closestPlayer = null;
        float shortestDist = 1000;

        foreach (Collider2D playerCollider in playersInConeLOSandNotDown)
        {
            if ((playerCollider.transform.position - transform.position).magnitude < shortestDist)
            {
                closestPlayer = playerCollider.gameObject;
                shortestDist = (playerCollider.transform.position - transform.position).magnitude;
            }
        }
        return closestPlayer;
    }
    bool CheckIfTargetstillInLOS()
    {
        Vector2 vectToTarget = target.transform.position - transform.position;
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, vectToTarget.normalized, vectToTarget.magnitude, solids);
        bool down = target.GetComponent<PlayerBase>().down;
        if (!hitInfo && !down)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Misc.
    void SelectPOI() //out of the POE's, select one weighted by inverse of distance
    {
        float totalDesireability = 0;
        List<float> desirabilityList = new List<float>();
        foreach (Transform poi in pointsOfInterest)
        {
            float desirablility = 1/Vector3.Distance(transform.position, poi.position);
            if (desirablility > 1f)
            {
                desirablility = 0f;
            }
            desirabilityList.Add(desirablility);
            totalDesireability += desirablility;
        }
        float randomVal = UnityEngine.Random.Range(0f, totalDesireability);
        float runningVal = 0;
        for (int i = 0; i < pointsOfInterest.Count; i++)
        {
            runningVal += desirabilityList[i];
            if (randomVal < runningVal)
            {
                nextPOI = pointsOfInterest[i].position;
                break;
            }
        }
    }
    void TurnTowardsDesiredRotation()
    {
        float difInAngles = Mathf.DeltaAngle(rb.rotation, desiredRotation);
        if(Mathf.Abs(difInAngles) > .1f)
        {
            float rotAmount = Mathf.Sign(difInAngles) * (rotSpeed * Time.deltaTime);
            if (Mathf.Abs(difInAngles) < Mathf.Abs(rotAmount))
            {
                rotAmount = difInAngles;
                isLookingAtDesiredRotation = true;
            }
            else
            {
                isLookingAtDesiredRotation = false;
            }
            rb.rotation += rotAmount;
        }
    }

    //pathfinding
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            pathDestination = path.vectorPath[path.vectorPath.Count - 1];
            
            //reject paths that can't actually be navigated to. Regenerates path
            float distFromDesiredDestinationToWherePathActuallyTermniates = Vector3.Distance(pathDestination, nextPOI);
            if(distFromDesiredDestinationToWherePathActuallyTermniates > .4)
            {
                Debug.Log(this.gameObject.name + "Rejected path to" + nextPOI);
                SelectPOI();
                UpdatePath(nextPOI);
            }
        }
    }
    void UpdatePathOnInterval(Vector3 destination)
    {
        repathIntervalCounter += Time.deltaTime;
        if (repathIntervalCounter >= repathInterval && seeker.IsDone())
        {
            UpdatePath(destination);
            repathIntervalCounter = 0;
        }
    }
    void UpdatePath(Vector3 destination)
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, destination, OnPathComplete);
        }
    }
    float MoveAlongPath()
    {
        if (path == null)
            return 0;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return 0;
        }

        //calculate direction and apply velocity
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        float angleOfMovement = UtilsClass.GetAngleFromVector(direction) - 90;
        float maxSpeedModifier = 1;
        if(enemyState == EnemyState.Patrolling)
        {
            float angularDifference = Mathf.DeltaAngle(rb.rotation, angleOfMovement);
            if (angularDifference < 0)
                angularDifference += 360;
            maxSpeedModifier = UtilsClass.Remap(angularDifference, 0, 360, 1, .05f);
        }
        rb.velocity += direction * moveAccel;
        if (rb.velocity.magnitude > maxSpeed * maxSpeedModifier)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed * maxSpeedModifier;
        }

        //calculate distance and check if next waypoint should be selected
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        //return angle of movement. Can be used for setting desiredAngle;
        return angleOfMovement;
    }
    IEnumerator RescanAreaAroundEnemyOnInterval(float interval)
    {
        while (true)
        {
            RescanArea(transform.position - transform.up*1.5f, new Vector3(1.5f,1.5f,1));
            yield return new WaitForSeconds(interval);
        }
    }
    private void RescanArea(Vector3 center, Vector3 dimensions)
    {
        Bounds rescanBounds = new Bounds(center, dimensions);
        var guo = new GraphUpdateObject(rescanBounds);
        // Set some settings
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
    }

    //health & dmg
    public void Damage(float dmg, GameObject shooter)
    {
        health -= dmg;
        if(health <= 0)
        {
            Die();
        }
        target = shooter;
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }
}
