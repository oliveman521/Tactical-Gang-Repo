using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    public LineRenderer laser;
    public GunBehavior parentGun;
    private Transform firepoint;
    private LayerMask hitMask;
    private float range;
    // Start is called before the first frame update
    void Start()
    {
        hitMask = parentGun.bulletHitMask;
        firepoint = parentGun.firePoint;
        range = parentGun.range;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo =  Physics2D.Raycast(firepoint.position, firepoint.up, range, hitMask);
        if(hitInfo)
        {
            laser.SetPosition(0, firepoint.position);
            laser.SetPosition(1, hitInfo.point);
        }
        else
        {
            laser.SetPosition(0, firepoint.position);
            laser.SetPosition(1, firepoint.position + firepoint.up*range);
        }
    }
}
