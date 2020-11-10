using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundScreenRandomly : MonoBehaviour
{
    public Camera cam;
    float left;
    float right;
    float top;
    float bottom;
    Vector3 targetLocation;
    public float moveSpeed = 6;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 botLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector2 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
        left = botLeft.x;
        bottom = botLeft.y;
        right = topRight.x;
        top = topRight.y;
        targetLocation = PickPosition();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (targetLocation - transform.position).normalized;
        //rb.rotation = Mathf.Atan2(direction.y, direction.x) - 180;
        rb.velocity = (Vector2)(direction * moveSpeed);
        if (Vector3.Distance(transform.position, targetLocation) < .25)
        {
            targetLocation = PickPosition();
        }
    }

    Vector3 PickPosition()
    {
        return new Vector3(Random.Range(left, right), Random.Range(bottom, top), 0);
    }
}
