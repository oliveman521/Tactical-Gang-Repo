using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using CodeMonkey.Utils;

[RequireComponent(typeof(Seeker))]
public class NPCPathfinding : MonoBehaviour
{
    [Header("PathFinding")]
    public float repathInterval = .5f;
    private float repathIntervalCounter = 0;
    public Vector3 pathDestination;
    private Path currentPath;
    private int currentWaypoint = 0;
    private float nextWaypointDistance = .35f;
    private Seeker seeker;
    private Vector3 targetDestination;
    private Rigidbody2D rb;

    private List<Transform> pointsOfInterest = new List<Transform>();

    public Vector2 directionOfPathVect 
    {
        get 
        {
            if (currentPath == null)
                return Vector2.zero;
            if (currentWaypoint >= currentPath.vectorPath.Count)
                return Vector2.zero;

            //calculate distance and check if next waypoint should be selected
            float distance = Vector2.Distance(rb.position, currentPath.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance && currentWaypoint < currentPath.vectorPath.Count - 1)
            {
                currentWaypoint++;
            }

            Vector2 direction = ((Vector2)currentPath.vectorPath[currentWaypoint] - rb.position).normalized;
            return direction;
        }  
    }
    public float directionOfPathAngle
    {
        get
        {
            float direction = UtilsClass.GetAngleFromVector(directionOfPathVect) - 90;
            return direction;
        }
    }
    public float distanceFromDestination { get { return Vector2.Distance(transform.position, pathDestination); } }

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        //pathfinding
        GameObject[] POIobjects = GameObject.FindGameObjectsWithTag("Point Of Interest");
        for (int i = 0; i < POIobjects.Length; i++)
        {
            pointsOfInterest.Add(POIobjects[i].GetComponent<Transform>());
        }
    }
    public void UpdatePath(Vector3 destination)
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, destination, OnPathComplete);
            targetDestination = destination;
            repathIntervalCounter = 0;
        }
    }
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            currentPath = p;
            currentWaypoint = 0;
            pathDestination = currentPath.vectorPath[currentPath.vectorPath.Count - 1];

            //reject paths that can't actually be navigated to. Regenerates path
            float distFromDesiredDestinationToWherePathActuallyTermniates = Vector3.Distance(pathDestination, targetDestination);
            if (distFromDesiredDestinationToWherePathActuallyTermniates > .4)
            {
                Debug.Log(this.gameObject.name + "Rejected path to" + targetDestination);
                PathToPOI();
            }
        }
    }
    public void UpdatePathOnInterval(Vector3 destination)
    {
        repathIntervalCounter += Time.deltaTime;
        if (repathIntervalCounter >= repathInterval && seeker.IsDone())
        {
            UpdatePath(destination);
        }
    }
    public void PathToPOI() //out of the POE's, select one weighted by inverse of distance
    {
        float totalDesireability = 0;
        List<float> desirabilityList = new List<float>();
        foreach (Transform poi in pointsOfInterest)
        {
            float desirablility = 1 / Vector3.Distance(transform.position, poi.position);
            if (desirablility > 1f)
            {
                desirablility = 0f;
            }
            desirabilityList.Add(desirablility);
            totalDesireability += desirablility;
        }
        float randomVal = Random.Range(0f, totalDesireability);
        float runningVal = 0;
        for (int i = 0; i < pointsOfInterest.Count; i++)
        {
            runningVal += desirabilityList[i];
            if (randomVal < runningVal)
            {
                UpdatePath(pointsOfInterest[i].position);
                return;
            }
        }
    }
}
