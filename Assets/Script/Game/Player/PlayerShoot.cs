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
        GameObject bullet = Instantiate(_bulletPrefab, _gunOffset.position, transform.rotation);
        bullet.GetComponent<Bullet>().Damage = _damage;
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();

        rigidbody.linearVelocity = _bulletSpeed * transform.up;
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
    }

    public void EquipWeapon(WeaponData weapon)
    {
        _damage = weapon.Damage;
        _timeBetweenShots = weapon.TimeBetweenShots;
        _bulletSpeed = weapon.BulletSpeed;
        _bulletPrefab = weapon.BulletPrefab != null ? weapon.BulletPrefab : _defaultBulletPrefab;
    }
}
