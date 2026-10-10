using UnityEngine;

public class EnemyCollectableDrop : MonoBehaviour
{
    [SerializeField]
    private  EnemyAttributes _enemyAttributes;

    private CollectableSpawner _collectableSpawner;

    private void Awake()
    {
        _collectableSpawner = FindObjectOfType<CollectableSpawner>();

    }

    public void RandomlyDropCollectable()
    {
        float random = Random.Range(0f, 1f);

        if(_enemyAttributes.ChanceOfCollectableDrop >= random)
        {
            _collectableSpawner.SpawnCollectable(transform.position);
        }
    }


}
