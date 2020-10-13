using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CodeMonkey.Utils;
using TMPro;

public class GunBehavior : MonoBehaviour
{
    [Header("Basic Gun Settings")]
    public bool fullAuto = true;
    public float bulletDamage;
    public float bulletKnockBack;
    public float fireRate;
    public float range;

    [Header("Advanced")]
    public float bulletsPerShot = 1;
    public LayerMask bulletHitMask;
    public float rangeVariability;
    public float angleVariability;
    public float trailFadeTime;
    private float timeSinceShotCounter;

    [Header("Audio")]
    public GameObject gunshotSounds;
    public AudioSource reloadSound;
    public AudioSource noAmmoSound;
    private AudioSource[] gunfireAudioSources;
    private int gunfireAudioSourceIndex = 0;

    [Header("Other References")]
    public Transform firePoint;
    public LineRenderer[] bulletTrailLines;
    private int bulletTrailIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        gunfireAudioSources = gunshotSounds.GetComponents<AudioSource>();
        if (bulletsPerShot > bulletTrailLines.Length)
            Debug.Log("Gun does not have enough line renderes for the number of shots you are trying to shoot at once. Will still function correctly, but will not visually match");
    }

    public void Shoot()
    {
        //play gunshot sound (multiple sources are cycled through so they can overlap)
        gunfireAudioSources[gunfireAudioSourceIndex].pitch = UnityEngine.Random.Range(.8f, 1.2f);
        gunfireAudioSources[gunfireAudioSourceIndex].Play();
        gunfireAudioSourceIndex++;
        if (gunfireAudioSourceIndex >= gunfireAudioSources.Length)
        {
             gunfireAudioSourceIndex = 0;
        }

        for (int i = 0; i < bulletsPerShot; i++)
        {
            //select a bullet trail line. multiple can be used so they are on screen at the same time.
            LineRenderer currentBulletTrailLine = bulletTrailLines[bulletTrailIndex];
            bulletTrailIndex++;
            if (bulletTrailIndex >= bulletTrailLines.Length)
                bulletTrailIndex = 0;

            //generate the angle and distance this bullet will shoot
            int trueAngle = UtilsClass.GetAngleFromVector(firePoint.up);
            Vector2 noisyAngle = UtilsClass.GetVectorFromAngle(trueAngle + (int)Random.Range(-angleVariability / 2, angleVariability / 2));
            float noisyRange = range + UnityEngine.Random.Range(-rangeVariability / 2, rangeVariability / 2);

            //fire the raycast
            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, noisyAngle, noisyRange, bulletHitMask);
            if (hitInfo)
            {
                currentBulletTrailLine.SetPosition(0, firePoint.position);
                currentBulletTrailLine.SetPosition(1, hitInfo.point);
                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Walls") || hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Obsticles"))
                {
                    try
                    {
                        Vector3 hitPosition = Vector3.zero;
                        hitPosition.x = hitInfo.point.x + noisyAngle.normalized.x * .01f;
                        hitPosition.y = hitInfo.point.y + noisyAngle.normalized.y * .01f; ;
                        hitInfo.collider.gameObject.GetComponent<DestructableTilemap>().ShootBlock(hitPosition, bulletDamage);
                    }
                    catch { }
                }
                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
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
                currentBulletTrailLine.SetPosition(0, firePoint.position);
                currentBulletTrailLine.SetPosition(1, firePoint.position + missPoistionFromBarrel);
            }
            StartCoroutine(HideBulletTrail(currentBulletTrailLine));
        }
    }
    IEnumerator HideBulletTrail(LineRenderer trail)
    {
        yield return new WaitForSeconds(trailFadeTime);
        trail.SetPosition(0,Vector3.zero);
        trail.SetPosition(1,Vector3.zero);
    }

    public void PlayNoAmmoSound()
    {
        noAmmoSound.Play();
    }
}
