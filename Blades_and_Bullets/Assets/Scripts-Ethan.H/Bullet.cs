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
    

    public Vector2 Direction { get; set; }
    public float Age => age;

    private int generation;
    private int maxSplit = 1;

    public void Init(BulletTypeSO bulletType, Vector2 direction, int amountSplit = 0)
    {
        bulletTypeSO = bulletType;
        Direction = direction.normalized;
        age = 0f;
        hasSplit = false;
        
        generation = amountSplit;

        movementType = null;
        if (bulletTypeSO != null)
        {
            movementType = bulletTypeSO.CreateMovement();
            movementType?.Init(this);
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void SpawnChild(Vector2 childDirection)
    {
        if (pool == null || bulletTypeSO.splitedBullet == null || bulletTypeSO.splitedBullet == null)
            return;

        Bullet child = pool.GetBullet();
        if (child == null)
        {
            return;
        }
        child.transform.position = transform.position;
        child.transform.rotation = Quaternion.identity;
        child.Init(bulletTypeSO.splitedBullet, childDirection, generation + 1);
    }
    private void Update()
    {
       
        age += Time.deltaTime;

        if (bulletTypeSO == null)
        {
            return;
        }

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

        if (pool == null)
        {
            pool.ReturnBulletToPool(this);
        }


    }

    



















}
