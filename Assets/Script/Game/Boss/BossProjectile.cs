using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField]
    private float _lifetime = 6f;

    private int _damage;

    public void Launch(Vector2 direction, float speed, int damage)
    {
        _damage = damage;
        transform.up = direction;
        GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed;
        Destroy(gameObject, _lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>())
        {
            collision.GetComponent<HealthController>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}