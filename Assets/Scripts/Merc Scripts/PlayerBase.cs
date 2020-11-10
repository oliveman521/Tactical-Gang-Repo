using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerSupplyManager))]
public class PlayerBase : MonoBehaviour
{

    [Header("Movement & Rotation")]
    public float startMoveSpeed = .1f;
    public float moveSpeed = .1f;
    Vector2 movement;
    Vector2 aim;
    private float desiredRotation = 0;
    private float moveSpeedModifierFromDamage = 1;

    [Header("References")]
    public SpriteRenderer playerColorOverlay;
    private ParticleSystem bloodTrail;
    private Rigidbody2D rb = new Rigidbody2D();
    private Color playerColor;

    [Header("Health")]
    public float maxHealth = 10;
    public float health = 10;
    public bool down = false;

    [Header("DamageNumbers")]
    public GameObject damageNumberPrefab;

    [Header("Scope Settings")]
    public float scopedRotationSensitivity = 3f;
    public float scopedMoveSpeedModifier = .5f;
    private float scopeStartAngle;
    private bool isScoped = false;
    private bool lastIsScoped = false;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bloodTrail = GetComponent<ParticleSystem>();
        playerColor = playerColorOverlay.color;
        ParticleSystem.MainModule bloodTrailMain = bloodTrail.main;
        bloodTrailMain.startColor = playerColor;

        health = maxHealth;
        moveSpeed = startMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Move
        rb.velocity = (movement * moveSpeed);
        //Rotate
        ChoseDesiredRotation();
        rb.rotation = desiredRotation;


    }

    //Movement & rotation
    private void ChoseDesiredRotation()
    {
        if (isScoped != lastIsScoped) // if scope status has changed
        {
            RecalculateMoveSpeed();
            scopeStartAngle = rb.rotation;
        }
        lastIsScoped = isScoped;
        //rotation dependent on scoped status
        if (isScoped)
        {
            desiredRotation = scopeStartAngle - Mathf.DeltaAngle(Vector2.SignedAngle(Vector2.up, aim), scopeStartAngle) * scopedRotationSensitivity;
        }
        else
        {
            desiredRotation = Vector2.SignedAngle(Vector2.up, aim);
        }
    }
    private void RecalculateMoveSpeed()
    {
        moveSpeedModifierFromDamage = health / maxHealth;
        moveSpeed = startMoveSpeed * moveSpeedModifierFromDamage;
        if (isScoped)
        {
            if (moveSpeed > startMoveSpeed * scopedMoveSpeedModifier)
            {
                moveSpeed = startMoveSpeed * scopedMoveSpeedModifier;
            }
        }
    }

    //health
    public void ChangeHealth(float changeAmount)
    {
        if (changeAmount == 0) //save us some trouble if we aren't actally changing anything
            return;

        float damageNumber = changeAmount;

        health += changeAmount;
        if (health <= 0)
        {
            health = 0;
            down = true;
        }
        else
        {
            down = false;
        }

        if (health > maxHealth)
        {
            damageNumber = changeAmount - (health-maxHealth);
            health = maxHealth;
        }

        SpawnDamgeNumber(damageNumber, transform.position, true);

        RecalculateMoveSpeed();
        ParticleSystem.EmissionModule bloodTrailEmission = bloodTrail.emission;
        bloodTrailEmission.rateOverTime = (maxHealth - health) / maxHealth * 5;
    }
    public void SpawnDamgeNumber(float change,Vector3 position,bool isHealth)
    {
        GameObject newDamgeNumber = Instantiate(damageNumberPrefab);
        newDamgeNumber.GetComponent<DamageNumberBehavior>().Setup(change, position, isHealth);
    }
    public void OnScope(CallbackContext ctx)
    {
        if (ctx.performed)
            isScoped = true;
        if (ctx.canceled)
            isScoped = false;
    }
    public void Movement(CallbackContext value)
    {
        Vector2 moveDir = value.ReadValue<Vector2>();
        movement = moveDir;
    }
    public void Aim(CallbackContext value)
    {
        Vector2 aimVal = value.ReadValue<Vector2>();
        if (aimVal != Vector2.zero)
        {
            aim = aimVal;
        }
    }
    public void Restart(CallbackContext ctx)
    {
        if (ctx.started)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
