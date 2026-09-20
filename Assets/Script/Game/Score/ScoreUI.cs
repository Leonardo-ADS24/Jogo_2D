using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private TMP_Text _scoreText;

    private void Awake()
    {
        _scoreText = GetComponent<TMP_Text>();
    }

    public void UpdateScore(ScoreController scoreController)
    {
        _scoreText.text = $"Score: {scoreController.Score}";
    }
}
