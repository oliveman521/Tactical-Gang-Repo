using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerBase))]
public class MedicAbilities : MonoBehaviour
{
    public LayerMask healableLayers;
    private PlayerBase playerBase;
    public float maxHealingPoints;
    public float healingPoints;
    private bool pulseCharging = false;
    private float timeSinceChargeStart;
    public float maxChargeTime;
    public float maxHealAmount;
    public float healRadius;
    // Start is called before the first frame update
    void Start()
    {
        playerBase = GetComponent<PlayerBase>();
    }

    void Update()
    {
        if(pulseCharging)
        {
            timeSinceChargeStart += Time.deltaTime;
        }
    }
    // Update is called once per frame
    void HealingPulse(CallbackContext ctx)
    {
        if(ctx.started)
        {
            pulseCharging = true;
            timeSinceChargeStart = 0;
        }
        if(ctx.canceled)
        {
            pulseCharging = false;
            float chargeAmount = timeSinceChargeStart / maxChargeTime;
            if(chargeAmount > 1)
                chargeAmount = 1;
            float healAmount = chargeAmount * maxHealAmount;

            Physics2D.OverlapCircle(transform.position, healRadius, );
        }
    }
}
