using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;
using System;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NPCPathfinding))]
public class NPCBase : MonoBehaviour
{
    [Header("Health")]
    public float health = 2;
    public float maxHealth = 2;
    public float desiredRotation;

    [Header("Masks")]
    public LayerMask solids;
    private ContactFilter2D playersFilter;
    public LayerMask playersMask;

    [Header("References")]
    public PolygonCollider2D coneOfView;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public NPCPathfinding pathfinding;

    [Header("Targeting")]
    public GameObject target = null;
    public Vector3 lastTargetSighting = Vector3.zero;
    public float timeTilLoseTarget = .5f;
    public float timeTilPatrolling = 10f;
    public float advanceDist = 3;
    private float timeSinceTargetSighting = 1000;


    [System.Serializable]
    public class NPCStateEvent : UnityEvent2<NPCStateData> { }


    private NPCStateEvent currentStateEvent = null;
    private NPCStateEvent lastStateEvent = null;

    [SerializeField] private NPCStateEvent NoTarget = null;
    [SerializeField] private NPCStateEvent TrackingClose = null;
    [SerializeField] private NPCStateEvent TrackingFar = null;
    [SerializeField] private NPCStateEvent CombatClose = null;
    [SerializeField] private NPCStateEvent CombatFar = null;



    // Start is called before the first frame update
    void Start()
    {
        //attatch local components
        rb = GetComponent<Rigidbody2D>();
        pathfinding = GetComponent<NPCPathfinding>();

        //Build filter for player
        playersFilter.SetLayerMask(playersMask);
    }

    private void Update()
    {
        if(target)
        {
            //See if another target is more eligible for attack
            GameObject newTarget = CheckForPlayersInCone();
            if (newTarget != null)
                target = newTarget;
            //Lose the target after not seen for some time
            if (CheckIfTargetstillInLOS())
            {
                timeSinceTargetSighting = 0;
                lastTargetSighting = target.transform.position;
            }
            else
            {
                timeSinceTargetSighting += Time.deltaTime;
                if (timeSinceTargetSighting >= timeTilLoseTarget)
                    target = null;
            }
        }
        else
        {
            target = CheckForPlayersInCone();
            timeSinceTargetSighting += Time.deltaTime;
        }


        //Based on targeting results, trigger some events
        if (target)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= advanceDist) //Close Combat
                currentStateEvent = CombatClose;
            else if (Vector3.Distance(transform.position, target.transform.position) > advanceDist) //Far Combat
                currentStateEvent = CombatFar;
            else
                Debug.Log("Error - enemy didn't pick a state");
        }
        else
        {
            if (timeSinceTargetSighting > timeTilPatrolling)
                currentStateEvent = NoTarget;
            else if (Vector3.Distance(transform.position, lastTargetSighting) <= advanceDist)
                currentStateEvent = TrackingClose;
            else if (Vector3.Distance(transform.position, lastTargetSighting) > advanceDist)
                currentStateEvent = TrackingFar;
            else
                Debug.Log("Error - enemy didn't pick a state");
        }

        if(lastStateEvent == currentStateEvent)//same event as last time
        { 
            currentStateEvent.Invoke(new NPCStateData(false, this));
        }
        else //this is the first time the event is being called
        {
            currentStateEvent.Invoke(new NPCStateData(true, this));
        }

        lastStateEvent = currentStateEvent;
    }

    //Functions to help with Targeting
    public GameObject CheckForPlayersInCone()
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
    public bool CheckIfTargetstillInLOS()
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
   

    //movement & rotation
    public bool TurnTowardsAngle(float rotation, float turnSpeed)
    {
        desiredRotation = rotation;
        bool isLookingAtDesiredRotation = true;
        float difInAngles = Mathf.DeltaAngle(rb.rotation, desiredRotation);
        if (Mathf.Abs(difInAngles) > .1f)
        {
            float rotAmount = Mathf.Sign(difInAngles) * (turnSpeed * Time.deltaTime);
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
        return isLookingAtDesiredRotation;
    }
    public bool TurnTowardsPoint(Vector3 point, float turnSpeed)
    {
        desiredRotation = UtilsClass.GetAngleFromVector(point - transform.position) - 90;
        bool isLookingAtDesiredRotation = true;
        float difInAngles = Mathf.DeltaAngle(rb.rotation, desiredRotation);
        if (Mathf.Abs(difInAngles) > .1f)
        {
            float rotAmount = Mathf.Sign(difInAngles) * (turnSpeed * Time.deltaTime);
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
        return isLookingAtDesiredRotation;
    }
    public void MoveAlongPath(float maxSpeed, float moveAccel, bool slowWhenTurning)
    {

        Vector2 direction = pathfinding.directionOfPathVect;
        if (direction == Vector2.zero)
            return;
        float maxSpeedModifier = 1;

        if (slowWhenTurning)
        {
            float angularDifference = Mathf.Abs(Mathf.DeltaAngle(rb.rotation, desiredRotation));
 
            maxSpeedModifier = UtilsClass.Remap(angularDifference, 0, 180, 1, .05f);
        }

        rb.velocity += direction * moveAccel * Time.deltaTime;
        if (rb.velocity.magnitude > maxSpeed * maxSpeedModifier)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed * maxSpeedModifier;
        }

    }


    //Health & Dmg
    public void Damage(float dmg, GameObject shooter)
    {
        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
        //target = shooter;
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }


}
public struct NPCStateData
{
    public NPCStateData(bool _initializing, NPCBase _npcBase)
    {
        initializing = _initializing;
        npcBase = _npcBase;
    }
    public bool initializing;
    public NPCBase npcBase;
}