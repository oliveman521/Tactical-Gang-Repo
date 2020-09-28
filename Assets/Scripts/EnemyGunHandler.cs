using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunHandler : MonoBehaviour
{
    private float rateOfFireCoutner;
    private GunBehavior equippedGun;
    // Start is called before the first frame update
    void Start()
    {
        equippedGun = GetComponentInChildren<GunBehavior>();
    }

    private void Update()
    {
        rateOfFireCoutner += Time.deltaTime;
    }
    public void TryToShoot()
    {
        if(rateOfFireCoutner > 1/equippedGun.fireRate)
        {
            equippedGun.Shoot();
            rateOfFireCoutner = 0;
        }
    }
}
