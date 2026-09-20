using UnityEngine;

public class EnemyScoreAllocator : MonoBehaviour
{
    [SerializeField]
    private int _killScore;

    private ScoreController _scoreController;

    private void Awake()
    {
        _scoreController = FindObjectOfType<ScoreController>();
    }

    public void AllocateScore()
    {
        _scoreController.AddScore(_killScore);
    }

}
