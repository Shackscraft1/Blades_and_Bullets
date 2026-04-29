using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    // Event for Top Collection
    // Event for Power Ups
    // Lives, Bombs, Special with cooldown
    public static Player Instance{get; private set;}
    private enum MoveState
    {
        Normal,
        Focused
    }

    private MoveState moveState;

    // private enum AttackState
    // {
    //     Low,
    //     Medium,
    //     High
    // }

    // private AttackState attackState;
    
    [SerializeField] private float speed;
    [SerializeField] private GameObject hitbox;
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject slash;
    [SerializeField] private GameObject focusSlash;
    [SerializeField] private GameObject specialSlash;
    [SerializeField] private GameObject bombPrefab;




    // [SerializeField] private GameObject bulletPrefab;
    // private float swingTime = 0f;
    // private float swingTimeMax = 1f;
    // private float currentSwingTime = 0f;    
    private float bombCooldown;
    private float deathTimer;
    private SpriteRenderer hitboxMesh;
    private SpriteRenderer Playersprite;
    private Collider2D hitboxCollider;
    public int lives = 3;
    public int bombs = 3;
    public int points = 0;
    private bool inputEnabled = true;
    private float shootTime = .3f;
    private float shootTimeMax = .3f;
    private bool specialSlashAnimation = false;
    private float specialSlashTimeMax = 4f;
    private float specialSlashTime = 0f;
    private float specialSlashSingleTime = 0f;


    // Bullet pool to return
    // [SerializeField] BulletPool bulletPool;
    // To see if its a bullet
    // [SerializeField] Bullet bulletComp;

    
    
    //Special slash variables
    public static EventHandler<ModifyAbilityCooldownArgs> ModifyAbilityCooldown;
    public class ModifyAbilityCooldownArgs : EventArgs
    {
        public float changeAmount;
    }
    private bool _specialSlashActive;
    //Player gets hit logic
    public static EventHandler PlayerGetsHit;

    //Firing bullets Logic;
    public static EventHandler PlayerFiresBullet;
    

    private void Awake()
    {
        Instance = this;
        moveState = MoveState.Normal;
        // slash.SetActive(false);
        // focusSlash.SetActive(false);
    }

    private void Start()
    {
        GameControllerScript.AbilityActiveStatus += AbilityActiveStatus;
        hitboxMesh = hitbox.GetComponent<SpriteRenderer>();
        Playersprite = sprite.GetComponent<SpriteRenderer>();
        hitboxMesh.enabled = false;
    }

    private void AbilityActiveStatus(object sender, EventArgs e)
    {
        _specialSlashActive = true;
    }

    private void Update()
    {
        bombCooldown -= Time.deltaTime;
        shootTime -= Time.deltaTime;
        if (specialSlashTime > 0f)
        {
            specialSlashTime -= Time.deltaTime;
            specialSlashSingleTime -= Time.deltaTime;
            if (specialSlashSingleTime < 0f)
            {
                Instantiate(specialSlash, transform.position, transform.rotation, transform);
                specialSlashSingleTime = .005f;
            } 

            if (specialSlashTime <= 0f)
            {
                specialSlashAnimation = false;
            } 
        }

        if (inputEnabled)
        {
            HandleMovement();
            HandleInteraction();  
        } else if (lives > 0)
        {
            if (deathTimer < 0)
            {
                inputEnabled = true;
                hitboxCollider.enabled = true;
                Playersprite.enabled = true;
            } else
            {
                deathTimer -= Time.deltaTime;
            }
        } else
        {
           Time.timeScale = 0f; //When you lose pause game
        }
        
        if (shootTime <= 0f && !specialSlashAnimation)
        {
            HandleShoot();
        }
    }

    private void HandleInteraction()
    {
        if(Keyboard.current.zKey.isPressed || Keyboard.current.periodKey.wasPressedThisFrame)
        {
            SpecialSlash();
            //Handle firing player bullets here
            FireBullets();
        }
        
        if(Keyboard.current.bKey.wasPressedThisFrame || Keyboard.current.slashKey.wasPressedThisFrame)
        {   
            if (bombs > 0 && bombCooldown <= 0)
            {
                Instantiate(bombPrefab, transform.position, Quaternion.Euler(0f, 0f, 0f));
                bombCooldown = 6f;
                bombs--;
            } else
            {
                Debug.Log("No Bombs");
            }
        }
    }

    private void FireBullets()
    {
        PlayerFiresBullet?.Invoke(this, EventArgs.Empty);

    }

    private void FireBullets()
    {
        PlayerFiresBullet?.Invoke(this, EventArgs.Empty);

    }
    private void HandleMovement()
    {
        Vector2 moveVector = new Vector2(0f, 0f);
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            moveVector.y = 1f;
        } 
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            moveVector.y = -1f;
        } 
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            moveVector.x = -1f;
        } 
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveVector.x = 1f;
        } 

        Vector3 endMoveVector = new Vector3(moveVector.x, moveVector.y, 0f).normalized;
        if (Keyboard.current.shiftKey.isPressed)
        {
            endMoveVector.x *= .4f;
            endMoveVector.y *= .4f;
            moveState = MoveState.Focused;
            
        } else
        {
            moveState = MoveState.Normal;
            

        }
        transform.position += endMoveVector * speed * Time.deltaTime;

        if (transform.position.y > 20)
        {
            // Shoot event for Quick Collect
        }
    }
    // 

    // private void HandleSwing()
    // {
    //     if (Keyboard.current.spaceKey.isPressed)
    //     {
    //         switch (moveState)
    //         {
    //             default:
    //         case MoveState.Normal:
    //             slash.SetActive(true);
    //             currentSwingTime = .5f;
    //             break;
    //         case MoveState.Focused:
    //             focusSlash.SetActive(true);
    //             currentSwingTime = .5f;
    //             break;     
    //         }
    //         swingTime = swingTimeMax;
    //     }
    // }

    private void HandleShoot()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            switch (moveState)
            {
                default:
            case MoveState.Normal:
                Instantiate(slash, transform.position, transform.rotation);
                break;
            case MoveState.Focused:
                Instantiate(focusSlash, transform.position, transform.rotation);
                break;     
            }
            shootTime = shootTimeMax;
        }
    }

    private void SpecialSlash()
    {
        if (!_specialSlashActive)
        {
            //this invoke line of code will be temporary until the logic for obtaining ability charge is implemented
            ModifyAbilityCooldown?.Invoke(this, new ModifyAbilityCooldownArgs{changeAmount = .1f});
            // return;
        }
        // specialSlash.SetActive(true);
        // currentSwingTime = 3f;
        _specialSlashActive = false;
        ModifyAbilityCooldown?.Invoke(this, new ModifyAbilityCooldownArgs{changeAmount = 0f});
        specialSlashAnimation = true;
        specialSlashTime = specialSlashTimeMax;

    }

    public void Death()
    {
        // lives--;
        
        Instantiate(bombPrefab, transform.position, Quaternion.Euler(0f, 0f, 0f));
        bombCooldown = 8f;
        transform.position = new Vector3(-3f, -4f, transform.position.z);
        deathTimer = 2f;
        inputEnabled = false; //Changing from input false to hitbox disabled
        Playersprite.enabled = false;
        hitboxMesh.enabled = false;
        hitboxCollider = hitbox.GetComponent<Collider2D>();
        hitboxCollider.enabled = false;
        PlayerGetsHit?.Invoke(this, EventArgs.Empty);
        // Shoot Event
        // Death Animation
    }
    

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision != null)
    //     {
    //         //dequeuing bullets triggers an error, still trying to find a fix
    //         bulletPool.ReturnBulletToPool(collision.GetComponentInParent<Bullet>());
    //         Death();
    //         PlayerGetsHit?.Invoke(this, EventArgs.Empty);
    //     }
    // }



}
