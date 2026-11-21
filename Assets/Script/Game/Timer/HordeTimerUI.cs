using UnityEngine;
using TMPro;

public class HordeTimerUI : MonoBehaviour
{
    [SerializeField]
    private Color _normalColor = Color.white;

    [SerializeField]
    private Color _warningColor = Color.red;

    [SerializeField]
    private float _warningSeconds = 10f;   // vermelho nos últimos 10 s

    private TMP_Text _timerText;

    private void Awake()
    {
        _timerText = GetComponent<TMP_Text>();
    }

    public void UpdateTime(HordeTimer timer)
    {
        int total = Mathf.CeilToInt(timer.TimeRemaining);
        int minutes = total / 60;
        int seconds = total % 60;

        _timerText.text = $"{minutes:00}:{seconds:00}";
        _timerText.color = timer.TimeRemaining <= _warningSeconds ? _warningColor : _normalColor;
    }
}