using System.Collections.Generic;
using UnityEngine;

namespace Game.Collectibles.Runtime
{
    public sealed class CollectiblePool : MonoBehaviour
    {
        [Header("pool setup")]
        [SerializeField] private Collectible collectiblePrefab; // prefab asset used as the template for pooled collectible objects
        [Min(1)][SerializeField] private int startingPoolSize = 16; // number of collectibles created when the scene starts

        private readonly Queue<Collectible> availableCollectibles = new Queue<Collectible>(); // inactive collectibles ready for reuse
        private Transform runtimePoolParent; // scene-only parent created/found at runtime so pooled objects never parent to a prefab asset

        public int AvailableCount => availableCollectibles.Count;

        private void Awake()
        {
            if (collectiblePrefab == null)
            {
                Debug.LogError($"{nameof(CollectiblePool)} on '{name}' is missing a collectible prefab reference.", this);
                return;
            }

            runtimePoolParent = GetOrCreateRuntimePoolParent(); // guarantees we use a real scene transform, not a prefab asset reference

            for (int i = 0; i < startingPoolSize; i++)
            {
                CreateAndStoreNewCollectible();
            }
        }

        public Collectible GetCollectible()
        {
            if (collectiblePrefab == null)
            {
                Debug.LogError($"{nameof(CollectiblePool)} cannot provide a collectible because no prefab is assigned.", this);
                return null;
            }

            if (runtimePoolParent == null)
            {
                runtimePoolParent = GetOrCreateRuntimePoolParent(); // rebuilds the runtime parent if something cleared it unexpectedly
            }

            if (availableCollectibles.Count == 0)
            {
                CreateAndStoreNewCollectible(); // expands the pool only when all prewarmed collectibles are currently in use
            }

            Collectible collectible = availableCollectibles.Dequeue();
            collectible.gameObject.SetActive(true);
            return collectible;
        }

        public void ReturnCollectibleToPool(Collectible collectible)
        {
            if (collectible == null)
            {
                Debug.LogWarning($"{nameof(CollectiblePool)} received a null collectible to return.", this);
                return;
            }

            if (runtimePoolParent == null)
            {
                runtimePoolParent = GetOrCreateRuntimePoolParent(); // ensures return logic always has a valid scene parent
            }

            collectible.transform.SetParent(runtimePoolParent, false); // reparents under the scene runtime container; false keeps local transform simple for pooled storage
            collectible.gameObject.SetActive(false);
            availableCollectibles.Enqueue(collectible);
        }

        private void CreateAndStoreNewCollectible()
        {
            Collectible newCollectible = Instantiate(collectiblePrefab, runtimePoolParent); // clones prefab into the active scene under the runtime parent
            newCollectible.name = $"{collectiblePrefab.name}_Pooled_{availableCollectibles.Count}";
            newCollectible.SetPool(this);
            newCollectible.gameObject.SetActive(false);
            availableCollectibles.Enqueue(newCollectible);
        }

        private Transform GetOrCreateRuntimePoolParent()
        {
            Transform existingChild = transform.Find("PooledObjects"); // uses a child in the current scene if one already exists

            if (existingChild != null)
            {
                return existingChild;
            }

            GameObject parentObject = new GameObject("PooledObjects"); // creates a real scene object, not a prefab asset child
            parentObject.transform.SetParent(transform, false);
            return parentObject.transform;
        }
    }
}