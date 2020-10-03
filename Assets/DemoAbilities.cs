using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class DemoAbilities : MonoBehaviour
{
    public GameObject detPackPrefab;
    public Transform packSpawnPoint;
    public List<GameObject> detPacks = new List<GameObject>();
    public float explosionRadius;
    public float explosionDamage;
    public LayerMask explosionHitableLayers;
    private ContactFilter2D hitableLayersFilter;
    // Start is called before the first frame update
    void Start()
    {
        hitableLayersFilter.layerMask = explosionHitableLayers;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LayDetPack(CallbackContext ctx)
    { 
        if(ctx.started)
        {
            GameObject newDetPack = Instantiate(detPackPrefab,packSpawnPoint.position,packSpawnPoint.rotation);
            detPacks.Add(newDetPack);
        }
    }
    public void DetonateDetPacks(CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (detPacks.Count > 0)
            {
                foreach (GameObject detPack in detPacks)
                {
                    Collider2D[] results = new Collider2D[10];
                    Physics2D.OverlapCircle(detPack.transform.position, explosionRadius, hitableLayersFilter, results);
                    for (int i = 0; i < results.Length; i++)
                    {
                        try
                        {
                            results[i].GetComponent<DestructableTilemap>().ExlosionBlockDamage(detPack.transform.position, explosionRadius);
                        }
                        catch { }
                        try
                        {
                            results[i].GetComponent<EnemyBehavior2>().Damage(explosionDamage, null);
                        }
                        catch { }
                    }
                    Destroy(detPack);
                }
                detPacks = new List<GameObject>();
            }
        }
    }
}

