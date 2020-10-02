using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerGunHandler : MonoBehaviour
{
    private GunBehavior equippedGun;
    private bool isShooting = false;

    public float maxAmmo;
    public float startAmmo;
    public float ammo;

    private bool fullAuto;
    private bool hasShot = false;

    private float rateOfFireCoutner;
    // Start is called before the first frame update
    void Start()
    {

        ammo = startAmmo;
        try
        {
            equippedGun = GetComponentInChildren<GunBehavior>();
            fullAuto = equippedGun.fullAuto;
        }
        catch
        {
            Debug.Log("No gun found");
            Destroy(this);
        }


    }

    // Update is called once per frame
    void Update()
    {
        rateOfFireCoutner += Time.deltaTime;

        if (hasShot == false || fullAuto == true) //check for full/semi auto settings
        {
            
            if (isShooting && ammo >= 1 && rateOfFireCoutner > 1 / equippedGun.fireRate) //shoot if   1. player is pulling trigger   2. PLayer has ammo    3. Enough time has passed since the last shot
            {

                equippedGun.Shoot();
                ammo -= 1;
                rateOfFireCoutner = 0;
                hasShot = true;

            }
            else if (isShooting && rateOfFireCoutner > 1 / equippedGun.fireRate)
            {
                equippedGun.PlayNoAmmoSound();
                rateOfFireCoutner = 0;
                hasShot = true;
            }
            
        }
    }

    public void AddAmmo(float amnt)
    {
        ammo += amnt;
        if(ammo > maxAmmo)
        {
            ammo = maxAmmo;
        }
    }
    public void OnShoot(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isShooting = true;
        }
        if (ctx.canceled)
        {
            isShooting = false;
            hasShot = false;
        }
    }
}
