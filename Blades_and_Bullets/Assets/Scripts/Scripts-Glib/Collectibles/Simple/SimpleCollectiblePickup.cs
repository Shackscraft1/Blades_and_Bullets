using Game.Collectibles.Data;
using Game.Collectibles.Player;
using UnityEngine;

public class SimpleCollectiblePickup : MonoBehaviour
{
    [SerializeField] private CollectibleTypeSO collectibleType;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float lifetime = 12f;

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (spriteRenderer != null && collectibleType != null)
        {
            spriteRenderer.sprite = collectibleType.Sprite; //applies correct pickup sprite
        }

        Destroy(gameObject, lifetime);
    }

    public void SetType(CollectibleTypeSO type)
    {
        collectibleType = type;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null && collectibleType != null)
        {
            spriteRenderer.sprite = collectibleType.Sprite;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        PlayerResourceInventory inventory = other.GetComponent<PlayerResourceInventory>();

        if(inventory == null)
        {
            return;
        }

        inventory.ApplyCollectible(collectibleType);
        Destroy(gameObject);
    }
}