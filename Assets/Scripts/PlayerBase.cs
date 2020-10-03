using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

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
    private FieldOfView fieldOfView;
    private Rigidbody2D rb = new Rigidbody2D();
    private Color playerColor;


    [Header("Health")]
    public float maxHealth = 10;
    public float health = 10;

    [Header("Scope Settings")]
    public float scopedRotationSensitivity = 3f;
    public float scopedMoveSpeedModifier = .5f;
    private float scopeStartAngle;
    private bool isScoped = false;
    private bool lastIsScoped = false;



    void Start()
    {
        fieldOfView = GetComponentInChildren<FieldOfView>();
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
        rb.MovePosition(rb.position + (movement * moveSpeed * Time.deltaTime));
        //Rotate
        ChoseDesiredrotation();
        rb.rotation = desiredRotation;


        //update fov game object
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection(transform.rotation.eulerAngles.z + 90);
    }

    //Movement & rotation
    private void ChoseDesiredrotation()
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
    public void Damage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            health = 0;
            ///die
        }

        moveSpeedModifierFromDamage = health / maxHealth;
        RecalculateMoveSpeed();
        ParticleSystem.EmissionModule bloodTrailEmission = bloodTrail.emission;
        bloodTrailEmission.rateOverTime = (maxHealth - health) / maxHealth * 5;
    }

    public void OnScope(CallbackContext ctx)
    {
        if (ctx.performed)
            isScoped = true;
        if (ctx.canceled)
            isScoped = false;
    }
    public void Move(CallbackContext value)
    {
        movement = value.ReadValue<Vector2>();
    }
    public void Aim(CallbackContext value)
    {
        Vector2 aimVal = value.ReadValue<Vector2>();
        if (aimVal != Vector2.zero)
        {
            aim = aimVal;
        }
    }
}
