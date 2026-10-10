using UnityEngine;

public class EnemyScoreAllocator : MonoBehaviour
{
    [SerializeField]
    private EnemyAttributes _enemyAttributes;

    private ScoreController _scoreController;

    private void Awake()
    {
        _scoreController = FindObjectOfType<ScoreController>();
    }

    public void AllocateScore()
    {
        _scoreController.AddScore(_enemyAttributes.KillScore);
    }

}
