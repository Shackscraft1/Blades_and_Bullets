using Game.Collectibles.Data;
using UnityEngine;

namespace Game.Collectibles.Spawning
{
    public sealed class EnemyCollectibleDropper : MonoBehaviour
    {
        [Header("references")]
        [SerializeField] private CollectibleSpawner collectibleSpawner; // spawns pooled collectibles through the real collectible system

        [Header("point drops")]
        [SerializeField] private CollectibleTypeSO pointsCollectible; // collectible data used for score drops
        [Range(0f, 1f)][SerializeField] private float pointDropChance = 1f; // 1 = always drops, 0 = never drops
        [Min(0)][SerializeField] private int minPointDrops = 1; // minimum if the drop roll succeeds
        [Min(0)][SerializeField] private int maxPointDrops = 3; // maximum if the drop roll succeeds

        [Header("power drops")]
        [SerializeField] private CollectibleTypeSO powerCollectible;
        [Range(0f, 1f)][SerializeField] private float powerDropChance = 0.35f;
        [Min(0)][SerializeField] private int minPowerDrops = 0;
        [Min(0)][SerializeField] private int maxPowerDrops = 1;

        [Header("scatter")]
        [SerializeField] private Vector2 baseDropDirection = Vector2.down; // default direction drops initially drift toward
        [Range(0f, 180f)][SerializeField] private float scatterAngle = 45f; // angle cone used to spread drops so they do not stack
        [SerializeField] private bool logDrops = false; // useful while balancing drop rates and counts

        public void Drop()
        {
            TryDropGroup(pointsCollectible, pointDropChance, minPointDrops, maxPointDrops, "points");
            TryDropGroup(powerCollectible, powerDropChance, minPowerDrops, maxPowerDrops, "power");
        }

        private void TryDropGroup(CollectibleTypeSO collectibleType, float dropChance, int minDrops, int maxDrops, string label)
        {
            if (collectibleSpawner == null)
            {
                Debug.LogWarning($"{nameof(EnemyCollectibleDropper)} on '{name}' has no collectible spawner assigned.", this);
                return;
            }

            if (collectibleType == null)
            {
                return; // allows enemies to intentionally have no point or power drop configured
            }

            if (maxDrops <= 0)
            {
                return; // zero max means this drop group is disabled
            }

            if (minDrops > maxDrops)
            {
                minDrops = maxDrops; // prevents invalid inspector setup from breaking Random.Range count selection
            }

            float roll = Random.value; // random value from 0 inclusive to 1 inclusive used for rarity balancing

            if (roll > dropChance)
            {
                return; // failed rarity roll, so this enemy drops none of this collectible type
            }

            int dropCount = Random.Range(minDrops, maxDrops + 1); // max is exclusive for int Random.Range, so +1 makes maxDrops possible

            for (int i = 0; i < dropCount; i++)
            {
                Vector2 driftDirection = GetScatterDirection(); // gives each pickup a slightly different initial drift direction
                collectibleSpawner.Spawn(collectibleType, transform.position, driftDirection);
            }

            if (logDrops)
            {
                Debug.Log($"{name} dropped {dropCount} {label} collectible(s).", this);
            }
        }

        private Vector2 GetScatterDirection()
        {
            Vector2 safeBaseDirection = baseDropDirection.sqrMagnitude > 0f ? baseDropDirection.normalized : Vector2.down; // avoids invalid zero-direction scatter
            float angle = Random.Range(-scatterAngle, scatterAngle); // creates a random spread angle around the base direction
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle); // rotates around z because this is 2D gameplay
            Vector2 scatteredDirection = rotation * safeBaseDirection; // applies the angle offset to the base drift direction

            return scatteredDirection.normalized; // keeps direction length at 1 so speed still comes from CollectibleTypeSO
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (minPointDrops > maxPointDrops)
            {
                maxPointDrops = minPointDrops;
            }

            if (minPowerDrops > maxPowerDrops)
            {
                maxPowerDrops = minPowerDrops;
            }

            if (baseDropDirection.sqrMagnitude <= 0f)
            {
                baseDropDirection = Vector2.down;
            }
        }
#endif
    }
}