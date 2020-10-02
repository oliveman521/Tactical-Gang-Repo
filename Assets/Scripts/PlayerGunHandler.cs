using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;

public class PlayerGunHandler : MonoBehaviour
{
    private GunBehavior equippedGun;
    private bool isShooting = false;

    public float maxAmmo = 15;
    public float startAmmo = 10;
    public float ammo;

    public float ammoPerSupply = 2;

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
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Supply Pack"))
        {
            float supplyCount = collision.gameObject.GetComponent<SupplyPack>().supplyCount;
            float overflow = (ammo + supplyCount*ammoPerSupply) - maxAmmo;
            if (overflow > 0)
            {
                ammo = maxAmmo;
                collision.gameObject.GetComponent<SupplyPack>().supplyCount = overflow/ammoPerSupply;
            }
            else
            {
                ammo += supplyCount;
                Destroy(collision.gameObject);
            }
        }
    } //pickup supply packs
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
