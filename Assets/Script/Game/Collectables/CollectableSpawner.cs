using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [System.Serializable]
    public class CollectableDrop
    {
        public GameObject Prefab;
        [Min(0f)] public float Weight = 1f;
    }

    [SerializeField]
    private List<CollectableDrop> _drops;

    public void SpawnCollectable(Vector2 position)
    {
        float totalWeight = 0f;
        foreach (var drop in _drops)
        {
            totalWeight += drop.Weight;
        }

        if (totalWeight <= 0f)
        {
            return;
        }

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var drop in _drops)
        {
            cumulative += drop.Weight;
            if (drop.Weight > 0f && roll <= cumulative)
            {
                Instantiate(drop.Prefab, position, Quaternion.identity);
                return;
            }
        }
    }
}