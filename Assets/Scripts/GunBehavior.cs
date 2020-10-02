using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CodeMonkey.Utils;
using TMPro;

public class GunBehavior : MonoBehaviour
{
    public bool fullAuto = true;
    public float bulletDamage;
    public float bulletKnockBack;
    public float fireRate;
    private float fireRateCounter;
    public float trailFadeTime;
    public Transform firePoint;
    public LineRenderer line;
    public float range;
    public float rangeVariability;
    public float angleVariability;
    public LayerMask bulletHitMask;
    private int gunfireSourceIndex;
    private AudioSource[] gunfireAudioSources;

    public GameObject gunshotSounds;
    public AudioSource reloadSound;
    public AudioSource noAmmoSound;


    // Start is called before the first frame update
    void Start()
    {
        gunfireAudioSources = gunshotSounds.GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        fireRateCounter += Time.deltaTime;
        if(fireRateCounter > trailFadeTime)
        {
            HideBulletTrail();
        }
    }
    public void Shoot()
    {
        //play sound (multiple sources are cycled through so they can overlap)
        gunfireAudioSources[gunfireSourceIndex].pitch = UnityEngine.Random.Range(.8f, 1.2f);
        gunfireAudioSources[gunfireSourceIndex].Play();
        gunfireSourceIndex++;
        if (gunfireSourceIndex >= gunfireAudioSources.Length)
        {
             gunfireSourceIndex = 0;
        }


            //generate the angle and distance this bullet will shoot
            int trueAngle = UtilsClass.GetAngleFromVector(firePoint.up);
            Vector2 noisyAngle = UtilsClass.GetVectorFromAngle(trueAngle + (int)Random.Range(-angleVariability / 2, angleVariability / 2));
            float noisyRange = range + UnityEngine.Random.Range(-rangeVariability / 2, rangeVariability / 2);

            //fire the raycast
            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, noisyAngle, noisyRange, bulletHitMask);
            if (hitInfo)
            {
                line.SetPosition(0, firePoint.position);
                line.SetPosition(1, hitInfo.point);
                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Walls") || hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Obsticles"))
                {
                    try
                    {
                    Vector3 hitPosition = Vector3.zero;
                    hitPosition.x = hitInfo.point.x + noisyAngle.normalized.x * .01f;
                    hitPosition.y = hitInfo.point.y + noisyAngle.normalized.y * .01f; ;
                    hitInfo.collider.gameObject.GetComponent<DestructableTilemap>().BreakBlock(hitPosition);
                    }
                    catch { }
                }
                if(hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    hitInfo.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(noisyAngle.normalized * bulletKnockBack);
                    hitInfo.collider.gameObject.GetComponent<PlayerBase>().Damage(bulletDamage);
                }
                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
                {
                    hitInfo.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(noisyAngle.normalized * bulletKnockBack);
                    hitInfo.collider.gameObject.GetComponent<EnemyBehavior2>().Damage(bulletDamage, this.transform.parent.gameObject);
                }

            }
            else //draw bullet trail for missed bullet
            {
                Vector3 missPoistionFromBarrel = noisyRange * noisyAngle.normalized;
                line.SetPosition(0, firePoint.position);
                line.SetPosition(1, firePoint.position + missPoistionFromBarrel);
            }
    }
    void HideBulletTrail()
    {
        line.SetPosition(0,Vector3.zero);
        line.SetPosition(1,Vector3.zero);
    }

    public void PlayNoAmmoSound()
    {
        noAmmoSound.Play();
    }
}
