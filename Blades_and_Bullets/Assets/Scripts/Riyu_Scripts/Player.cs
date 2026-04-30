using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
using Game.Collectibles.Player;

public class Player : MonoBehaviour
{
    public static Player Instance{get; private set;}
    public enum MoveState
    {
        Normal,
        Focused,
        Death
    }
    public MoveState moveState;
    [SerializeField] private float speed;
    [SerializeField] private GameObject bombPrefab;
    private float bombCooldown;
    private float deathTimer;
    private PlayerResourceInventory inventory;    
    
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
        inventory = GetComponent<PlayerResourceInventory>();
    }
    private void Start()
    {
        GameControllerScript.AbilityActiveStatus += AbilityActiveStatus;
        SlashScript.OnSlashingSomething +=OnSlashingSomething;
    }
    private void Update()
    {
        bombCooldown -= Time.deltaTime;
        deathTimer -= Time.deltaTime;

        if (deathTimer < 0f)
        {
            moveState = MoveState.Normal;
        }

        if (moveState != MoveState.Death) // not dead
        {
            HandleMovement();
            HandleInteraction();

        } else if (GameControllerScript.Instance.GetPlayerHP() < 0f) // dead check lives
        {
            Debug.Log("You Lost");
            Time.timeScale = 0f;
        }

    }

    private void HandleInteraction()
    {
        
        if(Keyboard.current.bKey.wasPressedThisFrame || Keyboard.current.slashKey.wasPressedThisFrame)
        {   
            if (inventory.Bombs > 0 && bombCooldown <= 0)
            {
                Instantiate(bombPrefab, transform.position, Quaternion.Euler(0f, 0f, 0f));
                bombCooldown = 6f;
                inventory.SubtractBomb();
            } 
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
    public void Death()
    {
        Instantiate(bombPrefab, transform.position, Quaternion.Euler(0f, 0f, 0f));
        bombCooldown = 8f;
        deathTimer = 2f;
        moveState = MoveState.Death;
        transform.position = new Vector3(-3f, -4f, transform.position.z);
        PlayerGetsHit?.Invoke(this, EventArgs.Empty);
    }
    private void OnSlashingSomething(object sender, SlashScript.OnSlashingSomethingArgs e)
    {
        ModifyAbilityCooldown?.Invoke(this, new ModifyAbilityCooldownArgs{changeAmount = .02f});
    }

    private void AbilityActiveStatus(object sender, EventArgs e)
    {
        _specialSlashActive = true;
    }

    private void OnDestroy()
    {
        SlashScript.OnSlashingSomething -=OnSlashingSomething;
        GameControllerScript.AbilityActiveStatus -= AbilityActiveStatus;
    }
}
