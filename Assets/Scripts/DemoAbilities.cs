using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using CodeMonkey.Utils;

public class DemoAbilities : MonoBehaviour
{
    public GameObject detPackPrefab;
    public Transform packSpawnPoint;
    public List<GameObject> detPacks = new List<GameObject>();
    public float placingRange = 2;
    public float explosionRadius;
    public float explosionDamage;
    public LayerMask wallLayers;
    private ContactFilter2D wallFilter;
    // Start is called before the first frame update
    void Start()
    {
        wallFilter.layerMask = wallLayers;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LayDetPack(CallbackContext ctx)
    { 
        if(ctx.started)
        {
            GameObject newDetPack;
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, placingRange, wallLayers);
            if(hitInfo)
            {
                float rotationToBeFlatToWall = UtilsClass.GetAngleFromVector(hitInfo.normal) + 90;
                newDetPack = Instantiate(detPackPrefab, hitInfo.point, Quaternion.Euler(new Vector3(0, 0, rotationToBeFlatToWall)));
            }
            else
            {
                newDetPack = Instantiate(detPackPrefab, packSpawnPoint.position, packSpawnPoint.rotation);
            }
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
                    Vector3 explosionCenter = detPack.transform.position + transform.up * explosionRadius/3;
                    Physics2D.OverlapCircle(explosionCenter, explosionRadius, wallFilter, results);
                    for (int i = 0; i < results.Length; i++)
                    {
                        try
                        {
                            results[i].GetComponent<DestructableTilemap>().ExplosionDamage(explosionCenter, explosionRadius, explosionDamage);
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

