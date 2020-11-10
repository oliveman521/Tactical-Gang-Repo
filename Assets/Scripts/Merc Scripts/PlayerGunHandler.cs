using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerBase))]
[RequireComponent(typeof(PlayerSupplyManager))]
public class PlayerGunHandler : MonoBehaviour
{
    private PlayerBase playerBase;
    private GunBehavior equippedGun;
    private PlayerSupplyManager supplyManager;
    private bool isShooting = false;

    public float suppliesPerShot = .5f;

    private bool fullAuto;
    private bool hasShot = false;
    private float rateOfFireCoutner;
    // Start is called before the first frame update
    void Start()
    {
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
        playerBase = GetComponent<PlayerBase>();
        supplyManager = GetComponent<PlayerSupplyManager>();

    }
    // Update is called once per frame
    void Update()
    {
        rateOfFireCoutner += Time.deltaTime;

        if (hasShot == false || fullAuto == true) //check for full/semi auto settings,
        {
            if (isShooting && rateOfFireCoutner > 1 / equippedGun.fireRate) //shoot if   1. player is pulling trigger   2. PLayer has ammo    3. Enough time has passed since the last shot
            {
                if (supplyManager.UseSupplies(suppliesPerShot))
                {
                    equippedGun.Shoot();
                    rateOfFireCoutner = 0;
                    hasShot = true;
                }
                else
                {
                    equippedGun.PlayNoAmmoSound();
                    rateOfFireCoutner = 0;
                    hasShot = true;
                }
            }
        }
    }
    public void OnShoot(CallbackContext ctx)
    {
        if (ctx.performed && playerBase.down == false)
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
