using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{

    [SerializeField]
    private EnemyAttributes _enemyAttributes;


    private HealthController _healthController;

    private void Awake()
    {
        _healthController = GetComponent<HealthController>();
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _healthController.SetHealth(_enemyAttributes.Health);
    }

}
