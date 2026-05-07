using Game.Collectibles.Data;
using UnityEngine;

public class RandomCollectibleSpawner : MonoBehaviour
{
    [Header("prefab")]
    [SerializeField] private SimpleCollectiblePickup pickupPrefab;

    [Header("collectible types")]
    [SerializeField] private CollectibleTypeSO pointsType;
    [SerializeField] private CollectibleTypeSO bombType;
    [SerializeField] private CollectibleTypeSO powerType;
    [SerializeField] private CollectibleTypeSO lifeType;

    [Header("spawn timing")]
    [SerializeField] private float minSpawnDelay = 2f;
    [SerializeField] private float maxSpawnDelay = 7f;

    [Header("spawn area")]
    [SerializeField] private Vector2 minPosition = new Vector2(-8f, -3.5f);
    [SerializeField] private Vector2 maxPosition = new Vector2(2f, 4f);

    [Header("weights")]
    [SerializeField] private float pointsWeight = 60f;
    [SerializeField] private float powerWeight = 25f;
    [SerializeField] private float bombWeight = 8f;
    [SerializeField] private float lifeWeight = 7f;

    private float spawnTimer;

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if(spawnTimer > 0f)
        {
            return;
        }

        SpawnRandomPickup();
        ResetTimer();
    }

    private void SpawnRandomPickup()
    {
        if(pickupPrefab == null)
        {
            Debug.LogWarning($"{nameof(RandomCollectibleSpawner)} has no pickup prefab", this);
            return;
        }

        CollectibleTypeSO selectedType = ChooseRandomType();

        if (selectedType == null)
        {
            return;
        }

        Vector2 position = new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y));

        SimpleCollectiblePickup pickup = Instantiate(pickupPrefab, new Vector3(position.x, position.y, 0.8f), Quaternion.identity);
        pickup.SetType(selectedType);
    }

    private CollectibleTypeSO ChooseRandomType()
    {
        float totalWeight = pointsWeight + powerWeight + bombWeight + lifeWeight;

        if(totalWeight <= 0f)
        {
            return null;
        }

        float roll = Random.Range(0f, totalWeight);

        if(roll < pointsWeight)
        {
            return pointsType;
        }
        roll -= pointsWeight;

        if (roll < powerWeight)
        {
            return powerType;
        }
        roll -= powerWeight;

        if (roll < bombWeight)
        {
            return bombType;
        }
        roll -= bombWeight;

        if (roll < lifeWeight)
        {
            return lifeType;
        }
        roll -= lifeWeight;

        return null;
        
    }

    private void ResetTimer()
    {
        spawnTimer = Random.Range(minSpawnDelay, maxSpawnDelay);
    }

}