using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSupplyManager : MonoBehaviour
{
    public float supplies = 10;
    public float maxSupplies = 15;
    private PlayerBase playerBase;

    private void Start()
    {
        playerBase = GetComponent<PlayerBase>();
    }
    public void OnCollisionEnter2D(Collision2D collision)  //pickup supply packs
    {
        if (collision.gameObject.CompareTag("Supply Pack"))
        {
            float supplyCount = collision.gameObject.GetComponent<SupplyPack>().supplyCount;
            float overflow = (supplies + supplyCount) - maxSupplies;
            if (overflow > 0)
            {
                supplies = maxSupplies;
                collision.gameObject.GetComponent<SupplyPack>().supplyCount = overflow;
            }
            else
            {
                supplies += supplyCount;
                Destroy(collision.gameObject);
            }
        }
    }

    public bool UseSupplies(float useAmmount)
    {
        float newTotal = supplies - useAmmount;
        if(newTotal < 0) //not enough supplies to perform action
        {
            return false;
        }
        else //enough supplies. Adjust total
        {
            playerBase.SpawnDamgeNumber(-useAmmount, transform.position, false);
            supplies = newTotal;
            return true;
        }
    }

}
