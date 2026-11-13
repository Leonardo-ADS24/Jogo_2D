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


    private bool _fireContinuously;
    private bool _fireSingle;
    private float _lastFireTime;
    //
    private int _damage = 10;
    private GameObject _defaultBulletPrefab;
    private int _projectilesPerShot = 1;
    private float _spreadAngle = 0f;
    private float _bulletLifetime = 0f;




    // Update is called once per frame
    void Update()
    {
        if(_autoFire || _fireContinuously || _fireSingle)
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
            // distribui os projéteis de forma uniforme dentro do leque
            float angleOffset = 0f;
            if (_projectilesPerShot > 1)
            {
                float t = i / (float)(_projectilesPerShot - 1);   // vai de 0 a 1
                angleOffset = Mathf.Lerp(-_spreadAngle / 2f, _spreadAngle / 2f, t);
            }

            Quaternion rotation = transform.rotation * Quaternion.Euler(0f, 0f, angleOffset);

            GameObject bullet = Instantiate(_bulletPrefab, _gunOffset.position, rotation);
            bullet.GetComponent<Bullet>().Damage = _damage;

            Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
            rigidbody.linearVelocity = _bulletSpeed * (rotation * Vector3.up);

            if (_bulletLifetime > 0f)
            {
                Destroy(bullet, _bulletLifetime);   // limita o alcance
            }
        }
    }

    //
    private void Awake()
    {
        _defaultBulletPrefab = _bulletPrefab;
    }

    public void EquipWeapon(WeaponData weapon)
    {
        _damage = weapon.Damage;
        _timeBetweenShots = weapon.TimeBetweenShots;
        _bulletSpeed = weapon.BulletSpeed;
        _bulletPrefab = weapon.BulletPrefab != null ? weapon.BulletPrefab : _defaultBulletPrefab;

        _projectilesPerShot = weapon.ProjectilesPerShot;   // novas
        _spreadAngle = weapon.SpreadAngle;
        _bulletLifetime = weapon.BulletLifetime;
    }
}
