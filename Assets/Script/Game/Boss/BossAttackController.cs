using System.Collections;
using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField]
    private Transform[] _muzzles;               // pontas dos 2 canhões

    [SerializeField]
    private GameObject _projectilePrefab;

    [SerializeField]
    private GameObject _slamWarningPrefab;

    [SerializeField]
    private SpriteRenderer _graphics;

    [Header("Projéteis")]
    [SerializeField]
    private float _projectileSpeed = 6f;

    [SerializeField]
    private int _projectileDamage = 15;

    [Header("Rajada dupla")]
    [SerializeField]
    private int _burstShots = 6;

    [SerializeField]
    private float _burstInterval = 0.25f;

    [Header("Leque")]
    [SerializeField]
    private int _fanProjectiles = 7;

    [SerializeField]
    private float _fanAngle = 90f;

    [Header("Pisão")]
    [SerializeField]
    private float _slamRadius = 4f;

    [SerializeField]
    private float _slamWarningTime = 1f;

    [SerializeField]
    private int _slamDamage = 30;

    [Header("Ritmo")]
    [SerializeField]
    private float _telegraphTime = 0.7f;

    [SerializeField]
    private float _restTime = 1.5f;

    [Range(0f, 1f)]
    [SerializeField]
    private float _phaseTwoHealth = 0.5f;

    private HealthController _health;
    private BossMovement _movement;
    private Rigidbody2D _rigidbody;
    private Transform _player;
    private GameObject _activeSlamWarning;
    private int _lastAttack = -1;
    private bool _dead;

    private bool IsPhaseTwo
    {
        get { return _health.ReaminingHealtPercentage <= _phaseTwoHealth; }
    }

    private float TimeFactor
    {
        get { return IsPhaseTwo ? 0.65f : 1f; }
    }

    private void Awake()
    {
        _health = GetComponent<HealthController>();
        _movement = GetComponent<BossMovement>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _movement.enabled = false;   // só anda depois que a entrada terminar
    }

    // Ligar no evento OnEntranceFinished do BossEntrance
    public void StartFight()
    {
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        if (player != null)
        {
            _player = player.transform;
        }

        _movement.enabled = true;
        StartCoroutine(FightLoop());
    }

    private IEnumerator FightLoop()
    {
        yield return new WaitForSeconds(1f);

        while (!_dead)
        {
            int attack;
            do
            {
                attack = Random.Range(0, 3);
            }
            while (attack == _lastAttack);
            _lastAttack = attack;

            _movement.CanMove = false;

            switch (attack)
            {
                case 0:
                    yield return StartCoroutine(BurstAttack());
                    break;
                case 1:
                    yield return StartCoroutine(FanAttack());
                    break;
                default:
                    yield return StartCoroutine(SlamAttack());
                    break;
            }

            _movement.CanMove = true;

            yield return new WaitForSeconds(_restTime * TimeFactor);
        }
    }

    // ---------- Ataques ----------

    private IEnumerator BurstAttack()
    {
        yield return StartCoroutine(Telegraph(_telegraphTime * TimeFactor));

        int shots = _burstShots + (IsPhaseTwo ? 3 : 0);

        for (int i = 0; i < shots; i++)
        {
            Transform muzzle = _muzzles[i % _muzzles.Length];
            Fire(muzzle.position, DirectionToPlayer(muzzle.position));
            yield return new WaitForSeconds(_burstInterval * TimeFactor);
        }
    }

    private IEnumerator FanAttack()
    {
        yield return StartCoroutine(Telegraph(_telegraphTime * TimeFactor));

        int count = _fanProjectiles + (IsPhaseTwo ? 2 : 0);
        Vector3 origin = transform.position + transform.up * 1.3f;
        Vector2 center = DirectionToPlayer(origin);
        int gap = Random.Range(1, count - 1);   // deixa um vão para o jogador passar

        for (int i = 0; i < count; i++)
        {
            if (i == gap)
            {
                continue;
            }

            float t = i / (float)(count - 1);
            float angle = Mathf.Lerp(-_fanAngle / 2f, _fanAngle / 2f, t);
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * center;
            Fire(origin, direction);
        }
    }

    private IEnumerator SlamAttack()
    {
        _activeSlamWarning = Instantiate(_slamWarningPrefab, transform.position, Quaternion.identity);
        _activeSlamWarning.transform.localScale = Vector3.one * _slamRadius * 2f;

        yield return StartCoroutine(Telegraph(_slamWarningTime * TimeFactor));

        Destroy(_activeSlamWarning);

        foreach (Collider2D hit in Physics2D.OverlapCircleAll(transform.position, _slamRadius))
        {
            if (hit.GetComponent<PlayerMovement>())
            {
                hit.GetComponent<HealthController>().TakeDamage(_slamDamage);
            }
        }
    }

    // ---------- Apoio ----------

    private IEnumerator Telegraph(float duration)
    {
        float elapsed = 0f;
        Color warningColor = new Color(1f, 0.45f, 0.45f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _graphics.color = Color.Lerp(Color.white, warningColor, Mathf.PingPong(elapsed * 6f, 1f));
            yield return null;
        }

        _graphics.color = Color.white;
    }

    private void Fire(Vector3 origin, Vector2 direction)
    {
        GameObject projectile = Instantiate(_projectilePrefab, origin, Quaternion.identity);
        projectile.GetComponent<BossProjectile>().Launch(direction, _projectileSpeed, _projectileDamage);
    }

    private Vector2 DirectionToPlayer(Vector3 from)
    {
        if (_player == null)
        {
            return transform.up;
        }

        return (_player.position - from).normalized;
    }

    // ---------- Morte (ligar no OnDie do HealthController) ----------

    public void OnBossDied()
    {
        _dead = true;
        StopAllCoroutines();

        _movement.enabled = false;
        _rigidbody.linearVelocity = Vector2.zero;

        GetComponent<Collider2D>().enabled = false;

        BossContactDamage contact = GetComponent<BossContactDamage>();
        if (contact != null)
        {
            contact.enabled = false;
        }

        if (_activeSlamWarning != null)
        {
            Destroy(_activeSlamWarning);
        }

        foreach (BossProjectile projectile in FindObjectsByType<BossProjectile>(FindObjectsSortMode.None))
        {
            Destroy(projectile.gameObject);
        }

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        for (int i = 0; i < 12; i++)
        {
            _graphics.color = i % 2 == 0 ? Color.red : Color.white;
            yield return new WaitForSeconds(0.12f);
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            _graphics.color = new Color(1f, 1f, 1f, 1f - t);
            yield return null;
        }

        Destroy(gameObject);
    }
}