using Game.Collectibles.Data;
using UnityEngine;

namespace Game.Collectibles.Spawning
{
    public class EnemyCollectibleDropper : MonoBehaviour
    {
        [Header("Spawner Reference")]
        [SerializeField] private CollectibleSpawner collectibleSpawner; // central pooled spawning system used to create collectibles

        [Header("Collectible Types")]
        [SerializeField] private CollectibleTypeSO pointsCollectible;
        [SerializeField] private CollectibleTypeSO powerCollectible; // SO defining visuals + behavior for power pickups

        [Header("Point Drops")]
        [Range(0f, 1f)]
        [SerializeField] private float pointDropChance = 1f;

        [SerializeField] private int minPointDrops = 1;
        [SerializeField] private int maxPointDrops = 3;

        [Header("Power Drops")]
        [Range(0f, 1f)]
        [SerializeField] private float powerDropChance = 0.25f;

        [SerializeField] private int minPowerDrops = 0;
        [SerializeField] private int maxPowerDrops = 1;

        
        private void Awake()
        {
            
            if (collectibleSpawner == null)
            {
                collectibleSpawner = FindAnyObjectByType<CollectibleSpawner>();
            }
        }

        public void Drop()
        {
            Debug.Log($"{name} EnemyCollectibleDropper.Drop() called at {transform.position}", this); // confirms the dropper itself started running
            // prevents null-reference failures if the spawner was never found or assigned
            if (collectibleSpawner == null)
            {
                Debug.LogWarning("[EnemyCollectibleDropper] No CollectibleSpawner found.", this);
                return;
            }

            DropCollectibleType(
                pointsCollectible,
                pointDropChance,
                minPointDrops,
                maxPointDrops);

            DropCollectibleType(
                powerCollectible,
                powerDropChance,
                minPowerDrops,
                maxPowerDrops);
        }

        private void DropCollectibleType(
            CollectibleTypeSO collectibleType,
            float dropChance,
            int minDrops,
            int maxDrops)
        {
            Debug.Log($"Trying to drop {collectibleType?.name}, chance={dropChance}, min={minDrops}, max={maxDrops}", this); // confirms the configured SO and rarity values
            // skips this collectible type if no SO was assigned
            if (collectibleType == null)
            {
                return;
            }

            // random.value returns 0-1, so this performs rarity/chance logic
            if (Random.value > dropChance)
            {
                return;
            }

            // chooses a random quantity between the configured min/max inclusive
            int dropCount = Random.Range(minDrops, maxDrops + 1);

            for (int i = 0; i < dropCount; i++)
            {
                // creates a random outward scatter direction for pickup spread
                Vector2 scatterDirection = Random.insideUnitCircle.normalized;

                // spawns the collectible through the pooled collectible system
                Debug.Log($"Spawning {dropCount} of {collectibleType.name}", this); // confirms the rarity roll succeeded and count was chosen
                collectibleSpawner.Spawn(
                    collectibleType,
                    transform.position,
                    scatterDirection);
            }
        }
    }
}