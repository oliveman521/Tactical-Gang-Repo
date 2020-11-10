using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Reflection;

public class BasicNPCBehaviors : MonoBehaviour
{
    void Start()
    {
        
    }


    public void MoveTowardsTarget(NPCStateData stateData, float maxSpeed, float acceleration)
    {
        if(stateData.initializing == true)//code to be run on start of this state
        {
            stateData.npcBase.pathfinding.UpdatePath(stateData.npcBase.target.transform.position);
        }


        stateData.npcBase.pathfinding.UpdatePathOnInterval(stateData.npcBase.target.transform.position);
        stateData.npcBase.MoveAlongPath(maxSpeed, acceleration, false);
    }
    public void MoveTowardsLastSighting(NPCStateData stateData, float maxSpeed, float acceleration)
    {
        if (stateData.initializing == true) //code to be run on start of this state
        {
            stateData.npcBase.pathfinding.UpdatePath(stateData.npcBase.lastTargetSighting);
        }
        stateData.npcBase.pathfinding.UpdatePathOnInterval(stateData.npcBase.lastTargetSighting);
        stateData.npcBase.MoveAlongPath(maxSpeed, acceleration, false);
    }
    public void TurnToTarget(NPCStateData stateData, float turnSpeed)
    {
        stateData.npcBase.TurnTowardsPoint(stateData.npcBase.target.transform.position, turnSpeed);
    }
    public void TurnToLastSighting(NPCStateData stateData, float turnSpeed)
    {
        stateData.npcBase.TurnTowardsPoint(stateData.npcBase.lastTargetSighting, turnSpeed);
    }

    private float timeWaitingAtPOI;
    private float timeWaitingAtPOICounter;
    private bool turningToFaceNewPath;
    public void PatrolPOIs(NPCStateData stateData, float maxSpeed, float acceleration, float turnSpeed)
    {
        if (stateData.initializing == true) //code to be run on start of this state
        {
            stateData.npcBase.pathfinding.PathToPOI();
            timeWaitingAtPOI = UnityEngine.Random.Range(5, 10);
        }

        if (stateData.npcBase.pathfinding.distanceFromDestination < .25f)
        {
            timeWaitingAtPOICounter += Time.deltaTime;

            LookAround(stateData, turnSpeed);

            if (timeWaitingAtPOICounter >= timeWaitingAtPOI)
            {
                timeWaitingAtPOICounter = 0;
                timeWaitingAtPOI = UnityEngine.Random.Range(4, 10);
                stateData.npcBase.pathfinding.PathToPOI();
                turningToFaceNewPath = true;
            }
        }
        else
        {
            if(turningToFaceNewPath == false)
            {
                stateData.npcBase.pathfinding.UpdatePathOnInterval(stateData.npcBase.pathfinding.pathDestination);
                stateData.npcBase.MoveAlongPath(maxSpeed, acceleration, true);
                stateData.npcBase.TurnTowardsAngle(stateData.npcBase.pathfinding.directionOfPathAngle, turnSpeed);
            }
            else
            {
                turningToFaceNewPath = !stateData.npcBase.TurnTowardsAngle(stateData.npcBase.pathfinding.directionOfPathAngle, turnSpeed/2);
            }
        }
    }
    private float pauseTime;
    private float pauseTimeCounter;
    private float targetAngle;
    private bool isLookingAtTargetAngle;
    public void LookAround(NPCStateData stateData, float turnSpeed)
    {
        if(isLookingAtTargetAngle || stateData.initializing)
        {
            isLookingAtTargetAngle = false;
            targetAngle = UnityEngine.Random.Range(-90, 90) + stateData.npcBase.rb.rotation;
            pauseTime = UnityEngine.Random.Range(1, 3);
            pauseTimeCounter = 0;
        }
        pauseTimeCounter += Time.deltaTime;
        if(pauseTimeCounter >= pauseTime)
        {
            isLookingAtTargetAngle = stateData.npcBase.TurnTowardsAngle(targetAngle, turnSpeed);
        }
    }

}
