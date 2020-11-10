using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGunHandler : MonoBehaviour
{
    private float rateOfFireCoutner;
    private float timeSinceShootingStart;
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
    public void TryToShoot(NPCStateData stateData, float delayBeforeFirstShot)
    {
        if(stateData.initializing == true)
        {
            timeSinceShootingStart = 0;
        }
        if(timeSinceShootingStart < delayBeforeFirstShot)
        {
            timeSinceShootingStart += Time.deltaTime;
            return;
        }
        if (rateOfFireCoutner > 1/equippedGun.fireRate)
        {
            equippedGun.Shoot();
            rateOfFireCoutner = 0;
        }
    }
}
