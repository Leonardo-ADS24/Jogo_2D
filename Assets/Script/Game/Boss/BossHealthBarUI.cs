using TMPro;
using UnityEngine;

public class BossHealthBarUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform _fill;

    [SerializeField]
    private RectTransform _trail;

    [SerializeField]
    private TMP_Text _nameText;

    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private float _trailDelay = 0.5f;

    [SerializeField]
    private float _trailSpeed = 0.5f;     // fração da barra por segundo

    [SerializeField]
    private float _fadeSpeed = 2f;

    private HealthController _boss;
    private float _trailPercentage = 1f;
    private float _lastDamageTime;
    private float _targetAlpha;

    private void Awake()
    {
        _canvasGroup.alpha = 0f;
    }

    public void Show(HealthController boss, string bossName)
    {
        _boss = boss;
        _nameText.text = bossName;
        _trailPercentage = 1f;
        _fill.localScale = Vector3.one;
        _trail.localScale = Vector3.one;

        _boss.OnHealthChanged.AddListener(OnBossHealthChanged);
        _boss.OnDie.AddListener(Hide);

        _targetAlpha = 1f;
    }

    public void Hide()
    {
        _targetAlpha = 0f;
    }

    private void OnBossHealthChanged()
    {
        _lastDamageTime = Time.time;
        float percentage = Mathf.Clamp01(_boss.ReaminingHealtPercentage);
        _fill.localScale = new Vector3(percentage, 1f, 1f);
    }

    private void Update()
    {
        _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, _targetAlpha, _fadeSpeed * Time.deltaTime);

        if (_boss == null)
        {
            return;
        }

        float current = Mathf.Clamp01(_boss.ReaminingHealtPercentage);

        if (_trailPercentage > current && Time.time - _lastDamageTime > _trailDelay)
        {
            _trailPercentage = Mathf.MoveTowards(_trailPercentage, current, _trailSpeed * Time.deltaTime);
        }
        else if (_trailPercentage < current)
        {
            _trailPercentage = current;
        }

        _trail.localScale = new Vector3(_trailPercentage, 1f, 1f);
    }
}