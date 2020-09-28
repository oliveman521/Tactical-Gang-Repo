using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerGunHandler : MonoBehaviour
{
    public AudioSource noAmmoSound;
    private GunBehavior equippedGun;
    private bool isShooting = false;

    public float maxAmmo;
    public float startAmmo;
    public float ammo;

    private float rateOfFireCoutner;
    // Start is called before the first frame update
    void Start()
    {

        ammo = startAmmo;
        equippedGun = GetComponentInChildren<GunBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        rateOfFireCoutner += Time.deltaTime;

        if (isShooting && ammo >= 1 && rateOfFireCoutner > 1/equippedGun.fireRate)
        {
            equippedGun.Shoot();
            ammo -= 1;
            rateOfFireCoutner = 0;
        }
        else if(isShooting && rateOfFireCoutner > 1 / equippedGun.fireRate)
        {
            noAmmoSound.Play();
            rateOfFireCoutner = 0;
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
            isShooting = true;
        if (ctx.canceled)
            isShooting = false;
    }
}
