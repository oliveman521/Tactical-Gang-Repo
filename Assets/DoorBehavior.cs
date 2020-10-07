using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    private HingeJoint2D hinge;
    public float desiredAngle;
    public float rangeOfMotion = 330; //the door's range of motion in degrees ceentered on its desired position;
    public float angleAllowance = 3;
    public float closingSpeed = 20;
    public float currentRotation;
    public float timeTilClose = 1;
    private float timeTilCloseCounter;
    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        desiredAngle = transform.rotation.eulerAngles.z;

        JointAngleLimits2D limits = hinge.limits;
        limits.min = desiredAngle - rangeOfMotion/2;
        limits.max = desiredAngle + rangeOfMotion / 2;
        hinge.limits = limits;
    }

    // Update is called once per frame
    void Update()
    {
        GentlySwingClosed();
    }

    void GentlySwingClosed()
    {
        currentRotation = transform.rotation.eulerAngles.z;
        while(currentRotation > 180)
        {
            currentRotation -= 360;
        }
        JointMotor2D motor = hinge.motor;
        if (currentRotation < desiredAngle - angleAllowance / 2) //angle is less than what it needs to be
        {
            timeTilCloseCounter += Time.deltaTime;
            if (timeTilCloseCounter >= timeTilClose)
            {
                motor.motorSpeed = -closingSpeed;
            }
        }
        else if(currentRotation > desiredAngle + angleAllowance/2) //angle is what than what it needs to be
        {
            timeTilCloseCounter += Time.deltaTime;
            if (timeTilCloseCounter >= timeTilClose)
            {
                motor.motorSpeed = closingSpeed;
            }
        }
        else
        {
            motor.motorSpeed = 0;
            timeTilCloseCounter = 0;
        }
        hinge.motor = motor;
    }
}
