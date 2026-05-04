using UnityEngine;
using Game.Collectibles.Player;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject slash;
    [SerializeField] private GameObject focusSlash;
    [SerializeField] private GameObject specialSlash;
    [SerializeField] private float power = 1;
    private float shootTime = .3f;
    private float shootTimeMax = .1f;
    private bool specialSlashAnimation = false;
    private float specialSlashTimeMax = .5f;
    private float specialSlashTime = 0f;
    private float specialSlashCD;
    private GameObject slashInstance;
    private PlayerResourceInventory inventory;  
    
    private float damage = 2;

    void Start()
    {
        inventory = Player.Instance.GetComponent<PlayerResourceInventory>();
    }
    
    void Update()
    {
        shootTime -= Time.deltaTime;
        specialSlashTime -= Time.deltaTime;
        specialSlashCD -= Time.deltaTime;

        if (specialSlashTime < 0f)
        {
            specialSlash.SetActive(false);
            specialSlashAnimation = false;
        }


        if (Player.Instance.moveState != Player.MoveState.Death)
        {   
            if(Keyboard.current.zKey.isPressed || Keyboard.current.periodKey.wasPressedThisFrame && specialSlashCD < 0f)
            {
                SpecialSlash();
            }
            if (shootTime <= 0f && !specialSlashAnimation)
            {
                HandleShoot();
            }
        }
        power = Mathf.Clamp(inventory.Power, 2f, 100f);
        damage = Mathf.Lerp(2f, 20f, Mathf.InverseLerp(2f, 100f, inventory.Power));


    }
    private void SpecialSlash()
    {
        specialSlashAnimation = true;
        specialSlash.SetActive(true);
        specialSlashTime = specialSlashTimeMax;
        specialSlashCD = 10f;
    }

    private void HandleShoot()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            switch (Player.Instance.moveState)
            {
                default:
            case Player.MoveState.Normal:
                slashInstance = Instantiate(slash, transform.position, transform.rotation);
                slashInstance.GetComponent<SlashScript>().SetDamage(damage * 1f);
                break;
            case Player.MoveState.Focused:
                slashInstance = Instantiate(focusSlash, transform.position, transform.rotation);
                slashInstance.GetComponent<SlashScript>().SetDamage(damage * 1.3f);
                break;     
            }
            slashInstance.transform.localScale *= Mathf.Lerp(0.35f, 1f, Mathf.InverseLerp(2f, 100f, power));

            // FireBullets();
            shootTime = shootTimeMax;
        }
    }
}
