using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    private IMovementType movementType;
    private BulletTypeSO bulletTypeSO;
    private BulletPool pool;
    private bool isBulletActive;
    private float age;
    private bool hasSplit;
    private Vector2 direction;
    private Transform target;
    

    public Vector2 Direction => direction;
    public float Age => age;
    public Transform Target => target;

    private int generation;
    private int maxSplit = 1;

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    public void Init(BulletTypeSO bulletType, Vector2 AimDirection, int amountSplit = 0, Transform homingTarget = null)
    {
        bulletTypeSO = bulletType;
        direction = AimDirection.normalized;
        age = 0f;
        hasSplit = false;
        target = homingTarget;
        generation = amountSplit;

        movementType = null;

        if (bulletTypeSO.bulletMovement != null && bulletTypeSO != null) 
        {
            movementType = bulletTypeSO.bulletMovement.CreateMovement();
            movementType?.Init(this);
        }

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

  
        if (spriteRenderer != null)
        {
            
            spriteRenderer.sprite = bulletTypeSO != null ? bulletTypeSO.sprite : null;
        }
      

        if (bulletTypeSO != null)
        {
            transform.localScale = Vector3.one * bulletTypeSO.scale;
        }

        gameObject.SetActive(true);
    }

    public void SetPool(BulletPool bulletPool)
    {
        pool = bulletPool;
    }

    public void BulletSplit(Vector2 childDirection)
    {
        if (pool == null || bulletTypeSO == null || bulletTypeSO.splitedBullet == null)
            return;

        Bullet child = pool.GetBullet();
        if (child == null)
        {
            return;
        }
        child.transform.position = transform.position;
        child.transform.rotation = Quaternion.identity;
        child.Init(bulletTypeSO.splitedBullet, childDirection, generation + 1, target);
    }
    private void Update()
    {
       
  

        if (bulletTypeSO == null)
        {
            return;
        }

        age += Time.deltaTime;

        if (bulletTypeSO.canSplit && !hasSplit && age >= bulletTypeSO.splitTime)
        {
            if(generation < bulletTypeSO.splitMaxCount && bulletTypeSO.splitBulletPattern != null)
            {
                hasSplit = true;
                bulletTypeSO.splitBulletPattern.Split(this);
                DespawnBullet();
                return;
            }
        }


        if (age >= bulletTypeSO.lifetime) 
        { 
            
            DespawnBullet();
            return;
        }
        movementType?.Tick(this);

       
    }

    public void DespawnBullet()
    {

        gameObject.SetActive(false);

        if (pool != null)
        {
          
            pool.ReturnBulletToPool(this);
        }


    }

    


}
