using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed;
    public Transform target;
    private Rigidbody2D rb;
    public Vector3 nextDestination;
    public float timeSicnceLastDestination;
    public LayerMask solids;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSicnceLastDestination += Time.deltaTime;
        if ((transform.position - nextDestination).magnitude < .1f || timeSicnceLastDestination > 5) //if we are at our next destination, pick a new one
        {
            nextDestination = PickDestination();
            timeSicnceLastDestination = 0;
        }
        ExecuteMove();
    }

    public void ExecuteMove()
    {
        Vector3 moveDirection = nextDestination - transform.position;
        moveDirection.Normalize();
        rb.velocity = moveDirection * moveSpeed;
    }

    public Vector3 PickDestination()
    {
        Vector3 destination = Vector3.zero;
        if (target)
        {
            if ((target.position - transform.position).magnitude < 5)
            {
                Vector3 targetDirection = target.position - transform.position;
                targetDirection.Normalize();
                destination = targetDirection + transform.position;
            }
        }
        else
        {
            destination = PickRandomDestinationInLineOfSight();
        }
        return destination;
    }
    public Vector3 PickRandomDestinationInLineOfSight()
    {
        Vector3 destination = Vector3.zero;
        for (int i = 0; i < 1000; i++)
        {
            float nextDestRange = UnityEngine.Random.Range(0, 5);
            float nextDestDirectionDeg = UnityEngine.Random.Range(0, 360);
            Vector3 nextDestDirectionVect = RadianToVector2(nextDestDirectionDeg * Mathf.Deg2Rad);
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.left, nextDestRange, solids);
            if (!hitInfo)
            {
                destination = transform.position + nextDestDirectionVect*nextDestRange;
                break;
            }
        }
        return destination;
    }
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }



}
