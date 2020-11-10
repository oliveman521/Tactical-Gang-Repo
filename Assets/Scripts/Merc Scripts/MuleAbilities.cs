using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class MuleAbilities : MonoBehaviour
{
    public float supplyTotal;
    public float suppliesGainedPerCache;
    public float supplyThrownPerPack;
    public GameObject supplyPack;
    public Transform throwPoint;
    public float throwVelocity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cache"))
        {
            supplyTotal += suppliesGainedPerCache;
            Destroy(collision.gameObject);
        }
    }

    public void ThrowSupplies(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (supplyTotal > 0)
            {
                float supplyCountThrown;
                if (supplyTotal < supplyThrownPerPack)
                {
                    supplyCountThrown = supplyTotal;
                    supplyTotal = 0;
                }
                else
                {
                    supplyCountThrown = supplyThrownPerPack;
                    supplyTotal -= supplyThrownPerPack;
                }
                GameObject newSupplyPack = Instantiate(supplyPack, throwPoint.position, throwPoint.rotation);
                newSupplyPack.GetComponent<Rigidbody2D>().velocity = throwPoint.up * throwVelocity;
                newSupplyPack.GetComponent<SupplyPack>().supplyCount = supplyCountThrown;
            }
        }
    }
}
