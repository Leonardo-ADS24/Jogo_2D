using System.Collections;
using UnityEngine;

public class BossEncounterController : MonoBehaviour
{
    [Header("Horda")]
    [SerializeField]
    private EnemySpawner[] _spawners;

    [SerializeField]
    private bool _destroyRemainingEnemies = true;

    [Header("Chefe")]
    [SerializeField]
    private GameObject _bossPrefab;

    [SerializeField]
    private Transform _bossSpawnPoint;

    [SerializeField]
    private string _bossName = "VORAX";

    [Header("Interface")]
    [SerializeField]
    private BossHealthBarUI _bossHealthBar;

    [SerializeField]
    private VictoryController _victoryController;

    [Header("Aviso")]
    [SerializeField]
    private GameObject _warningObject;

    [SerializeField]
    private float _delayBeforeWarning = 1.5f;

    [SerializeField]
    private float _warningDuration = 3f;

    private bool _started;

    public void StartBossEncounter()
    {
        if (_started)
        {
            return;
        }

        _started = true;
        StartCoroutine(BossSequence());
    }

    private IEnumerator BossSequence()
    {
        foreach (var spawner in _spawners)
        {
            spawner.enabled = false;
        }

        if (_destroyRemainingEnemies)
        {
            ClearEnemies();
        }

        yield return new WaitForSeconds(_delayBeforeWarning);

        if (_warningObject != null)
        {
            _warningObject.SetActive(true);
            yield return new WaitForSeconds(_warningDuration);
            _warningObject.SetActive(false);
        }

        GameObject boss = Instantiate(_bossPrefab, _bossSpawnPoint.position, Quaternion.Euler(0f, 0f, 180f));
        HealthController bossHealth = boss.GetComponent<HealthController>();

        if (_bossHealthBar != null)
        {
            _bossHealthBar.Show(bossHealth, _bossName);
        }

        if (_victoryController != null)
        {
            bossHealth.OnDie.AddListener(_victoryController.OnBossDefeated);
        }
    }

    private void ClearEnemies()
    {
        foreach (var enemy in FindObjectsByType<EnemyMovent>(FindObjectsSortMode.None))
        {
            HealthController health = enemy.GetComponent<HealthController>();

            if (health != null)
            {
                health.TakeDamage(float.MaxValue);
            }
        }
    }
}