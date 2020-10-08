using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement & Rotation")]
    public float startMoveSpeed = .1f;
    public float moveSpeed = .1f;
    Vector2 movement = Vector2.zero;
    Vector2 aim = Vector2.zero;
    private float desiredRotation = 0;
    //move speed modifiers (msm)
    private float msmFromDamage = 1;

    [Header("References")]
    public ParticleSystem bloodTrail;
    private GunBehavior equippedGun;
    //PlayerController controls;
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
    private bool isShooting = false;
    private bool isScoped = false;
    private bool lastIsScoped = false;
    


    void Awake()
    {
        //controls = new PlayerController();
        //controls.Player.SecondaryAction.performed += ctx => isScoped = true;
        //controls.Player.SecondaryAction.canceled += ctx => isScoped = false;
        //controls.Player.PrimaryAction.performed += ctx => isShooting = true;
       // controls.Player.PrimaryAction.canceled += ctx => isShooting = false;
       // controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
       // controls.Player.Move.canceled += ctx => movement = Vector2.zero;
       // controls.Player.Aim.performed += ctx => aim = ctx.ReadValue<Vector2>();
       // controls.Player.Menu.performed += ctx => MenuButton();
    }
    void Start()
    {
        fieldOfView = GetComponentInChildren<FieldOfView>();
        rb = GetComponent<Rigidbody2D>();
        equippedGun = GetComponentInChildren<GunBehavior>();
        bloodTrail = GetComponent<ParticleSystem>();
        playerColor = GetComponent<SpriteRenderer>().color;
        ParticleSystem.MainModule bloodTrailMain = bloodTrail.main;
        bloodTrailMain.startColor = playerColor;
        health = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if (isShooting == true)
            equippedGun.Shoot();

        movement.Normalize();


        if(isScoped != lastIsScoped) // if scope status has changed
        {
            RecalculateMoveSpeed();
            scopeStartAngle = rb.rotation;
        }
        lastIsScoped = isScoped;
        //rotation dependent on scoped status
        if (isScoped)
        {
            desiredRotation = scopeStartAngle - Mathf.DeltaAngle(Vector2.SignedAngle(Vector2.up, aim),scopeStartAngle)*scopedRotationSensitivity;
        }
        else
        {
            desiredRotation = Vector2.SignedAngle(Vector2.up, aim);
        }
        rb.rotation = desiredRotation;
    }

    // Update is called 50 times per second
    void FixedUpdate()
    {
        //apply movement
        rb.MovePosition(rb.position + (movement * moveSpeed));
    }
    void MenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Damage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            health = 0;
            ///die
        }
        
        msmFromDamage = health / maxHealth;
        RecalculateMoveSpeed();
        ParticleSystem.EmissionModule bloodTrailEmission = bloodTrail.emission;
        bloodTrailEmission.rateOverTime = (maxHealth - health)/maxHealth * 5;

    }

    private void RecalculateMoveSpeed()
    {
        moveSpeed = startMoveSpeed * msmFromDamage;
        if (isScoped)
        {
            if (moveSpeed > startMoveSpeed * scopedMoveSpeedModifier)
            {
                moveSpeed = startMoveSpeed * scopedMoveSpeedModifier;
            }
        }
    }

    void OnEnable()
    {
        //controls.Player.Enable();
    }
    void OnDisable()
    {
        //controls.Player.Disable();
    }
}
