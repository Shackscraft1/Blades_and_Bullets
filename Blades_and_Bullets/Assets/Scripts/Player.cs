using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{
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
    [SerializeField] private GameObject melee;
    [SerializeField] private GameObject bulletPrefab;
    private float shootTime = .1f;
    private float shootTimeMax = .07f;
    private MeshRenderer hitboxMesh;
    // private GameInput gameInput;


    private void Awake()
    {
        Instance = this;
        moveState = MoveState.Normal;
    }

    private void Start()
    {
        hitboxMesh = hitbox.GetComponent<MeshRenderer>();
        hitboxMesh.enabled = false;
        melee.SetActive(false);
    }
    private void Update()
    {
        HandleMovement();
        HandleInteraction();
        shootTime -= Time.deltaTime;
        if (shootTime <= 0)
        {
           HandleShoot();            
           shootTime = shootTimeMax;
        }

    }

    private void HandleInteraction()
    {
        if(Keyboard.current.zKey.isPressed || Keyboard.current.slashKey.isPressed)
        {
            melee.SetActive(true);
        }
    }
    private void HandleMovement()
    {
        Vector2 moveVector = new Vector2(0f, 0f);
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            moveVector.y = 1f;
            Debug.Log("W");
        } 
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            moveVector.y = -1f;
            Debug.Log("S");

        } 
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            moveVector.x = -1f;
            Debug.Log("A");

        } 
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveVector.x = 1f;
            Debug.Log("D");

        } 

        Vector3 endMoveVector = new Vector3(moveVector.x, moveVector.y, 0f).normalized;
        if (Keyboard.current.shiftKey.isPressed)
        {
            endMoveVector.x *= .4f;
            endMoveVector.y *= .4f;
            moveState = MoveState.Focused;
            hitboxMesh.enabled = true;
        } else
        {
            moveState = MoveState.Normal;
            hitboxMesh.enabled = false;

        }

        transform.position += endMoveVector * speed * Time.deltaTime;
        Debug.Log(moveState);

    }

    private void HandleShoot()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            switch (moveState)
            {
                default:
            case MoveState.Normal:
                Instantiate(bulletPrefab, transform.position, transform.rotation);
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, 20f)));
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, -20f)));
                break;
            case MoveState.Focused:
                Instantiate(bulletPrefab, transform.position, transform.rotation);
                Instantiate(bulletPrefab, transform.position + new Vector3(2f, 0f,0f), transform.rotation);
                Instantiate(bulletPrefab, transform.position + new Vector3(-2f, 0f,0f), transform.rotation);  
                break;     
            }
              
        }
    }

}
