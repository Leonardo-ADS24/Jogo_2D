using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BossEntrance : MonoBehaviour
{
    [SerializeField]
    private Vector2 _targetPosition = new Vector2(0f, 2f);

    [SerializeField]
    private float _entranceDuration = 3f;

    public UnityEvent OnEntranceFinished;   // aqui vão entrar a barra de vida e os ataques

    private IEnumerator Start()
    {
        HealthController health = GetComponent<HealthController>();
        health.isInvincible = true;

        Vector2 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < _entranceDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / _entranceDuration);
            transform.position = Vector2.Lerp(startPosition, _targetPosition, t);
            yield return null;
        }

        transform.position = _targetPosition;
        health.isInvincible = false;
        OnEntranceFinished.Invoke();
    }
}