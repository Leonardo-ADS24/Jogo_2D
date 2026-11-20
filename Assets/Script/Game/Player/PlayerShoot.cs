using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private float _bulletSpeed;

    [SerializeField]
    private Transform _gunOffset;

    [SerializeField]
    private float _timeBetweenShots;

    [SerializeField]
    private bool _autoFire;

    [SerializeField]
    private AudioClip _defaultShotSound;   // som da arma inicial (pode ficar vazio = sem som)

    private bool _fireContinuously;
    private bool _fireSingle;
    private float _lastFireTime;

    //
    private int _damage = 10;
    private GameObject _defaultBulletPrefab;

    // shotgun
    private int _projectilesPerShot = 1;
    private float _spreadAngle = 0f;
    private float _bulletLifetime = 0f;

    // áudio
    private AudioSource _audioSource;
    private AudioClip _shotSound;
    private float _shotVolume = 1f;

    // Update is called once per frame
    void Update()
    {
        if (_autoFire || _fireContinuously || _fireSingle)
        {
            float timeSinceLastFire = Time.time - _lastFireTime;

            if (timeSinceLastFire >= _timeBetweenShots)
            {
                FireBullet();
                _lastFireTime = Time.time;
                _fireSingle = false;
            }
        }
    }

    private void FireBullet()
    {
        for (int i = 0; i < _projectilesPerShot; i++)
        {
            float angleOffset = 0f;
            if (_projectilesPerShot > 1)
            {
                float t = i / (float)(_projectilesPerShot - 1);
                angleOffset = Mathf.Lerp(-_spreadAngle / 2f, _spreadAngle / 2f, t);
            }

            Quaternion rotation = transform.rotation * Quaternion.Euler(0f, 0f, angleOffset);

            GameObject bullet = Instantiate(_bulletPrefab, _gunOffset.position, rotation);
            bullet.GetComponent<Bullet>().Damage = _damage;

            Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
            rigidbody.linearVelocity = _bulletSpeed * (rotation * Vector3.up);

            if (_bulletLifetime > 0f)
            {
                Destroy(bullet, _bulletLifetime);
            }
        }

        PlayShotSound();   // fora do laço: um som por disparo, e não um por projétil
    }

    private void PlayShotSound()
    {
        if (_shotSound != null)
        {
            _audioSource.pitch = Random.Range(0.94f, 1.06f);
            _audioSource.PlayOneShot(_shotSound, _shotVolume);
        }
    }

    private void OnFire(InputValue inputValue)
    {
        _fireContinuously = inputValue.isPressed;

        if (inputValue.isPressed)
        {
            _fireSingle = true;
        }
    }

    //
    private void Awake()
    {
        _defaultBulletPrefab = _bulletPrefab;

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        _audioSource.playOnAwake = false;
        _shotSound = _defaultShotSound;
    }

    public void EquipWeapon(WeaponData weapon)
    {
        _damage = weapon.Damage;
        _timeBetweenShots = weapon.TimeBetweenShots;
        _bulletSpeed = weapon.BulletSpeed;
        _bulletPrefab = weapon.BulletPrefab != null ? weapon.BulletPrefab : _defaultBulletPrefab;

        _projectilesPerShot = weapon.ProjectilesPerShot;
        _spreadAngle = weapon.SpreadAngle;
        _bulletLifetime = weapon.BulletLifetime;

        _shotSound = weapon.ShotSound != null ? weapon.ShotSound : _defaultShotSound;
        _shotVolume = weapon.ShotVolume;
    }
}