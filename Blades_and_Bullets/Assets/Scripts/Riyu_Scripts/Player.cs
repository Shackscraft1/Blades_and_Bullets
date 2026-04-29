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
    [SerializeField] private GameObject slash;
    [SerializeField] private GameObject focusSlash;
    [SerializeField] private GameObject specialSlash;
    [SerializeField] private GameObject bombPrefab;


    // [SerializeField] private GameObject bulletPrefab;
    private float swingTime = 0f;
    private float swingTimeMax = 1f;
    private float bombCooldown;
    private float currentSwingTime = 0f;
    private float deathTimer;
    
    // private GameInput gameInput
    public int lives = 3;
    public int bombs = 3;
    public int points = 0;
    private bool inputEnabled = true;

    //Bullet pool to return
    [SerializeField]
    BulletPool bulletPool;
    //To see if its a bullet
    [SerializeField]
    Bullet bulletComp;

    
    
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
        slash.SetActive(false);
        focusSlash.SetActive(false);
    }

    private void Start()
    {
        GameControllerScript.AbilityActiveStatus += AbilityActiveStatus;
        SlashScript.OnSlashingSomething += OnSlashingSomething;
        GameControllerScript.OnPlayerDeath += OnPlayerDeath;
 
    }

    private void OnPlayerDeath(object sender, EventArgs e)
    {
        GameControllerScript.OnPlayerDeath -= OnPlayerDeath;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameControllerScript.AbilityActiveStatus -= AbilityActiveStatus;
        SlashScript.OnSlashingSomething -= OnSlashingSomething;
        
    }

    private void OnSlashingSomething(object sender, SlashScript.OnSlashingSomethingArgs e)
    {
        ModifyAbilityCooldown?.Invoke(this, new ModifyAbilityCooldownArgs{changeAmount = .03f});
        
    }

    private void AbilityActiveStatus(object sender, EventArgs e)
    {
        _specialSlashActive = true;
    }

    private void Update()
    {
        bombCooldown -= Time.deltaTime;
        swingTime -= Time.deltaTime;

        if (inputEnabled)
        {
            HandleMovement();
            HandleInteraction();  
        } else if (lives > 0)
        {
            if (deathTimer < 0)
            {
                inputEnabled = true;
            } else
            {
                deathTimer -= Time.deltaTime;
            }
        } else
        {
            Debug.Log("You Lost");
           Time.timeScale = 0f; //When you lose pause game
        }
        
        if (swingTime <= 0 && currentSwingTime <= 0 && inputEnabled)
        {
           HandleSwing();            
        }
        if(currentSwingTime > 0)
        {
            currentSwingTime -= Time.deltaTime;
        } else
        {
            slash.SetActive(false);
            focusSlash.SetActive(false);
            specialSlash.SetActive(false);
        }
        // Debug.Log(bombCooldown);
       // Debug.Log("Lives: " + lives + " , Bombs: " + bombs);
    }

    private void HandleInteraction()
    {
        if(Keyboard.current.zKey.isPressed || Keyboard.current.periodKey.wasPressedThisFrame)
        {
            //Handle firing player bullets here
            FireBullets();
        }
        if(Keyboard.current.cKey.isPressed || Keyboard.current.cKey.wasPressedThisFrame)
        {
            SpecialSlash();
        }
        
        if(Keyboard.current.bKey.wasPressedThisFrame || Keyboard.current.slashKey.wasPressedThisFrame)
        {   
            if (bombs > 0 && bombCooldown <= 0)
            {
                Instantiate(bombPrefab, transform.position, Quaternion.Euler(90f, 0f, 0f));
                bombCooldown = 6f;
                bombs--;
            } else
            {
                Debug.Log("No Bombs");
            }
        }
        // Test Keybinds

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            lives--;
        }

        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            Death();
        }


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

    private void HandleSwing()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            switch (moveState)
            {
                default:
            case MoveState.Normal:
                slash.SetActive(true);
                currentSwingTime = .5f;
                break;
            case MoveState.Focused:
                focusSlash.SetActive(true);
                currentSwingTime = .5f;
                break;     
            }
            swingTime = swingTimeMax;
        }
    }

    private void SpecialSlash()
    {
        if (!_specialSlashActive) return;
        specialSlash.SetActive(true);
        currentSwingTime = 1f;
        _specialSlashActive = false;
        ModifyAbilityCooldown?.Invoke(this, new ModifyAbilityCooldownArgs{changeAmount = 0f});
    }

    private void Death()
    {
        lives--;
        Instantiate(bombPrefab, transform.position, Quaternion.Euler(90f, 0f, 0f));
        bombCooldown = 8f;
        inputEnabled = false; //Changing from input false to hitbox disabled
        deathTimer = 4f;
        // Shoot Event
        // Death Animation
    }
    
    






    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponentInParent<Bullet>() != null)
        {
            
            PlayerGetsHit?.Invoke(this, EventArgs.Empty);
        }
        


    }



}
