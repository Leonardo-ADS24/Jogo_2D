using UnityEngine;
using UnityEngine.Events;

public class HordeTimer : MonoBehaviour
{
    [SerializeField]
    private float _durationInSeconds = 180f;   // 3 minutos

    public UnityEvent OnTimeChanged;   // dispara 1 vez por segundo
    public UnityEvent OnTimeEnded;     // dispara quando chega a zero

    public float TimeRemaining { get; private set; }
    public bool IsRunning { get; private set; }

    private int _lastShownSecond = -1;

    private void Start()
    {
        TimeRemaining = _durationInSeconds;
        IsRunning = true;
        OnTimeChanged.Invoke();
    }

    private void Update()
    {
        if (!IsRunning)
        {
            return;
        }

        TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            IsRunning = false;
            OnTimeChanged.Invoke();
            OnTimeEnded.Invoke();
            return;
        }

        int second = Mathf.CeilToInt(TimeRemaining);
        if (second != _lastShownSecond)
        {
            _lastShownSecond = second;
            OnTimeChanged.Invoke();
        }
    }

    public void StopTimer()
    {
        IsRunning = false;
    }
}